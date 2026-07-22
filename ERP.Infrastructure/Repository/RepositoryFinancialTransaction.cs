using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repository
{
    public class RepositoryFinancialTransaction : IRepositoryFinancialTransaction
    {
        private readonly ERPContext _context;
        public RepositoryFinancialTransaction(ERPContext context)
        {
            _context = context;
        }

        public void Create(FinancialTransactionModel financialTransaction)
        {
            _context.FinancialTransactions.Add(financialTransaction);
        }

        public FinancialTransactionModel GetBy(long id)
        {
            return _context.FinancialTransactions.FirstOrDefault(x => x.Id == id);
        }

        public List<FinancialTransactionModel> GetAll()
        {
            return _context.FinancialTransactions.AsNoTracking().ToList();
        }

        public bool IsExist(long id)
        {
            return _context.FinancialTransactions.Any(x => x.Id == id);
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
