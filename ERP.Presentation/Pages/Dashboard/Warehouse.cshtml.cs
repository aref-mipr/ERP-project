using ERP.Application.Contract.ProductItemAgg;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Dashboard
{
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
            ProductItems = _appliationProductItem.GetIAlltemsInWarehouse();
            TempData["NumberItems"] = ProductItems.Count();
        }
    }
}
