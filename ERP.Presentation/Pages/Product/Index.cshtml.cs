using ERP.Application.Contract.FilterAgg;
using ERP.Application.Contract.ProductAgg;
using ERP.Application.Contract.ProductItemAgg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Product
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IApplicationProduct _applicationProduct;
        private readonly IApplicationProductItem _applicationProductItem;
        public IndexModel(IApplicationProduct applicationProduct, IApplicationProductItem applicationProductItem)
        {
            _applicationProduct = applicationProduct;
            _applicationProductItem = applicationProductItem;
        }

        public List<ProductViewModel> Products { get; set; }

        public FilterParamsDto FilterParams  { get; set; }
        public SearchViewModel Search { get; set; }
        public void OnGet(int pageId = 1, string? search = "")
        {
            ViewData["PageTitle"] = "مدیریت محصولات";
            ViewData["ProductActive"] = "active";
            TempData["Subject"] = "نام محصول";

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
            Products = _applicationProduct.GetAll(FilterParams);

            TempData["NumberItems"] = _applicationProduct.GetCount();
        }

        public async Task<JsonResult> OnGetItemsByProductId(int productId)
        {
            var productItems = _applicationProductItem.GetAllBy(productId);

            return new JsonResult(productItems);
        }
    }
}
