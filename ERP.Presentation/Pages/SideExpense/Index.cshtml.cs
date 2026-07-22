using ERP.Application.Contract.SideExpenseAgg;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.SideExpense
{
    public class IndexModel : PageModel
    {
        private readonly IApplicationSideExpense _applicationSideExpense;
        public IndexModel(IApplicationSideExpense applicationSideExpense)
        {
            _applicationSideExpense = applicationSideExpense;
        }

        public List<SideExpenseViewModel> SideExpenses { get; set; }

        public void OnGet()
        {
            SideExpenses = _applicationSideExpense.GetAll();
            TempData["NumberItems"] = SideExpenses.Count();
        }
    }
}
