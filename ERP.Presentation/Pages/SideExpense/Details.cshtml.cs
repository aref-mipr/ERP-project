using ERP.Application.Contract.SideExpenseAgg;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.SideExpense
{
    public class DetailsModel : PageModel
    {
        private readonly IApplicationSideExpense _applicationSideExpense;
        public DetailsModel(IApplicationSideExpense applicationSideExpense)
        {
            _applicationSideExpense = applicationSideExpense;
        }

        public SideExpenseViewModel SideExpense { get; set; }
        public void OnGet(int id)
        {
            ViewData["PageTitle"] = "مدیریت هزینه های جانبی";
            ViewData["SideExpenseActive"] = "active";
            SideExpense = _applicationSideExpense.GetBy(id);
        }
    }
}
