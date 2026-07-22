using ERP.Domain.Criteria;

namespace ERP.Domain.Entity
{
    public class ProductModel
    {
        public int Id { get; private set; }
        public int ProductCategoryId { get; private set; }
        public int ProductCode { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal SellPrice { get; private set; }
        public decimal CostPrice { get; private set; }
        public int StockQuantity { get; private set; }
        public DateTime CreationTime { get; private set; }
        public ProductCategoryModel ProductCateory { get; private set; }
        public ICollection<ProductItemModel> ProductItems { get; private set; }
        public ICollection<FinancialTransactionModel> FinancialTransactions { get; private set; }

        protected ProductModel() { }

        public ProductModel(ProductCriteria productCriteria)
        {
            ProductCategoryId = productCriteria.ProductCategoryId;
            ProductCode = productCriteria.ProductCode;
            Name = productCriteria.Name;
            Description = productCriteria.Description;
            CostPrice = productCriteria.CostPrice;
            SellPrice = productCriteria.SellPrice;
            StockQuantity = productCriteria.StockQuantity;
            CreationTime = DateTime.Now;
            ProductItems = new List<ProductItemModel>();
            FinancialTransactions = new List<FinancialTransactionModel>();
        }

        public void Edit(ProductCriteria productCriteria)
        {
            ProductCategoryId = productCriteria.ProductCategoryId;
            Name = productCriteria.Name;
            Description = productCriteria.Description;
            SellPrice = productCriteria.SellPrice;
            CostPrice = productCriteria.CostPrice;
        }

        public void ChangeStockQuantity(int quantity)
        {
            StockQuantity += quantity;
        }
    }
}
