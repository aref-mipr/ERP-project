using ERP.Application.Contract.FinancialTransactionAgg;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.FinancialTransaction
{
    public class BudgetsModel : PageModel
    {
        private readonly IApplicationFinancialTransaction _applicationFinancialTransaction;
        public BudgetsModel(IApplicationFinancialTransaction applicationFinancialTransaction)
        {
            _applicationFinancialTransaction = applicationFinancialTransaction;
        }

        public List<FinancialTransactionViewModel> Budgets { get; set; }

        public void OnGet()
        {
            ViewData["PageTitle"] = "مدیریت سرمایه";
            ViewData["BudgetActive"] = "active";
            Budgets = _applicationFinancialTransaction.GetBudgets();
            TempData["NumberItems"] = Budgets.Count();
        }
    }
}
