using ERP.Application.Contract.CustomerAgg;
using ERP.Application.Contract.FilterAgg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Customer
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IApplicationCustomer _applicationCustomer;
        public IndexModel(IApplicationCustomer applicationCustomer)
        {
            _applicationCustomer = applicationCustomer;
        }

        public List<CustomerViewModel> Customers { get; set; }
        public FilterParamsDto FilterParams { get; set; }
        public SearchViewModel Search { get; set; }

        public void OnGet(int pageId = 1, string? search = "")
        {
            ViewData["PageTitle"] = "مدیریت مشتریان";
            ViewData["CustomerActive"] = "active";
            TempData["Subject"] = "نام مشتری";

            const int take = 15;
            int count = _applicationCustomer.GetCount(search);
            int pageCount = (int)Math.Ceiling((double)count / take);

            if (pageCount < 1)
                pageCount = 1;

            if (pageId < 1)
                pageId = 1;

            if(pageId > pageCount)
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
            Customers = _applicationCustomer.GetAll(FilterParams);

            TempData["NumberItems"] = Customers.Count();
        }
    }
}
