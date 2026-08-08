using ERP.Application.Contract.BudgetAgg;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Budget
{
    public class IndexModel : PageModel
    {
        private readonly IApplicationBudget _applicationBudget;
        public IndexModel(IApplicationBudget applicationBudget)
        {
            _applicationBudget = applicationBudget;
        }

        public List<BudgetViewModel> Budgets { get; set; }

        public void OnGet()
        {
            ViewData["PageTitle"] = "مدیریت سرمایه";
            ViewData["BudgetActive"] = "active";
            Budgets = _applicationBudget.GetAll();
            TempData["NumberItems"] = Budgets.Count();
        }
    }
}
