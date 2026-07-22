using static ERP.Domain.Entity.OrderModel;

namespace ERP.Domain.Criteria
{
    public class OrderCriteria
    {
        public int CustomerId { get; set; }
        public int OrderCode { get; set; }
        public string? Description { get; set; }
        public decimal InitialAmount { get; set; } 
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public OrderStatuses OrderStatus { get; set; }
    }
}
