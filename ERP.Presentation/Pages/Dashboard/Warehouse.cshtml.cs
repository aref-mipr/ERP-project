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

        public List<ProductItemViewModel> ProductItems { get; set; }
        public void OnGet()
        {
            ViewData["PageTitle"] = "لیست انبار";
            ViewData["WarehouseActive"] = "active";
            ProductItems = _appliationProductItem.GetIAlltemsInWarehouse();
            TempData["NumberItems"] = ProductItems.Count();
        }
    }
}
