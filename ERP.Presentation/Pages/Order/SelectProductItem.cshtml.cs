using Azure;
using ERP.Application.Contract.FilterAgg;
using ERP.Application.Contract.OrderItemAgg;
using ERP.Application.Contract.ProductItemAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Order
{
    [Authorize]
    public class SelectProductItemModel : PageModel
    {
        private readonly IApplicationProductItem _applicationProductItem;
        private readonly IApplicationOrderItem _applicationOrderItem;
        private readonly IResultMessage _resultMessage;

        public SelectProductItemModel(IApplicationProductItem applicationProductItem,
            IApplicationOrderItem applicationOrderItem, IResultMessage resultMessage)
        {
            _applicationProductItem = applicationProductItem;
            _applicationOrderItem = applicationOrderItem;
            _resultMessage = resultMessage;
        }

        public List<ProductItemViewModel> ProductItems { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<long> ProductItemIds { get; set; } = new();

        [BindProperty]
        public int Id { get; set; }
        public FilterParamsDto FilterParams { get; set; }
        public SearchViewModel Search { get; set; }

        public void OnGet(int id, int pageId = 1, string? search = "")
        {
            ViewData["PageTitle"] = "مدیریت سفارش ها";
            ViewData["OrderActive"] = "active";
            Id = id;
            ProductItems = _applicationProductItem.GetAllReadyToSell(id);
        }

        public IActionResult OnPost(int id)
        {
            if (!ProductItemIds.Any())
            {
                TempData["Message"] = _resultMessage.Error("حداقل یک آیتم را انتخاب کنید!");
                return RedirectToPage(new {id});
            }
            return RedirectToPage(
                "/Order/SelectCustomer",
                new
                {ProductItemIds, id});
        }
    }
}
