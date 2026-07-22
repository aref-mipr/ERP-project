using ERP.Application.Contract.ProductCategoryAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.ProductCategory
{
    public class CreateModel : PageModel
    {
        private readonly IApplicationProductCategory _applicationProductCategory;
        private readonly IResultMessage _resultMessage;
        public CreateModel(IApplicationProductCategory applicationProductCategory, IResultMessage resultMessage)
        {
            _applicationProductCategory = applicationProductCategory;
            _resultMessage = resultMessage;
        }

        [BindProperty]
        public CreateProductCategoryDto Command { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = _resultMessage.Error("خطا در ثبت دسته بندی");
                return Page();
            }
            
            _applicationProductCategory.Create(Command);
            TempData["Message"] = _resultMessage.Success("این دسته بندی با موفقیت افزوده شد");

            return RedirectToPage("Index");
        }
    }
}
