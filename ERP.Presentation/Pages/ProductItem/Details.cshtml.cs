using ERP.Application.Contract.ProductAgg;
using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static ERP.Domain.Entity.ProductItemModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ERP.Presentation.Pages.ProductItem
{
    public class DetailsModel : PageModel
    {
        private readonly IApplicationProductItem _applicationProductItem;
        private readonly IEnumExtension _enumExtension;
        public DetailsModel(IApplicationProductItem applicationProductItem, IEnumExtension enumExtension)
        {
            _applicationProductItem = applicationProductItem;
            _enumExtension = enumExtension;
        }

        public ProductItemViewModel ProductItem { get; set; }

        public void OnGet(int id)
        {
            ProductItem = _applicationProductItem.GetBy(id);

            if (ProductItem.ProductItemStatus == _enumExtension.ItemStatusesToPersianString(ProductItemStatuses.Approved))
                TempData["StatusStyle"] = "bg-success";

            else if (ProductItem.ProductItemStatus == _enumExtension.ItemStatusesToPersianString(ProductItemStatuses.Returned) ||
                ProductItem.ProductItemStatus == _enumExtension.ItemStatusesToPersianString(ProductItemStatuses.ThrownOut))
                TempData["StatusStyle"] = "bg-danger";

            else if (ProductItem.ProductItemStatus == _enumExtension.ItemStatusesToPersianString(ProductItemStatuses.Selled))
                TempData["StatusStyle"] = "bg-warning text-dark";

            else
                TempData["StatusStyle"] = "bg-secondary";
        }

        public RedirectToPageResult OnGetRedirectToProduct()
        {
            long productId = _applicationProductItem.GetBy(ProductItem.Id).ProductItemCriterias.ProductId;
            return RedirectToPage("/Product/Details", new { id = productId });
        }
    }
}
