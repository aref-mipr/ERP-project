using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.FilterAgg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Budget
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IApplicationBudget _applicationBudget;
        public IndexModel(IApplicationBudget applicationBudget)
        {
            _applicationBudget = applicationBudget;
        }

        public List<BudgetViewModel> Budgets { get; set; }
        public FilterParamsDto FilterParams { get; set; }
        public SearchViewModel Search { get; set; }

        public void OnGet(int pageId = 1)
        {
            ViewData["PageTitle"] = "مدیریت سرمایه";
            ViewData["BudgetActive"] = "active";

            const int take = 10;
            int count = _applicationBudget.GetCount();
            int pageCount = (int)Math.Ceiling((double)count / take);

            if (pageCount < 1)
                pageCount = 1;

            if (pageId < 1)
                pageId = 1;

            if(pageId > pageCount)
                pageId = pageCount;

            var filterParamsCriteria = new FilterParamsCriteria
            {
                PageCount = pageCount,
                PageId = pageId,
                Take = take,
            };

            FilterParams = new FilterParamsDto(filterParamsCriteria);
            Search = new SearchViewModel
            {
                FilterParams = FilterParams
            };

            Budgets = _applicationBudget.GetAll(FilterParams);
            TempData["NumberItems"] = _applicationBudget.GetCount();
        }
    }
}
