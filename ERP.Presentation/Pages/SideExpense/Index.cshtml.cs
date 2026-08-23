using ERP.Application.Contract.FilterAgg;
using ERP.Application.Contract.SideExpenseAgg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.SideExpense
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IApplicationSideExpense _applicationSideExpense;
        public IndexModel(IApplicationSideExpense applicationSideExpense)
        {
            _applicationSideExpense = applicationSideExpense;
        }

        public List<SideExpenseViewModel> SideExpenses { get; set; }
        public FilterParamsDto FilterParams { get; set; }
        public SearchViewModel Search { get; set; }

        public void OnGet(int pageId = 1, string? search = "")
        {
            ViewData["PageTitle"] = "مدیریت هزینه های جانبی";
            ViewData["SideExpenseActive"] = "active";
            const int take = 15;

            var count = _applicationSideExpense.GetCount(search);

            var pageCount = (int)Math.Ceiling((double)count / take);

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
            SideExpenses = _applicationSideExpense.GetAll(FilterParams);
            TempData["NumberItems"] = _applicationSideExpense.GetCount();
        }
    }
}
