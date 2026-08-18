using ERP.Application.Contract.UserAgg;
using ERP.Domain.Interface.Repository;
using ERP.Infrastructure.Repository;
using ERP.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP.Presentation.ViewComponents
{
    public class HeaderViewComponent: ViewComponent
    {
        private readonly IRepositoryBudget _repositoryBudget;
        private readonly IApplicationUser _applicationUser;
        public HeaderViewComponent(IRepositoryBudget repositoryBudget, IApplicationUser applicationUser)
        {
            _repositoryBudget = repositoryBudget;
            _applicationUser = applicationUser;
        }

        public IViewComponentResult Invoke()
        {
            int userId = 0;
            if(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
                userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new HeaderViewModel
            {
                InitialCapital = _repositoryBudget.HasInitialCapital(),
                UserLogin = userId != 0 ? _applicationUser.GetBy(userId) : null
            };
            return View("Header", model);
        }
    }
}
