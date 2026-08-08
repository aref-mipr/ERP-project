using ERP.Application.Contract.OrderItemAgg;
using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Order
{
    public class SelectProductItemModel : PageModel
    {
        private readonly IApplicationProductItem _applicationProductItem;
        private readonly IApplicationOrderItem _applicationOrderItem;
        private readonly IResultMessage _resultMessage;

        public SelectProductItemModel(IApplicationProductItem applicationProductItem,
            IApplicationOrderItem applicationOrderItem, IResultMessage resultMessage)
        {
            _applicationProductItem = applicationProductItem;
            _applicationOrderItem = applicationOrderItem;
            _resultMessage = resultMessage;
        }

        public List<ProductItemViewModel> ProductItems { get; set; }

        [BindProperty]
        public List<long> ProductItemIds { get; set; }

        [BindProperty]
        public int Id { get; set; }

        public void OnGet(int id)
        {
            ViewData["PageTitle"] = "مدیریت سفارش ها";
            ViewData["OrderActive"] = "active";
            ProductItems = _applicationProductItem.GetAllReadyToSell();
            Id = id;
            var orderItems = _applicationOrderItem.GetAllWaitingOrderBy(id);
            List<ProductItemViewModel> editProductItems = new List<ProductItemViewModel>();
            foreach (var orderItem in orderItems)
            {
                editProductItems.Add(_applicationProductItem.GetBy(orderItem.ProductItemId));
            }
            ProductItems.AddRange(editProductItems);
        }

        public IActionResult OnPost(int id)
        {
            if (!ProductItemIds.Any())
            {
                TempData["Message"] = _resultMessage.Error("حداقل یک آیتم را انتخاب کنید!");
                return RedirectToPage();
            }
            return RedirectToPage(
                "/Order/SelectCustomer",
                new
                {ProductItemIds, id});
        }
    }
}
