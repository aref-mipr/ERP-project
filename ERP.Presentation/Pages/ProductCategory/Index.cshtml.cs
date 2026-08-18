using ERP.Application.Contract.ProductAgg;
using ERP.Application.Contract.ProductCategoryAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.ProductCategory
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IApplicationProductCategory _applicationProductCategory;
        private readonly IApplicationProduct _applicationProduct;
        private readonly IResultMessage _resultMessage;
        public IndexModel(IApplicationProductCategory applicationProductCategory, IResultMessage resultMessage, IApplicationProduct applicationProduct)
        {
            _applicationProductCategory = applicationProductCategory;
            _resultMessage = resultMessage;
            _applicationProduct = applicationProduct;
        }

        public List<ProductCategoryViewModel> ProductCategories { get; set; }

        public void OnGet()
        {
            ViewData["PageTitle"] = "مدیریت دسته بندی ها";
            ViewData["ProductCategoryActive"] = "active";
            ProductCategories = _applicationProductCategory.GetAll();
            TempData["NumberItems"] = _applicationProductCategory.GetAll().Count();
        }

        public RedirectToPageResult OnGetRemove(int id)
        {
            _applicationProductCategory.Remove(id);
            TempData["Message"] = _resultMessage.Success("این دسته بندی با موفقیت غیرفعال شد");
            return RedirectToPage("Index");
        }
        public RedirectToPageResult OnGetRestore(int id)
        {
            _applicationProductCategory.Restore(id);
            TempData["Message"] = _resultMessage.Success("این دسته بندی با موفقیت فعال شد");
            return RedirectToPage("Index");
        }

        public async Task<JsonResult> OnGetProductsByCategoryId(int categoryId)
        {
            var products = _applicationProduct.GetProductsByCategoryId(categoryId);

            return new JsonResult(products);
        }

    }
}
