using ERP.Application.Contract.EmployeeAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Employee
{
    public class EditModel : PageModel
    {
        private readonly IApplicationEmployee _applicationEmployee;
        private readonly IRepositoryEmployee _repositoryEmployee;
        private readonly IResultMessage _resultMessage;
        public EditModel(IApplicationEmployee applicationEmployee, IRepositoryEmployee repositoryEmployee, IResultMessage resultMessage)
        {
            _applicationEmployee = applicationEmployee;
            _repositoryEmployee = repositoryEmployee;
            _resultMessage = resultMessage;
        }

        [BindProperty]
        public EditEmployeeDto Command { get; set; }
        public void OnGet(int id)
        {
            Command = _applicationEmployee.GetForEdit(id);
        }

        public IActionResult OnPost()
        {
            var employee = _repositoryEmployee.GetBy(Command.Id);
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("خطا در ویرایش کارمند");
                return Page();
            }
            _applicationEmployee.Edit(Command);
            TempData["Message"] = _resultMessage.Success("این کارمند با موفقیت ویرایش شد");
            return RedirectToPage("Index");
        }
    }
}