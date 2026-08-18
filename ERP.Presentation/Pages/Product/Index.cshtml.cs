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
        public void OnGet()
        {
            ViewData["PageTitle"] = "مدیریت محصولات";
            ViewData["ProductActive"] = "active";
            Products = _applicationProduct.GetAll();
            TempData["NumberItems"] = Products.Count;
        }

        public async Task<JsonResult> OnGetItemsByProductId(int productId)
        {
            var productItems = _applicationProductItem.GetAllBy(productId);

            return new JsonResult(productItems);
        }
    }
}
