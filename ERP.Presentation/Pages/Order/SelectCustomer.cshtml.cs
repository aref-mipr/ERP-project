using ERP.Application.Contract.CustomerAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Order
{
    [Authorize]
    public class SelectCustomerModel : PageModel
    {
        private readonly IApplicationCustomer _applicationCustomer;
        private readonly IResultMessage _resultMessage;

        public SelectCustomerModel(IApplicationCustomer applicationCustomer, IResultMessage resultMessage)
        {
            _applicationCustomer = applicationCustomer;
            _resultMessage = resultMessage;
        }

        public List<CustomerViewModel> Customers { get; set; }

        [BindProperty]
        public int CustomerId { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<long> ProductItemIds { get; set; }

        [BindProperty]
        public int Id { get; set; }

        public void OnGet(int id)
        {
            ViewData["PageTitle"] = "مدیریت سفارش ها";
            ViewData["OrderActive"] = "active";
            Customers = _applicationCustomer.GetAll();
            Id = id;
        }

        public IActionResult OnPost(int id)
        {
            if(CustomerId == 0)
            {
                TempData["Message"] = _resultMessage.Error("یک مشتری را انتخاب کنید!");
                return RedirectToPage(new {CustomerId, ProductItemIds });
            }
            return RedirectToPage(
                "/Order/Create",
                new {CustomerId, ProductItemIds, id });
        }
    }
}
