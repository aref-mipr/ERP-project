using ERP.Domain.Criteria;

namespace ERP.Application.Contract.OrderItemAgg
{
    public class CreateOrderItemDto
    {
        public int Id { get; set; }
        public OrderItemCriteria OrderItemsCriteria { get; set; }
    }
}
