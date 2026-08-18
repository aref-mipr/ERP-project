using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ERP.Presentation.Pages.User
{
    public class LicenseModel : PageModel
    {
        private readonly IResultMessage _resultMessage;
        public LicenseModel(IResultMessage resultMessage)
        {
            _resultMessage = resultMessage;
        }

        [BindProperty]
        [Required(ErrorMessage = "کد لایسنس را وارد کنید")]
        public string LicenseCode { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var x = LicenseCode;
            if(LicenseCode == "AB-12345678")
            {
                TempData["Message"] = _resultMessage.Success("کد فعالسازی با موفقیت تایید شد");
                return RedirectToPage("Register");
            }
            else
            {
                TempData["Message"] = _resultMessage.Error("از درستی کد فعالسازی اطمینان حاصل فرمایید");
                return Page();
            }
        }
    }
}
