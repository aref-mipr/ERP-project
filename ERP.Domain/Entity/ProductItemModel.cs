using ERP.Domain.Criteria;

namespace ERP.Domain.Entity
{
    public class ProductItemModel
    {
        public long Id { get; private set; }
        public int ProductId { get; private set; }
        public long ProductItemCode { get; private set; }
        public decimal Price { get; private set; }
        public string? Description { get; private set; }
        public ProductItemStatuses ProductItemStatus { get; private set; }
        public ProductModel Product { get; private set; }
        public ICollection<OrderItemModel> OrderItems { get; private set; }
        public ICollection<FinancialTransactionModel> FinancialTransactions { get; private set; }

        public enum ProductItemStatuses
        {
            Testing = 1,
            Approved = 2,
            Returned = 3,
            Selled = 4,
            Unsellable = 5,
            ThrownOut = 6,
            WaitingOrder = 7,
        }

        protected ProductItemModel() { }

        public ProductItemModel(ProductItemCriteria productItemCriteria)
        {
            ProductId = productItemCriteria.ProductId;
            ProductItemCode = productItemCriteria.ProductItemCode;
            Description = productItemCriteria.Description;
            Price = productItemCriteria.Price;
            ProductItemStatus = ProductItemStatuses.Testing;
            FinancialTransactions = new List<FinancialTransactionModel>();
        }
        public void Edit(ProductItemCriteria productItemCriteria)
        {
            Price = productItemCriteria.Price;
            Description = productItemCriteria.Description;
            ProductItemStatus = productItemCriteria.ProductItemStatus;
        }
        public void EditByProduct(decimal price)
        {
            Price = price;
        }

        public void ChangeStatus(ProductItemStatuses status)
        {
            ProductItemStatus = status;
        }
    }
}
