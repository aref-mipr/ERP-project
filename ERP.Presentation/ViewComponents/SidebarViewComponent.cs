using ERP.Domain.Interface.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Presentation.ViewComponents
{
    public class SidebarViewComponent: ViewComponent
    {
        private readonly IRepositoryBudget _repositoryBudget;
        public SidebarViewComponent(IRepositoryBudget repositoryBudget)
        {
            _repositoryBudget = repositoryBudget;
        }

        public bool InitialCapital { get; set; }

        public IViewComponentResult Invoke()
        {
            InitialCapital = _repositoryBudget.HasInitialCapital();
            return View("Sidebar", InitialCapital);
        }
    }
}
