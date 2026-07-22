using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Order
{
    public class SelectProductItemModel : PageModel
    {
        private readonly IApplicationProductItem _applicationProductItem;
        private readonly IResultMessage _resultMessage;

        public SelectProductItemModel(IApplicationProductItem applicationProductItem, IResultMessage resultMessage)
        {
            _applicationProductItem = applicationProductItem;
            _resultMessage = resultMessage;
        }

        public List<ProductItemViewModel> ProductItems { get; set; }

        [BindProperty]
        public List<long> ProductItemIds { get; set; }

        public void OnGet()
        {
            ProductItems = _applicationProductItem.GetAllReadyToSell();
        }

        public IActionResult OnPost()
        {
            if (!ProductItemIds.Any())
            {
                TempData["Message"] = _resultMessage.Error("حداقل یک آیتم را انتخاب کنید!");
                return RedirectToPage();
            }
            return RedirectToPage(
                "/Order/SelectCustomer",
                new
                {
                    ProductItemIds
                });
        }
    }
}
