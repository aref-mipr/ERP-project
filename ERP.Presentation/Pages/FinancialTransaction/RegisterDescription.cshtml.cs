using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ERP.Presentation.Pages.FinancialTransaction
{
    [Authorize]
    public class RegisterDescriptionModel : PageModel
    {
        private readonly IApplicationFinancialTransaction _applicationFinancialTransaction;
        private readonly IRepositoryFinancialTransaction _repositorynFinancialTransaction;
        private readonly IResultMessage _resultMessage;
        public RegisterDescriptionModel(IApplicationFinancialTransaction applicationFinancialTransaction,
            IRepositoryFinancialTransaction repositoryFinancialTransaction ,IResultMessage resultMessage)
        {
            _applicationFinancialTransaction = applicationFinancialTransaction;
            _repositorynFinancialTransaction = repositoryFinancialTransaction;
            _resultMessage = resultMessage;
        }

        [BindProperty]
        [MinLength(1, ErrorMessage = "توضیحاتی وارد کنید یا از این صفحه خارج شوید")]
        public string Description { get; set; }

        [BindProperty]
        public long Id { get; set; }
        public FinancialTransactionViewModel Transaction { get; set; }

        public void OnGet(long id)
        {
            ViewData["PageTitle"] = "مدیریت تراکنش ها";
            ViewData["TransactionActive"] = "active";
            Id = id;
            if(_applicationFinancialTransaction.GetDescritpion(id) != null)
                Description = _applicationFinancialTransaction.GetDescritpion(id);
        }

        public IActionResult OnPost(long id)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("ثبت توضیحات با خطا مواجه شد");
                return Page();
            }
            _applicationFinancialTransaction.RegisterDescription(id, Description);
            Transaction = _applicationFinancialTransaction.GetBy(id);
            TempData["Message"] = _resultMessage.Success("توضیحات با موفقیت ثبت شد");
            return RedirectToPage("Details", Transaction);
        }
    }
}
