using ERP.Application.Contract.CustomerAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Customer
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IApplicationCustomer _applicationCustomer;
        private readonly IResultMessage _resultMessage;
        public EditModel(IApplicationCustomer applicationCustomer, IResultMessage resultMessage)
        {
            _applicationCustomer = applicationCustomer;
            _resultMessage = resultMessage;
        }

        [BindProperty]
        public EditCustomerDto Command { get; set; }
        public void OnGet(int id)
        {
            ViewData["PageTitle"] = "مدیریت مشتریان";
            ViewData["CustomerActive"] = "active";
            Command = _applicationCustomer.GetForEdit(id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("خطا در ثبت مشتری");
                return Page();
            }
            _applicationCustomer.Edit(Command);
            TempData["Message"] = _resultMessage.Success("این مشتری با موفقیت ویرایش شد");
            return RedirectToPage("Index");
        }
    }
}
