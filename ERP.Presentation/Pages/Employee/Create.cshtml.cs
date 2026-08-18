using ERP.Application.Contract.EmployeeAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Employee
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IApplicationEmployee _applicationEmployee;
        private readonly IResultMessage _resultMessage;
        public CreateModel(IApplicationEmployee applicationEmployee, IResultMessage resultMessage)
        {
            _applicationEmployee = applicationEmployee;
            _resultMessage = resultMessage;
        }

        [BindProperty]
        public CreateEmployeeDto Command { get; set; }

        public void OnGet()
        {
            ViewData["PageTitle"] = "مدیریت کارمندان";
            ViewData["EmployeeActive"] = "active";
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("خطا در ثبت کارمند");
                return Page();
            }
            _applicationEmployee.Create(Command);
            TempData["Message"] = _resultMessage.Success("این کارمند با موفقیت ثبت شد");
            return RedirectToPage("Index");
        }
    }
}
