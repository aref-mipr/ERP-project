using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.FinancialTransaction
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IApplicationFinancialTransaction _applicationfinancialTransaction;
        private readonly IEnumExtension _enumExtension;
        public IndexModel(IApplicationFinancialTransaction applicationfinancialTransaction, IEnumExtension enumExtension)
        {
            _applicationfinancialTransaction = applicationfinancialTransaction;
            _enumExtension = enumExtension;
        }

        public List<FinancialTransactionViewModel> FinancialTransactions { get; set; }

        public void OnGet()
        {
            ViewData["PageTitle"] = "مدیریت تراکنش ها";
            ViewData["TransactionActive"] = "active";
            FinancialTransactions = _applicationfinancialTransaction.GetAll();
            TempData["NumberItems"] = FinancialTransactions.Count();
        }
    }
}
