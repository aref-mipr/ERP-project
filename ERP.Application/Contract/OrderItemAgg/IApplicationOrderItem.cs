namespace ERP.Application.Contract.OrderItemAgg
{
    public interface IApplicationOrderItem
    {
        List<OrderItemViewModel> GetAllBy(int orderId);
        void Return(long id);
    }
}
