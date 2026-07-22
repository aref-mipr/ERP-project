using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Application.Service;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static ERP.Domain.Entity.FinancialTransactionModel;

namespace ERP.Presentation.Pages.FinancialTransaction
{
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
            FinancialTransactions = _applicationfinancialTransaction.GetAll();
            TempData["NumberItems"] = FinancialTransactions.Count();
        }
    }
}
