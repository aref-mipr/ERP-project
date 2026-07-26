using ERP.Domain.Criteria;

namespace ERP.Domain.Entity
{
    public class OrderModel
    {
        public int Id { get; private set; }
        public int CustomerId { get; private set; }
        public int OrderCode { get; private set; }
        public string? Description { get; private set; }
        public OrderStatuses OrderStatus { get; private set; }
        public decimal InitialAmount { get; private set; }
        public decimal DiscountAmount { get; private set; }
        public decimal FinalAmount { get; private set; }
        public DateTime CreationTime { get; private set; }
        public CustomerModel Customer { get; private set; }
        public ICollection<OrderItemModel> OrderItems { get; private set; }
        public ICollection<FinancialTransactionModel> FinancialTransactions { get; private set; }
        public enum OrderStatuses
        {
            Pending = 1,
            Approved = 2,
            Canceled = 3,
        }

        protected OrderModel() { }
        public OrderModel(OrderCriteria orderCriteria)
        {
            CustomerId = orderCriteria.CustomerId;
            OrderCode = orderCriteria.OrderCode;
            Description = orderCriteria.Description;
            InitialAmount = orderCriteria.InitialAmount;
            DiscountAmount = orderCriteria.DiscountAmount;
            OrderStatus = OrderStatuses.Pending;
            FinalAmount = orderCriteria.FinalAmount;
            CreationTime = DateTime.Now;
            OrderItems = new List<OrderItemModel>();
            FinancialTransactions = new List<FinancialTransactionModel>();
        }

        public void Edit(OrderCriteria orderCriteria)
        {
            CustomerId = orderCriteria.CustomerId;
            Description = orderCriteria.Description;
            InitialAmount = orderCriteria.InitialAmount;
            DiscountAmount = orderCriteria.DiscountAmount;
            FinalAmount = orderCriteria.FinalAmount;
        }

        public void ChangeStatus(OrderStatuses status)
        {
            OrderStatus = status;
        }
    }
}
