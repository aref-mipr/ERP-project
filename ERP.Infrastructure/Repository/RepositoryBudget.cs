using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repository
{
    public class RepositoryBudget : IRepositoryBudget
    {
        private readonly ERPContext _context;
        public RepositoryBudget(ERPContext context)
        {
            _context = context;
        }

        public void Create(BudgetModel budget)
        {
            _context.Budgets.Add(budget);
        }
        public BudgetModel GetBy(long id)
        {
            return _context.Budgets.FirstOrDefault(x => x.Id == id);
        }

        public BudgetModel GetLast()
        {
            return _context.Budgets.OrderByDescending(x => x.Id).FirstOrDefault();
        }

        public List<BudgetModel> GetAll()
        {
            return _context.Budgets.AsNoTracking().ToList();
        }

        public bool IsExist(long id)
        {
            return _context.Budgets.Any(x => x.Id == id);
        }

        public bool HasInitialCapital()
        {
            return _context.Budgets.Any();
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
