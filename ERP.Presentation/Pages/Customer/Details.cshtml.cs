using ERP.Application.Contract.CustomerAgg;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Customer
{
    public class DetailsModel : PageModel
    {
        private readonly IApplicationCustomer _applicationCustomer;
        public DetailsModel(IApplicationCustomer applicationCustomer)
        {
            _applicationCustomer = applicationCustomer;
        }

        public CustomerViewModel Customer { get; set; }
        public void OnGet(int id)
        {
            Customer = _applicationCustomer.GetBy(id);
        }
    }
}
