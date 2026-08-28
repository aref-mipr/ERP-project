using ERP.Application.Contract.UserAgg;
using ERP.Domain.Interface.Utility;
using ERP.Presentation.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.User
{
    public class LoginModel : PageModel
    {
        private readonly IApplicationUser _applicationUser;
        private readonly IResultMessage _resultMessage;
        private readonly IAuthenticationService _authenticationService;
        public LoginModel(IApplicationUser applicationUser, IResultMessage resultMessage,
            IAuthenticationService authenticationService)
        {
            _applicationUser = applicationUser;
            _resultMessage = resultMessage;
            _authenticationService = authenticationService;
        }

        [BindProperty]
        public LoginUserViewModel Command { get; set; }

        public IActionResult OnGet()
        {
            if (!_applicationUser.HasActiveUser())
                return RedirectToPage("License");

            return Page();
        }

        public IActionResult OnPost()
        {
            var user = _applicationUser.Login(Command);
            if (user == null)
            {
                TempData["ErrorMessage"] = _resultMessage.NotFound("لطفا از صحت اطلاعات وارد شده اطمینان حاصل فرمایید");
                return Page();
            }
            else
            {
                ModelState.Remove("Command.Id");
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = _resultMessage.Error("ورود کاربر با خطا مواجه شد");
                    return Page();
                }

                _authenticationService.SignIn(user.Id.ToString(), user.FullName);
                TempData["Wellcome"] = _resultMessage.Success($"خوش آمدید {user.FullName}");
                return RedirectToPage("/Dashboard/Index");
            }
        }
    }
}
