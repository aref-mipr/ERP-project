using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using ERP.Infrastructure.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Budget
{
    public class CreateModel : PageModel
    {
        private readonly IApplicationBudget _applicationBudget;
        private readonly IRepositoryBudget _repositoryBudget;
        private readonly IResultMessage _resultMessage;
        public CreateModel(IApplicationBudget applicationBudget, IRepositoryBudget repositoryBudget, IResultMessage resultMessage)
        {
            _applicationBudget = applicationBudget;
            _repositoryBudget = repositoryBudget;
            _resultMessage = resultMessage;
        }

        [BindProperty]
        public decimal Amount { get; set; }

        public void OnGet()
        {
            if (_repositoryBudget.HasInitialCapital())
                TempData["RegisterBudgetText"] = "ثبت سرمایه جدید";
            else
                TempData["RegisterBudgetText"] = "ثبت سرمایه اولیه";
        }

        public RedirectToPageResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("خطا در ثبت سرمایه");
                return RedirectToPage();
            }
            _applicationBudget.Create(Amount);
            TempData["Message"] = _resultMessage.Success("سرمایه با موفقیت ثبت شد");
            return RedirectToPage("/FinancialTransaction/Budgets");
        }
    }
}
