using ERP.Application.Contract.UserAgg;
using ERP.Domain.Interface.Repository;
using ERP.Infrastructure.Repository;
using ERP.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

            var persianDate = new PersianCalendar();

            var model = new HeaderViewModel
            {
                InitialCapital = _repositoryBudget.HasInitialCapital(),
                DateNow = $"{DateTime.Now:HH:mm} , " +
                        $"{persianDate.GetYear(DateTime.Now):0000}/" +
                        $"{persianDate.GetMonth(DateTime.Now):00}/" +
                        $"{persianDate.GetDayOfMonth(DateTime.Now):00}",
                UserLogin = userId != 0 ? _applicationUser.GetBy(userId) : null
            };
            return View("Header", model);
        }
    }
}
