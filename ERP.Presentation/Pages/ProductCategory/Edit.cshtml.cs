using ERP.Application.Contract.ProductCategoryAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.ProductCategory
{
    public class EditModel : PageModel
    {
        private readonly IApplicationProductCategory _applicationProductCategory;
        private readonly IResultMessage _resultMessage;
        public EditModel(IApplicationProductCategory applicationProductCategory, IResultMessage resultMessage)
        {
            _applicationProductCategory = applicationProductCategory;
            _resultMessage = resultMessage;
        }

        [BindProperty]
        public EditProductCategoryDto Command { get; set; }

        public void OnGet(int id)
        {
            Command = _applicationProductCategory.GetForEdit(id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("خطا در ثبت دسته بندی");
                return Page();
            }

            _applicationProductCategory.Edit(Command);
            TempData["Message"] = _resultMessage.Success("این دسته بندی با موفقیت ویرایش شد");
            return RedirectToPage("Index");
        }
    }
}
