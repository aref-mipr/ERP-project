namespace ERP.Application.Contract.OrderItemAgg
{
    public class OrderItemViewModel
    {
        public long Id { get; set; }
        public long ProductItemId { get; set; }
        public long ProductItemCode { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public bool Returned { get; set; }
    }
}
