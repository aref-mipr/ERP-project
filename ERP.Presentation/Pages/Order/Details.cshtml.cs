using ERP.Application.Contract.OrderAgg;
using ERP.Application.Contract.OrderItemAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static ERP.Domain.Entity.OrderModel;

namespace ERP.Presentation.Pages.Order
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IApplicationOrder _applicationOrder;
        private readonly IRepositoryOrder _repositoryOrder;
        private readonly IApplicationOrderItem _applicationOrderItem;
        private readonly IRepositoryOrderItem _repositoryOrderItem;
        private readonly IRepositoryBudget _repositoryBudget;
        private readonly IResultMessage _resultMessage;
        public DetailsModel(IApplicationOrder applicationOrder, IApplicationOrderItem applicationOrderItem,
            IRepositoryOrderItem repositoryOrderItem, IRepositoryOrder repositoryOrder,
            IRepositoryBudget repositoryBudget, IResultMessage resultMessage)
        {
            _applicationOrder = applicationOrder;
            _applicationOrderItem = applicationOrderItem;
            _repositoryOrderItem = repositoryOrderItem;
            _repositoryOrder = repositoryOrder;
            _repositoryBudget = repositoryBudget;
            _resultMessage = resultMessage;
        }

        public OrderViewModel Order { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }

        public void OnGet(int id)
        {
            ViewData["PageTitle"] = "مدیریت سفارش ها";
            ViewData["OrderActive"] = "active";
            Order = _applicationOrder.GetBy(id);
            OrderItems = _applicationOrderItem.GetAllBy(id);

            TempData["NumberItems"] = OrderItems.Count();
            TempData["DiscountPercent"] = "Model.Order.DiscountAmount / Model.Order.FinalAmount * 100";
            var orderStatus = _repositoryOrder.GetBy(id).OrderStatus;
            if(orderStatus == OrderStatuses.Approved)
                TempData["Approved"] = true;

            if (orderStatus == OrderStatuses.Pending)
                TempData["Pending"] = true;
        }

        public IActionResult OnGetReturned(long id, int orderId)
        {
            if(_repositoryOrderItem.GetBy(id).Price > _repositoryBudget.GetLast().TotalBudget)
            {
                TempData["Message"] = _resultMessage.Error("عدم بودجه کافی");
                return Page();
            }
            _applicationOrderItem.Return(id);
            TempData["Message"] = _resultMessage.Success("آیتم با موفقیت مرجوع شد");
            return RedirectToPage(new { id = orderId });
        }

        public IActionResult OnGetEdit(int id)
        {
            if(_repositoryOrder.GetBy(id).OrderStatus != OrderStatuses.Pending)
            {
                TempData["ErrorMessage"] = _resultMessage.Error("سفارش تایید یا رد شده را نمی توانید ویرایش کنید");
                return RedirectToPage("Index");
            }
            return RedirectToPage("SelectProductItem", new {id});
        }
    }
}
