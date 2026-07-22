using ERP.Domain.Criteria;

namespace ERP.Application.Contract.OrderAgg
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal InitialAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string CreationTime { get; set; }
        public string CustomerFullName { get; set; }
        public int CustomerCode { get; set; }
        public string OrderStatus { get; set; }
        public OrderCriteria OrdersCriteria { get; set; }
    }
}
