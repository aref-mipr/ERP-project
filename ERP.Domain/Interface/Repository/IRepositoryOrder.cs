using ERP.Domain.Entity;

namespace ERP.Domain.Interface.Repository
{
    public interface IRepositoryOrder
    {
        void Create(OrderModel order);
        OrderModel GetBy(int id);
        List<OrderModel> GetAll();
        bool IsExist(int id);
        void SaveChange();
    }
}
