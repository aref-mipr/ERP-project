using ERP.Application.Contract.CustomerAgg;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Customer
{
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
            Customers = _applicationCustomer.GetAll();
            TempData["NumberItems"] = Customers.Count();
        }
    }
}
