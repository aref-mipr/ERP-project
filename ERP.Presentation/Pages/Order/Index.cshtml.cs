using ERP.Application.Contract.OrderAgg;
using ERP.Application.Contract.OrderItemAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using static ERP.Domain.Entity.OrderModel;

namespace ERP.Presentation.Pages.Order
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IApplicationOrder _applicationOrder;
        private readonly IApplicationOrderItem _applicationOrderItem;
        private readonly IEnumExtension _enumExtension;
        private readonly IResultMessage _resultMessage;
        public IndexModel(IApplicationOrder applicationOrder, IApplicationOrderItem applicationOrderItem,
            IEnumExtension enumExtension, IResultMessage resultMessage)
        {
            _applicationOrder = applicationOrder;
            _applicationOrderItem = applicationOrderItem;
            _enumExtension = enumExtension;
            _resultMessage = resultMessage;
        }

        public List<OrderViewModel> Orders { get; set; }
        public SelectList StatusesList { get; set; }

        [BindProperty]
        public OrderStatuses Status { get; set; }

        public void OnGet()
        {
            ViewData["PageTitle"] = "مدیریت سفارش ها";
            ViewData["OrderActive"] = "active";
            Orders = _applicationOrder.GetAll();
            TempData["NumberItems"] = Orders.Count;
            var statuses = _applicationOrder.CreateStatuses()
                .Where(x => x.Text != _enumExtension.OrderStatusesToPersianString(OrderStatuses.Pending));
            TempData["PendingStatus"] = _enumExtension.OrderStatusesToPersianString(OrderStatuses.Pending);
            StatusesList = new SelectList(statuses, "Value", "Text");
        }

        public RedirectToPageResult OnPost(int id)
        {
            _applicationOrder.ChangeStatus(id, Status);
            TempData["Message"] = _resultMessage.Success("وضعیت سفارش با موفقیت تغییر کرد");
            return RedirectToPage();
        }

        public async Task<JsonResult> OnGetItemsByOrderId(int orderId)
        {
            var orderItems = _applicationOrderItem.GetAllBy(orderId);

            return new JsonResult(orderItems);
        }
    }
}

