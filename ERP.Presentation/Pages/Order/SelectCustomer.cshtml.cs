using Azure;
using ERP.Application.Contract.CustomerAgg;
using ERP.Application.Contract.FilterAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Order
{
    [Authorize]
    public class SelectCustomerModel : PageModel
    {
        private readonly IApplicationCustomer _applicationCustomer;
        private readonly IResultMessage _resultMessage;

        public SelectCustomerModel(IApplicationCustomer applicationCustomer, IResultMessage resultMessage)
        {
            _applicationCustomer = applicationCustomer;
            _resultMessage = resultMessage;
        }

        public List<CustomerViewModel> Customers { get; set; }

        [BindProperty]
        public int CustomerId { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<long> ProductItemIds { get; set; }

        [BindProperty]
        public int Id { get; set; }
        public FilterParamsDto FilterParams { get; set; }
        public SearchViewModel Search { get; set; }


        public void OnGet(int id, int pageId = 1, string? search = "")
        {
            ViewData["PageTitle"] = "مدیریت سفارش ها";
            ViewData["OrderActive"] = "active";
            TempData["Subject"] = "نام مشتری";
            TempData["AnotherParam"] = true;

            Id = id;

            const int take = 10;
            int count = _applicationCustomer.GetCount(search);
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
                FilterParams = FilterParams,

                AdditionalParameters = new Dictionary<string, string[]>
                {
                    ["ProductItemIds"] = ProductItemIds
                        .Select(x => x.ToString())
                        .ToArray(),
                    ["id"] = new [] {Id.ToString()}
                }
            };
            Customers = _applicationCustomer.GetAll(FilterParams);
        }

        public IActionResult OnPost(int id)
        {
            if(CustomerId == 0)
            {
                TempData["Message"] = _resultMessage.Error("یک مشتری را انتخاب کنید!");
                return RedirectToPage(new {CustomerId, ProductItemIds });
            }
            return RedirectToPage(
                "/Order/Create",
                new {CustomerId, ProductItemIds, id });
        }
    }
}
