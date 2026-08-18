using ERP.Application.Contract.ProductItemAgg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.ProductItem
{
    [Authorize]
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
            ViewData["PageTitle"] = "مدیریت محصولات";
            ViewData["ProductActive"] = "active";
            ProductItems = _appliationProductItem.GetAll();
            TempData["NumberItems"] = ProductItems.Count();
        }
    }
}
