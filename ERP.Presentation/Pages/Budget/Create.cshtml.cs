using ERP.Application.Contract.BudgetAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ERP.Presentation.Pages.Budget
{
    [Authorize]
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
        [Required(ErrorMessage = "مقدار سرمایه را وارد کنید")]
        [Range(1, int.MaxValue, ErrorMessage = "مبلغ سرمایه باید بیشتر از 0 باشد")]
        public decimal Amount { get; set; }

        public void OnGet()
        {
            ViewData["PageTitle"] = "مدیریت سرمایه";
            ViewData["BudgetActive"] = "active";
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
