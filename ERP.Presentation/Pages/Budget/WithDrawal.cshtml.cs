using ERP.Application.Contract.BudgetAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Budget
{
    public class WithDrawalModel : PageModel
    {
        private readonly IApplicationBudget _applicationBudget;
        private readonly IResultMessage _resultMessage;
        private readonly IRepositoryBudget _repositoryBudget;
        public WithDrawalModel(IApplicationBudget applicationBudget, IResultMessage resultMessage, IRepositoryBudget repositoryBudget)
        {
            _applicationBudget = applicationBudget;
            _resultMessage = resultMessage;
            _repositoryBudget = repositoryBudget;
        }

        [BindProperty]
        public decimal Amount { get; set; }

        public void OnGet()
        {
        }

        public RedirectToPageResult OnPost()
        {
            if(Amount > _repositoryBudget.GetLast().TotalBudget)
            {
                TempData["Message"] = _resultMessage.Error("عدم بودجه کافی");
                return RedirectToPage();
            }
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("خطا در برداشت از سرمایه");
                return RedirectToPage();
            }
            _applicationBudget.WithDrawal(Amount);
            return RedirectToPage("/Budget/Index");
        }
    }
}
