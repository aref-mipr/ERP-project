using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repository
{
    public class RepositorySideExpense : IRepositorySideExpense
    {
        private readonly ERPContext _context;
        public RepositorySideExpense(ERPContext context)
        {
            _context = context;
        }

        public void Create(SideExpenseModel sideExpense)
        {
            _context.SideExpenses.Add(sideExpense);
        }
        public SideExpenseModel GetBy(int id)
        {
            return _context.SideExpenses.FirstOrDefault(x => x.Id == id);
        }

        public List<SideExpenseModel> GetAll()
        {
            return _context.SideExpenses.AsNoTracking().ToList();
        }


        public bool IsExist(int id)
        {
            return _context.SideExpenses.Any(x => x.Id == id);
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
