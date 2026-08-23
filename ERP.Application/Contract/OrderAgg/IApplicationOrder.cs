using ERP.Application.Contract.FilterAgg;
using static ERP.Domain.Entity.OrderModel;

namespace ERP.Application.Contract.OrderAgg
{
    public interface IApplicationOrder
    {
        void Create(CreateOrderDto command);
        void Edit(CreateOrderDto command);
        OrderViewModel GetBy(int id);
        List<OrderViewModel> GetAll();
        List<OrderViewModel> GetAll(FilterParamsDto filterParams);
        List<OrderViewModel> GetAllBy(int customerId);
        List<OrderViewModel> GetAllApproved();
        int GetCount(string? subject = null);
        List<OrderStatusViewModel> CreateStatuses();
        void ChangeStatus(int id, OrderStatuses status);
    }
}
