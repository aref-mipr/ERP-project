namespace ERP.Domain.Criteria
{
    public class OrderItemCriteria
    {
        public int OrderId { get; set; }
        public long ProductItemId { get; set; }
        public decimal Price { get; set; }
    }
}
