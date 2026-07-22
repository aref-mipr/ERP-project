using ERP.Domain.Interface.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Presentation.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IRepositoryBudget _repositoryBudget;
        public IndexModel(IRepositoryBudget repositoryBudget)
        {
            _repositoryBudget = repositoryBudget;
        }

        public bool InitialCapital { get; set; }
        public void OnGet()
        {
            InitialCapital = _repositoryBudget.HasInitialCapital();
        }
    }
}
