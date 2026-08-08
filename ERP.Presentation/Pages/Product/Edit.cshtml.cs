using ERP.Application.Contract.ProductAgg;
using ERP.Application.Contract.ProductCategoryAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP.Presentation.Pages.Product
{
    public class EditModel : PageModel
    {
        private readonly IApplicationProduct _applicationProduct;
        private readonly IApplicationProductCategory _applicationProductCategory;
        private readonly IRepositoryBudget _repositoryBudget;
        private readonly IResultMessage _resultMessage;
        public EditModel(IApplicationProduct applicationProduct, IResultMessage resultMessage,
            IApplicationProductCategory applicationProductCategory, IRepositoryBudget repositoryBudget)
        {
            _applicationProduct = applicationProduct;
            _resultMessage = resultMessage;
            _applicationProductCategory = applicationProductCategory;
            _repositoryBudget = repositoryBudget;
        }

        [BindProperty]
        public EditProductDto Command { get; set; }

        [BindProperty]
        public decimal LastCostPrice { get; set; }

        public SelectList CategoriesList { get; set; }

        public void OnGet(int id)
        {
            ViewData["PageTitle"] = "مدیریت محصولات";
            ViewData["ProductActive"] = "active";
            Command = _applicationProduct.GetForEdit(id);
            CategoriesList = new SelectList(_applicationProductCategory.GetAll().Where(x => x.IsActive == true), "Id", "Name");

            LastCostPrice = Command.ProductCriterias.CostPrice;
        }

        public IActionResult OnPost()
        {
            var x = Command.ProductCriterias.StockQuantity * (Command.ProductCriterias.CostPrice - LastCostPrice);
            if (_repositoryBudget.GetLast().TotalBudget < (Command.ProductCriterias.StockQuantity * (Command.ProductCriterias.CostPrice - LastCostPrice)))
            {
                TempData["Message"] = _resultMessage.Error("عدم بودجه کافی");
                CategoriesList = new SelectList(_applicationProductCategory.GetAll().Where(x => x.IsActive == true), "Id", "Name");
                return Page();
            }
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("خطا در ویرایش محصول");
                CategoriesList = new SelectList(_applicationProductCategory.GetAll().Where(x => x.IsActive == true), "Id", "Name");
                return Page();
            }

            _applicationProduct.Edit(Command);
            TempData["Message"] = _resultMessage.Success("این محصول با موفقیت ویرایش شد");
            return RedirectToPage("Index");
        }
    }
}
