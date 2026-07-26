using ERP.Application.Contract.ProductAgg;
using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static ERP.Domain.Entity.ProductItemModel;

namespace ERP.Presentation.Pages.Product
{
    public class DetailsModel : PageModel
    {
        private readonly IApplicationProduct _applicationProduct;
        private readonly IApplicationProductItem _applicationProductItem;
        private readonly IEnumExtension _enumExtension;
        public DetailsModel(IApplicationProduct applicationProduct, IApplicationProductItem applicationProductItem,
            IEnumExtension enumExtension)
        {
            _applicationProduct = applicationProduct;
            _applicationProductItem = applicationProductItem;
            _enumExtension = enumExtension;
        }

        public ProductViewModel Product { get; set; }
        public List<ProductItemViewModel> ProductItems { get; set; }

        public void OnGet(int id)
        {
            Product = _applicationProduct.GetBy(id);
            ProductItems = _applicationProductItem.GetAllBy(id);
            TempData["NumberItems"] = ProductItems.Count();
            TempData["NumberItemsInStock"] = ProductItems
                .Where(x => x.ProductItemStatus == _enumExtension.ItemStatusesToPersianString(ProductItemStatuses.Approved)).Count();
        }
    }
}
