using ERP.Application.Contract.CustomerAgg;
using ERP.Application.Contract.OrderAgg;
using ERP.Application.Contract.OrderItemAgg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Customer
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IApplicationCustomer _applicationCustomer;
        private readonly IApplicationOrder _applicationOrder;
        private readonly IApplicationOrderItem _applicationOrderItem;
        public DetailsModel(IApplicationCustomer applicationCustomer, IApplicationOrder applicationOrder,
             IApplicationOrderItem applicationOrderItem)
        {
            _applicationCustomer = applicationCustomer;
            _applicationOrder = applicationOrder;
            _applicationOrderItem = applicationOrderItem;
        }

        public CustomerViewModel Customer { get; set; }
        public List<OrderViewModel> Orders { get; set; }
        public void OnGet(int id)
        {
            ViewData["PageTitle"] = "مدیریت مشتریان";
            ViewData["CustomerActive"] = "active";
            Customer = _applicationCustomer.GetBy(id);
            Orders = _applicationOrder.GetAllBy(id);
            TempData["NumberItems"] = Orders.Count();
        }

        public async Task<JsonResult> OnGetItemsByOrderId(int orderId)
        {
            var orderItems = _applicationOrderItem.GetAllBy(orderId);

            return new JsonResult(orderItems);
        }
    }
}
