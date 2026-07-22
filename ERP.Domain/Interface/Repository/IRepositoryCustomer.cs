using ERP.Domain.Entity;

namespace ERP.Domain.Interface.Repository
{
    public interface IRepositoryCustomer
    {
        void Create(CustomerModel customer);
        CustomerModel GetBy(int id);
        List<CustomerModel> GetAll();
        bool IsExist(int id);
        void SaveChange();
    }
}
