using ERP.Application.Contract.UserAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.User
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IApplicationUser _applicationUser;
        private readonly IResultMessage _resultMessage;
        public EditModel(IApplicationUser applicationUser, IResultMessage resultMessage)
        {
            _applicationUser = applicationUser;
            _resultMessage = resultMessage;
        }

        [BindProperty]
        public EditUserDto Command { get; set; }

        [BindProperty]
        public string RepeatPassword { get; set; }

        public void OnGet(int id)
        {
            Command = _applicationUser.GetForEdit(id);
        }

        public IActionResult OnPost()
        {
            var user = _applicationUser.GetBy(Command.UsersCriteria.UserName);
            if (user != null)
            {
                if (user.UsersCriteria.UserName == Command.UsersCriteria.UserName)
                {
                    if (!ModelState.IsValid)
                    {
                        TempData["Message"] = _resultMessage.Error("ویرایش کاربر با خطا مواجه شد");
                        return Page();
                    }
                    else if (Command.Password != RepeatPassword)
                    {
                        TempData["RepeatPassword"] = _resultMessage.Error("از درستی رمز عبور اطمینان یابید");
                        return Page();
                    }
                    else
                    {
                        _applicationUser.Edit(Command);
                        TempData["Message"] = _resultMessage.Success("ویرایش کاربر با موفقیت انجام شد");
                        int id = Command.Id;
                        return RedirectToPage("Details", new { id });
                    }
                }
                else
                {
                    TempData["Message"] = _resultMessage.Error("نام کاربری تکراری است");
                    return Page();
                }
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    TempData["Message"] = _resultMessage.Error("ویرایش کاربر با خطا مواجه شد");
                    return Page();
                }
                else if (Command.Password != RepeatPassword)
                {
                    TempData["RepeatPassword"] = _resultMessage.Error("از درستی رمز عبور اطمینان یابید");
                    return Page();
                }
                else
                {
                    _applicationUser.Edit(Command);
                    TempData["Message"] = _resultMessage.Success("ویرایش کاربر با موفقیت انجام شد");
                    int id = Command.Id;
                    return RedirectToPage("Details", new { id });
                }
            }
            
        }
    }
}
