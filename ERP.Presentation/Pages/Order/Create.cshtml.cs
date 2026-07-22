using ERP.Application.Contract.CustomerAgg;
using ERP.Application.Contract.OrderAgg;
using ERP.Application.Contract.ProductItemAgg;
using ERP.Application.Service;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Order
{
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

        public void OnGet(
            int customerId,
            List<long> productItemIds)
        {
            ProductItems = _applicationProductItem.GetAllReadyToSell()
                .Where(x => productItemIds.Contains(x.Id)).ToList();

            Customer = _applicationCustomer.GetBy(customerId);

            Command = new CreateOrderDto();
            Command.ProductItemIds = productItemIds;
            Command.OrdersCriteria = new OrderCriteria();
            Command.OrdersCriteria.CustomerId = customerId;
            Command.OrdersCriteria.InitialAmount = ProductItems.Sum(x => x.ProductItemCriterias.Price);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("سفارش ثبت نشد");
                return Page();
            }
            _applicationOrder.Create(Command);
            TempData["Message"] = _resultMessage.Success("سفارش با موفقیت ثبت شد");
            return RedirectToPage("Index");
        }
    }
}
