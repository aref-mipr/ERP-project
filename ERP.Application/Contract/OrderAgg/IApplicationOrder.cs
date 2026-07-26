using static ERP.Domain.Entity.OrderModel;

namespace ERP.Application.Contract.OrderAgg
{
    public interface IApplicationOrder
    {
        void Create(CreateOrderDto command);
        //void Edit(EditOrderDto command);
        void Edit(CreateOrderDto command);
        OrderViewModel GetBy(int id);
        List<OrderViewModel> GetAll();
        List<OrderViewModel> GetAllBy(int customerId);
        List<OrderStatusViewModel> CreateStatuses();
        void ChangeStatus(int id, OrderStatuses status);
    }
}
