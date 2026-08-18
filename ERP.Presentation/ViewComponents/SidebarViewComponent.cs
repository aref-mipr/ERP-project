using ERP.Application.Contract.UserAgg;
using ERP.Domain.Interface.Repository;
using ERP.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP.Presentation.ViewComponents
{
    public class SidebarViewComponent: ViewComponent
    {
        private readonly IRepositoryBudget _repositoryBudget;
        private readonly IApplicationUser _applicationUser;
        public SidebarViewComponent(IRepositoryBudget repositoryBudget, IApplicationUser applicationUser)
        {
            _repositoryBudget = repositoryBudget;
            _applicationUser = applicationUser;
        }

        public bool InitialCapital { get; set; }

        public IViewComponentResult Invoke()
        {
            int userId = 0;
            if (HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
                userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new SidebarViewMdoel
            {
                InitialCapital = _repositoryBudget.HasInitialCapital(),
                UserLogin = userId != 0 ? _applicationUser.GetBy(userId) : null,
            };
            return View("Sidebar", model);
        }
    }
}
