namespace ERP.Application.Contract.ProductAgg
{
    public class ProductViewModel
    {
        public int Id { get; set; } 
        public string CreationTime { get; set; }
        public string ProductCategory { get; set; }
        public int ProductCode { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}
