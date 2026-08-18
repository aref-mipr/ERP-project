using ERP.Application.Contract.CustomerAgg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Customer
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IApplicationCustomer _applicationCustomer;
        public IndexModel(IApplicationCustomer applicationCustomer)
        {
            _applicationCustomer = applicationCustomer;
        }

        public List<CustomerViewModel> Customers { get; set; }

        public void OnGet()
        {
            ViewData["PageTitle"] = "مدیریت مشتریان";
            ViewData["CustomerActive"] = "active";
            Customers = _applicationCustomer.GetAll();
            TempData["NumberItems"] = Customers.Count();
        }
    }
}
