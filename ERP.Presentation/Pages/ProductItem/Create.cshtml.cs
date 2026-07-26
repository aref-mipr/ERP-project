using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.ProductItem
{
    public class CreateModel : PageModel
    {
        private readonly IApplicationProductItem _applicationProductItem;
        private readonly IRepositoryProduct _repositoryProduct;
        private readonly IResultMessage _resultMessage;
        private readonly IRepositoryBudget _repositoryBudget;
        public CreateModel(IApplicationProductItem applicationProductItem, IResultMessage resultMessage,
            IRepositoryProduct repositoryProduct, IRepositoryBudget repositoryBudget)
        {
            _applicationProductItem = applicationProductItem;
            _repositoryProduct = repositoryProduct;
            _resultMessage = resultMessage;
            _repositoryBudget = repositoryBudget;
        }

        [BindProperty]
        public CreateProductItemDto Command { get; set; }

        public void OnGet(int productId)
        {
            Command = _applicationProductItem.GetBy(productId);
        }

        public IActionResult OnPost()
        {
            if(_repositoryProduct.GetBy(Command.ProductItemCriterias.ProductId).CostPrice > _repositoryBudget.GetLast().TotalBudget)
            {
                TempData["Message"] = _resultMessage.Error("عدم بودجه کافی");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("خطا در ثبت محصول");
                return Page();
            }

            _applicationProductItem.Create(Command);
            TempData["Message"] = _resultMessage.Success("آیتم با موفقیت ثبت شد");
            int id = Command.ProductItemCriterias.ProductId;
            return RedirectToPage("/Product/Details", new { id });
        }
    }
}
