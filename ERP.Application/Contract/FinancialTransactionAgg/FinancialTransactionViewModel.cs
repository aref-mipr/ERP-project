namespace ERP.Application.Contract.FinancialTransactionAgg
{
    public class FinancialTransactionViewModel
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string TransactionTime { get; set; }
        public string TransactionType { get; set; }
        public string? ProductName { get; set; }
        public int? OrderCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? SideExpense { get; set; }
    }
}
