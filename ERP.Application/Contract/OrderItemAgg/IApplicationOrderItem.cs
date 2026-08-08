namespace ERP.Application.Contract.OrderItemAgg
{
    public interface IApplicationOrderItem
    {
        List<OrderItemViewModel> GetAllWaitingOrderBy(int orderId);
        List<OrderItemViewModel> GetAllBy(int orderId);
        void Return(long id);
    }
}
