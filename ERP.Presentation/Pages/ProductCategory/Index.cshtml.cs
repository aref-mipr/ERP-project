using ERP.Application.Contract.FilterAgg;
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
        public FilterParamsDto FilterParams { get; set; }
        public SearchViewModel Search { get; set; }

        public void OnGet(int pageId = 1, string? search = "")
        {
            ViewData["PageTitle"] = "مدیریت دسته بندی ها";
            ViewData["ProductCategoryActive"] = "active";
            const int take = 15;

            var count = _applicationProduct.GetCount(search);

            var pageCount = (int)Math.Ceiling((double)count / take);

            if (pageCount < 1)
                pageCount = 1;

            if (pageId < 1)
                pageId = 1;

            if (pageId > pageCount)
                pageId = pageCount;

            var filterParamsCriteria = new FilterParamsCriteria
            {
                Take = take,
                PageCount = pageCount,
                PageId = pageId,
                Subject = search
            };

            FilterParams = new FilterParamsDto(filterParamsCriteria);
            Search = new SearchViewModel
            {
                FilterParams = FilterParams
            };
            ProductCategories = _applicationProductCategory.GetAll(FilterParams);
            TempData["NumberItems"] = _applicationProductCategory.GetCount();
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
