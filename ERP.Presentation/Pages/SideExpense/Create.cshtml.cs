using ERP.Application.Contract.SideExpenseAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.SideExpense
{
    public class CreateModel : PageModel
    {
        private readonly IApplicationSideExpense _applicationSideExpense;
        private readonly IRepositorySideExpense _repositorySideExpense;
        private readonly IResultMessage _resultMessage;
        private readonly IRepositoryBudget _repositoryBudget;
        public CreateModel(IApplicationSideExpense applicationSideExpense, IRepositorySideExpense repositorySideExpense,
            IResultMessage resultMessage, IRepositoryBudget repositoryBudget)
        {
            _applicationSideExpense = applicationSideExpense;
            _repositorySideExpense = repositorySideExpense;
            _resultMessage = resultMessage;
            _repositoryBudget = repositoryBudget;
        }

        [BindProperty]
        public CreateSideExpenseDto Command { get; set; }
        public void OnGet()
        {
        }

        public RedirectToPageResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("خطا در ثبت هزینه جانبی");
                return RedirectToPage();
            }

            if (Command.SideExpensesCriteria.Amount > _repositoryBudget.GetLast().TotalBudget)
            {
                TempData["Message"] = _resultMessage.Error("عدم بودجه کافی");
                return RedirectToPage();
            }

            _applicationSideExpense.Create(Command);
            TempData["Message"] = _resultMessage.Success("هزینه جانبی با موفقیت ثبت شد");
            return RedirectToPage("Index");
        }
    }
}
