using ERP.Domain.Entity;

namespace ERP.Domain.Interface.Repository
{
    public interface IRepositoryFinancialTransaction
    {
        void Create(FinancialTransactionModel financialTransaction);
        FinancialTransactionModel GetBy(long id);
        List<FinancialTransactionModel> GetAll();
        bool IsExist(long id);
        void SaveChange();
    }
}
