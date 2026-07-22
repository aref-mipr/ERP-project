using ERP.Application.Contract.SideExpenseAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.SideExpense
{
    public class EditModel : PageModel
    {
        private readonly IApplicationSideExpense _applicationSideExpense;
        private readonly IResultMessage _resultMessage;
        private readonly IRepositoryBudget _repositoryBudget;
        public EditModel(IApplicationSideExpense applicationSideExpense, IResultMessage resultMessage,
            IRepositoryBudget repositoryBudget)
        {
            _applicationSideExpense = applicationSideExpense;
            _resultMessage = resultMessage;
            _repositoryBudget = repositoryBudget;
        }

        [BindProperty]
        public EditSideExpenseDto Command { get; set; }
        public void OnGet(int id)
        {
            Command = _applicationSideExpense.GetForEdit(id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("خطا در ویرایش هزینه جانبی");
                return Page();
            }

            if (Command.SideExpensesCriteria.Amount > _repositoryBudget.GetLast().TotalBudget)
            {
                TempData["Message"] = _resultMessage.Error("عدم بودجه کافی");
                return Page();
            }

            _applicationSideExpense.Edit(Command);
            TempData["Message"] = _resultMessage.Success("هزینه جانبی با موفقیت ویرایش شد");
            return RedirectToPage("Index");
        }
    }
}
