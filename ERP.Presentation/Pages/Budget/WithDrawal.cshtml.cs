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
        [Required(ErrorMessage = "مقدار برداشت از سرمایه را وارد کنید")]
        [Range(1, int.MaxValue, ErrorMessage = "مبلغ برداشت از سرمایه باید بیشتر از 0 باشد")]
        public decimal Amount { get; set; }

        public void OnGet()
        {
            ViewData["PageTitle"] = "مدیریت سرمایه";
            ViewData["BudgetActive"] = "active";
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
            TempData["Message"] = _resultMessage.Success("برداشت از سرمایه با موفقیت انجام شد");
            return RedirectToPage("/Budget/Index");
        }
    }
}
