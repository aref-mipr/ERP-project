using ERP.Application.Contract.CustomerAgg;
using ERP.Application.Contract.OrderAgg;
using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Order
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IApplicationProductItem _applicationProductItem;
        private readonly IApplicationCustomer _applicationCustomer;
        private readonly IApplicationOrder _applicationOrder;
        private readonly IResultMessage _resultMessage;

        public CreateModel(
            IApplicationProductItem applicationProductItem, IApplicationCustomer applicationCustomer,
            IApplicationOrder applicationOrder, IResultMessage resultMessage)
        {
            _applicationProductItem = applicationProductItem;
            _applicationCustomer = applicationCustomer;
            _applicationOrder = applicationOrder;
            _resultMessage = resultMessage;
        }

        public List<ProductItemViewModel> ProductItems { get; set; }

        public CustomerViewModel Customer { get; set; }

        [BindProperty]
        public CreateOrderDto Command { get; set; }

        [BindProperty]
        public int Id { get; set; }

        public void OnGet(
            int customerId,
            List<long> productItemIds,
            int id)
        {
            ViewData["PageTitle"] = "مدیریت سفارش ها";
            ViewData["OrderActive"] = "active";
            ProductItems = _applicationProductItem.GetAll()
                .Where(x => productItemIds.Contains(x.Id)).ToList();

            Customer = _applicationCustomer.GetBy(customerId);
            Id = id;

            Command = new CreateOrderDto();
            Command.Id = id;
            Command.ProductItemIds = productItemIds;
            Command.OrdersCriteria = new OrderCriteria();
            Command.OrdersCriteria.CustomerId = customerId;
            Command.OrdersCriteria.InitialAmount = ProductItems.Sum(x => x.Price);
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("سفارش ثبت نشد");
                return Page();
            }

            if (id == 0)
                _applicationOrder.Create(Command);
            else
                _applicationOrder.Edit(Command);

            TempData["Message"] = _resultMessage.Success("سفارش با موفقیت ثبت شد");
            return RedirectToPage("Index");
        }
    }
}
