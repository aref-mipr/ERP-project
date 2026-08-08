using ERP.Domain.Entity;

namespace ERP.Domain.Interface.Repository
{
    public interface IRepositoryEmployee
    {
        void Create(EmployeeModel employee);
        EmployeeModel GetBy(int id);
        List<EmployeeModel> GetAll();
        bool IsExist(int id);
        void SaveChange();
    }
}
