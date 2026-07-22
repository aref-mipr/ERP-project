namespace ERP.Domain.Criteria
{
    public class ProductCriteria
    {
        public int ProductCategoryId { get; set; } 
        public int ProductCode { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal SellPrice { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; }

    }
}
