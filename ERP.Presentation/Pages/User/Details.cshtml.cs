using ERP.Application.Contract.UserAgg;
using ERP.Presentation.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.User
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IApplicationUser _applicationUser;
        private readonly IAuthenticationService _authenticationService;
        public DetailsModel(IApplicationUser applicationUser, IAuthenticationService authenticationService)
        {
            _applicationUser = applicationUser;
            _authenticationService = authenticationService;
        }

        public UserViewModel User { get; set; }

        public void OnGet(int id)
        {
            User = _applicationUser.GetBy(id);
        }

        public async Task<IActionResult> OnPost()
        {
            await _authenticationService.SignOut();
            return RedirectToPage("Login");
        }
    }
}
