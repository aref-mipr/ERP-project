using ERP.Application.Contract.FilterAgg;
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
        public FilterParamsDto FilterParams { get; set; }
        public SearchViewModel Search { get; set; }

        public void OnGet(int pageId = 1, string? search = "")
        {
            ViewData["PageTitle"] = "مدیریت سفارش ها";
            ViewData["OrderActive"] = "active";
            var statuses = _applicationOrder.CreateStatuses()
                .Where(x => x.Text != _enumExtension.OrderStatusesToPersianString(OrderStatuses.Pending));
            TempData["PendingStatus"] = _enumExtension.OrderStatusesToPersianString(OrderStatuses.Pending);
            StatusesList = new SelectList(statuses, "Value", "Text");

            const int take = 15;
            int count = _applicationOrder.GetCount(search);
            int pageCount = (int)Math.Ceiling((double)count / take);

            if (pageCount < 1)
                pageCount = 1;

            if (pageId < 1)
                pageId = 1;

            if (pageId > pageCount)
                pageId = pageCount;

            var filterParamsCriteria = new FilterParamsCriteria
            {
                Take = take,
                PageCount = pageCount,
                PageId = pageId,
                Subject = search
            };

            FilterParams = new FilterParamsDto(filterParamsCriteria);
            Search = new SearchViewModel
            {
                FilterParams = FilterParams
            };

            Orders = _applicationOrder.GetAll(FilterParams);
            TempData["NumberItems"] = _applicationOrder.GetCount();
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

