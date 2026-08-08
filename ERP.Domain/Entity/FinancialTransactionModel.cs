using ERP.Domain.Criteria;

namespace ERP.Domain.Entity
{
    public class FinancialTransactionModel
    {
        public long Id { get; private set; }
        public long? ProductItemId { get; private set; }
        public int? ProductId { get; private set; }
        public int? OrderId { get; private set; }
        public long? OrderItemId { get; private set; }
        public int? EmployeeId { get; private set; }
        public int? SideExpenseId { get; private set; }
        public decimal Amount { get; private set; }
        public string? Description { get; private set; }
        public DateTime TransactionTime { get; private set; }
        public TransactionTypes TransactionType { get; private set; }
        public ProductItemModel ProductItem { get; private set; }
        public ProductModel Product { get; private set; }
        public OrderModel Order { get; private set; }
        public OrderItemModel OrderItem { get; private set; }
        public EmployeeModel Employee { get; private set; }
        public SideExpenseModel SideExpense { get; private set; }

        public enum TransactionTypes
        {
            OpeningBalance = 1,
            Purchase = 2,
            ReturnedProduct = 3,
            Sale = 4,
            ReturnedOrderItem = 5,
            Salary = 6,
            Expence = 7,
            Adjustment = 8,
            IncreaseBudget = 9,
            OnerWithdrawal = 10,
        }

        protected FinancialTransactionModel() { }

        public FinancialTransactionModel(FinancialTransactionCriteria finalTransactionCriteria)
        {
            ProductItemId = finalTransactionCriteria.ProductItemId;
            ProductId = finalTransactionCriteria.ProductId;
            OrderId = finalTransactionCriteria.OrderId;
            OrderItemId = finalTransactionCriteria.OrderItemId;
            EmployeeId = finalTransactionCriteria.EmployeeId;
            SideExpenseId = finalTransactionCriteria.SideExpenseId;
            Amount = finalTransactionCriteria.Amount;
            Description = finalTransactionCriteria.Description;
            TransactionType = finalTransactionCriteria.TransactionType;
            TransactionTime = DateTime.Now;
        }

        public void RegisterOrEditDescription(string description)
        {
            Description = description;
        }
    }
}