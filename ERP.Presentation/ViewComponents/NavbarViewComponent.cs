using ERP.Application.Contract.BudgetAgg;
using ERP.Domain.Interface.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Presentation.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly IRepositoryBudget _repositoryBudget;
        public NavbarViewComponent(IRepositoryBudget repositoryBudget)
        {
            _repositoryBudget = repositoryBudget;
        }

        public bool InitialCapital { get; set; }

        public IViewComponentResult Invoke()
        {
            InitialCapital = _repositoryBudget.HasInitialCapital();
            return View("Navbar",InitialCapital);
        }
    }
}
