using static ERP.Domain.Entity.ProductItemModel;

namespace ERP.Domain.Criteria
{
    public class ProductItemCriteria
    {
        public int ProductId { get; set; } 
        public long ProductItemCode { get; set; } 
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public ProductItemStatuses ProductItemStatus { get; set; }

    }
}
