using ERP.Domain.Entity;

namespace ERP.Domain.Interface.Repository
{
    public interface IRepositorySideExpense
    {
        void Create(SideExpenseModel sideExpense);
        SideExpenseModel GetBy(int id);
        List<SideExpenseModel> GetAll();
        bool IsExist(int id);
        void SaveChange();
    }
}
