using Azure;
using ERP.Application.Contract.FilterAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.FinancialTransaction
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IApplicationFinancialTransaction _applicationFinancialTransaction;
        private readonly IEnumExtension _enumExtension;
        public IndexModel(IApplicationFinancialTransaction applicationFinancialTransaction, IEnumExtension enumExtension)
        {
            _applicationFinancialTransaction = applicationFinancialTransaction;
            _enumExtension = enumExtension;
        }

        public List<FinancialTransactionViewModel> FinancialTransactions { get; set; }
        public FilterParamsDto FilterParams { get; set; }
        public SearchViewModel Search { get; set; }

        public void OnGet(int pageId = 1, string? search = "")
        {
            ViewData["PageTitle"] = "مدیریت تراکنش ها";
            ViewData["TransactionActive"] = "active";

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
            FinancialTransactions = _applicationFinancialTransaction.GetAll(FilterParams);
            TempData["NumberItems"] = _applicationFinancialTransaction.GetCount();
        }
    }
}
