using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using static ERP.Domain.Entity.ProductItemModel;

namespace ERP.Presentation.Pages.ProductItem
{
    public class EditModel : PageModel
    {
        private readonly IApplicationProductItem _applicationProductItem;
        private readonly IRepositoryProductItem _repositoryProductItem;
        private readonly IResultMessage _resultMessage;
        private readonly IEnumExtension _enumExtension;
        public EditModel(IApplicationProductItem applicationProductItem, IRepositoryProductItem repositoryProductItem, 
            IResultMessage resultMessage, IEnumExtension enumExtension)
        {
            _applicationProductItem = applicationProductItem;
            _repositoryProductItem = repositoryProductItem;
            _resultMessage = resultMessage;
            _enumExtension = enumExtension;
        }

        [BindProperty]
        public EditProductItemDto Command { get; set; }

        public SelectList StatusesList { get; set; }
        public void OnGet(long id)
        {
            Command = _applicationProductItem.GetForEdit(id);
            if(Command.ProductItemCriterias.ProductItemStatus != ProductItemStatuses.Returned &&
                Command.ProductItemCriterias.ProductItemStatus != ProductItemStatuses.ThrownOut &&
                Command.ProductItemCriterias.ProductItemStatus != ProductItemStatuses.WaitingOrder &&
                Command.ProductItemCriterias.ProductItemStatus != ProductItemStatuses.Selled)
            {
                TempData["Actived"] = true;
            }
            var statuses = _applicationProductItem.CreateStatuses()
                .Where(x => x.Text != _enumExtension.ItemStatusesToPersianString(ProductItemStatuses.Selled) &&
                x.Text != _enumExtension.ItemStatusesToPersianString(ProductItemStatuses.WaitingOrder));
            StatusesList = new SelectList(statuses, "Value", "Text");
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("خطا در ثبت محصول");
                return Page();
            }
            _applicationProductItem.Edit(Command);
            TempData["Message"] = _resultMessage.Success("این محصول با موفقیت ویرایش شد");
            long productId = _repositoryProductItem.GetBy(Command.Id).ProductId;
            return RedirectToPage("/Product/Details", new { id = productId });
        }
    }
}
