using Azure;
using ERP.Application.Contract.FilterAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.FinancialTransaction
{
    [Authorize]
    public class BudgetsModel : PageModel
    {
        private readonly IApplicationFinancialTransaction _applicationFinancialTransaction;
        public BudgetsModel(IApplicationFinancialTransaction applicationFinancialTransaction)
        {
            _applicationFinancialTransaction = applicationFinancialTransaction;
        }

        public List<FinancialTransactionViewModel> Budgets { get; set; }
        public FilterParamsDto FilterParams { get; set; }
        public SearchViewModel Search { get; set; }

        public void OnGet(int pageId = 1, string? search = "")
        {
            ViewData["PageTitle"] = "مدیریت سرمایه";
            ViewData["BudgetActive"] = "active";

            const int take = 15;
            int count = _applicationFinancialTransaction.GetCount(search);
            int pageCount = (int)Math.Ceiling((double)count / take);

            if (pageCount < 1)
                pageCount = 1;

            if (pageId < 1)
                pageId = 1;

            if (pageId > pageCount)
                pageId = pageCount;

            var filterParamsCriteria = new FilterParamsCriteria
            {
                Take = take,
                PageCount = pageCount,
                PageId = pageId,
                Subject = search
            };

            FilterParams = new FilterParamsDto(filterParamsCriteria);
            Search = new SearchViewModel
            {
                FilterParams = FilterParams
            };

            Budgets = _applicationFinancialTransaction.GetBudgets(FilterParams);
            TempData["NumberItems"] = Budgets.Count();
        }
    }
}
