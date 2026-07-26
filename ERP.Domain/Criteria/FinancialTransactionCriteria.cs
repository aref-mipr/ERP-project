using static ERP.Domain.Entity.FinancialTransactionModel;

namespace ERP.Domain.Criteria
{
    public class FinancialTransactionCriteria
    {
        public long? ProductItemId { get; set; }
        public int? ProductId { get; set; }
        public int? OrderId { get; set; }
        public long? OrderItemId { get; set; }
        public int? EmployeeId { get; set; }
        public int? SideExpenseId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public TransactionTypes TransactionType { get; set; }
    }
}
