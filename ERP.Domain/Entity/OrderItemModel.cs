
using ERP.Domain.Criteria;

namespace ERP.Domain.Entity
{
    public class OrderItemModel
    {
        public long Id { get; private set; }
        public int OrderId { get; private set; }
        public long ProductItemId { get; private set; }
        public decimal Price { get; private set; }
        public bool Returned { get; private set; }
        public DateTime CreationTime { get; private set; }
        public OrderModel Order { get; private set; }
        public ProductItemModel ProductItem { get; private set; }
        public List<FinancialTransactionModel> FinancialTransactions { get; private set; }

        protected OrderItemModel() { }
        public OrderItemModel(OrderItemCriteria orderItemCriteria)
        {
            OrderId = orderItemCriteria.OrderId;
            ProductItemId = orderItemCriteria.ProductItemId;
            Price = orderItemCriteria.Price;
            Returned = false;
            CreationTime = DateTime.Now;
            FinancialTransactions = new List<FinancialTransactionModel>();
        }

        public void Return()
        {
            Returned = true;
        }
    }
}
