using ERP.Application.Contract.ProductAgg;
using ERP.Application.Contract.ProductCategoryAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP.Presentation.Pages.Product
{
    public class CreateModel : PageModel
    {
        private readonly IApplicationProduct _applicationProduct;
        private readonly IApplicationProductCategory _applicationProductCategory;
        private readonly IRepositoryBudget _repositoryBudget;
        private readonly IResultMessage _resultMessage;
        public CreateModel(IApplicationProduct applicationProduct, IResultMessage resultMessage,
            IApplicationProductCategory applicationProductCategory, IRepositoryBudget repositoryBudget)
        {
            _applicationProduct = applicationProduct;
            _resultMessage = resultMessage;
            _applicationProductCategory = applicationProductCategory;
            _repositoryBudget = repositoryBudget;
        }

        [BindProperty]
        public CreateProductDto Command { get; set; }

        public SelectList CategoriesList { get; set; }

        public void OnGet()
        {
            CategoriesList = new SelectList(_applicationProductCategory.GetAll().Where(x => x.IsActive == true), "Id", "Name");
        }

        public RedirectToPageResult OnPost()
        {
            if(_repositoryBudget.GetLast().TotalBudget < ( Command.ProductCriterias.StockQuantity * Command.ProductCriterias.CostPrice))
            {
                TempData["Message"] = _resultMessage.Error("عدم بودجه کافی");
                return RedirectToPage();
            }
            ModelState.Remove("Command.ProductItemCriterias");
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("خطا در ثبت محصول");
                return RedirectToPage();
            }

            _applicationProduct.Create(Command);
            TempData["Message"] = _resultMessage.Success("این محصول با موفقیت افزوده شد");
            return RedirectToPage("Index");
        }
    }
}
