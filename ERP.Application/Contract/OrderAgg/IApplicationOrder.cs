using static ERP.Domain.Entity.OrderModel;

namespace ERP.Application.Contract.OrderAgg
{
    public interface IApplicationOrder
    {
        void Create(CreateOrderDto command);
        OrderViewModel GetBy(int id);
        List<OrderViewModel> GetAll();
        List<OrderStatusViewModel> CreateStatuses();
        void ChangeStatus(int id, OrderStatuses status);
    }
}
