using ERP.Domain.Interface.Repository;
using ERP.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Presentation.ViewComponents
{
    public class HeaderViewComponent: ViewComponent
    {
        private readonly IRepositoryBudget _repositoryBudget;
        public HeaderViewComponent(IRepositoryBudget repositoryBudget)
        {
            _repositoryBudget = repositoryBudget;
        }

        public bool InitialCapital { get; set; }

        public IViewComponentResult Invoke()
        {
            InitialCapital = _repositoryBudget.HasInitialCapital();
            return View("Header", InitialCapital);
        }
    }
}
