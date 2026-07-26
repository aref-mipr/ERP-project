using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ERP.Presentation.Pages.FinancialTransaction
{
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
            Id = id;
            if(_repositorynFinancialTransaction.GetBy(id).Description != null)
                Description = _applicationFinancialTransaction.GetBy(id).Description;
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
