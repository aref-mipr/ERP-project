using ERP.Domain.Entity;

namespace ERP.Domain.Interface.Repository
{
    public interface IRepositoryBudget
    {
        void Create(BudgetModel budget);
        BudgetModel GetBy(long id);
        BudgetModel GetLast();
        List<BudgetModel> GetAll();
        bool IsExist(long id);
        bool HasInitialCapital();
        void SaveChange();
    }
}
