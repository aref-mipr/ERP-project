using ERP.Domain.Entity;

namespace ERP.Domain.Interface.Repository
{
    public interface IRepositoryOrderItem
    {
        void Create(OrderItemModel order);
        void Remove(OrderItemModel orderItem);
        OrderItemModel GetBy(long id);
        List<OrderItemModel> GetAll();
        List<OrderItemModel> GetAllBy(int orderId);
        List<OrderItemModel> GetAllWaitingOrderBy(int orderId);
        bool IsExist(long id);
        void SaveChange();
    }
}
