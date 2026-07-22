using ERP.Application.Contract.ProductItemAgg;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.ProductItem
{
    public class IndexModel : PageModel
    {
        private readonly IApplicationProductItem _appliationProductItem;
        public IndexModel(IApplicationProductItem appliationProductItem)
        {
            _appliationProductItem = appliationProductItem;
        }

        public List<ProductItemViewModel> ProductItems { get; set; }
        public void OnGet()
        {
            ProductItems = _appliationProductItem.GetAll();
            TempData["NumberItems"] = ProductItems.Count();
        }
    }
}
