using ERP.Application.Contract.CustomerAgg;
using ERP.Application.Contract.ProductCategoryAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Customer
{
    public class CreateModel : PageModel
    {
        private readonly IApplicationCustomer _applicationCustomer;
        private readonly IResultMessage _resultMessage;
        public CreateModel(IApplicationCustomer applicationCustomer, IResultMessage resultMessage)
        {
            _applicationCustomer = applicationCustomer;
            _resultMessage = resultMessage;
        }

        [BindProperty]
        public CreateCustomerDto Command { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                 TempData["Message"] = _resultMessage.Error("خطا در ثبت مشتری");
                return Page();
            }
            _applicationCustomer.Create(Command);
            TempData["Message"] = _resultMessage.Success("این مشتری با موفقیت افزوده شد");
            return RedirectToPage("Index");
        }
    }
}
