using ERP.Application.Contract.FinancialTransactionAgg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.FinancialTransaction
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IApplicationFinancialTransaction _applicationFinancialTransaction;
        public DetailsModel(IApplicationFinancialTransaction applicationFinancialTransaction)
        {
            _applicationFinancialTransaction = applicationFinancialTransaction;
        }

        public FinancialTransactionViewModel Transaction { get; set; }

        public void OnGet(long id)
        {
            ViewData["PageTitle"] = "مدیریت تراکنش ها";
            ViewData["TransactionActive"] = "active";
            Transaction = _applicationFinancialTransaction.GetBy(id);
        }
    }
}
