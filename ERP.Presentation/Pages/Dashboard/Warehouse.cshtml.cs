using ERP.Application.Contract.FilterAgg;
using ERP.Application.Contract.ProductItemAgg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Dashboard
{
    [Authorize]
    public class WarehouseModel : PageModel
    {
        private readonly IApplicationProductItem _appliationProductItem;
        public WarehouseModel(IApplicationProductItem appliationProductItem)
        {
            _appliationProductItem = appliationProductItem;
        }

        public SearchViewModel Search { get; set; }
        public FilterParamsDto FilterParams { get; set; }

        public List<ProductItemViewModel> ProductItems { get; set; }
        public void OnGet(int pageId = 0, string? search = "")
        {
            ViewData["PageTitle"] = "لیست انبار";
            ViewData["WarehouseActive"] = "active";

            const int take = 15;
            int count = _appliationProductItem.GetCountInWarehouse(search);
            int pageCount = (int)Math.Ceiling((double)count / take);

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

            ProductItems = _appliationProductItem.GetIAlltemsInWarehouse(FilterParams);
            TempData["NumberItems"] = _appliationProductItem.GetCountInWarehouse();
        }
    }
}
