
namespace ERP.Application.Contract.ProductItemAgg
{
    public class ProductItemViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string CreationTime { get; set; }
        public string ProductItemStatus { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public long ProductItemCode { get; set; }
        public string? Description { get; set; }

    }
}