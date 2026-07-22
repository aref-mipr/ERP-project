using ERP.Domain.Entity;

namespace ERP.Domain.Interface.Repository
{
    public interface IRepositoryOrderItem
    {
        void Create(OrderItemModel order);
        OrderItemModel GetBy(long id);
        List<OrderItemModel> GetAll();
        List<OrderItemModel> GetAllBy(int orderId);
        bool IsExist(long id);
        void SaveChange();
    }
}
