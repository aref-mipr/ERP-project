using ERP.Application.Contract.UserAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.User
{
    public class RegisterModel : PageModel
    {
        private readonly IApplicationUser _applicationUser;
        private readonly IResultMessage _resultMessage;
        public RegisterModel(IApplicationUser applicationUser, IResultMessage resultMessage)
        {
            _applicationUser = applicationUser;
            _resultMessage = resultMessage;
        }

        [BindProperty]
        public RegisterUserDto Command { get; set; }

        [BindProperty]
        public string RepeatPassword { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var user = _applicationUser.GetBy(Command.UsersCriteria.UserName);
            if(user != null)
            {
                TempData["ErrorMessage"] = _resultMessage.Error("نام کاربری تکراری است");
                return Page();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = _resultMessage.Error("ثبت کاربر با خطا مواجه شد");
                    return Page();
                }else if (Command.Password != RepeatPassword)
                {
                    TempData["RepeatPassword"] = _resultMessage.Error("از درستی رمز عبور اطمینان یابید");
                    return Page();
                }
                else
                {
                    _applicationUser.Register(Command);
                    TempData["Message"] = _resultMessage.Success("ثبت نام شما با موفقیت انجام شد. برای ورود، نام کاربری و رمز عبور خود را وارد کنید");
                    return RedirectToPage("Login");
                }
            }
        }
    }
}
