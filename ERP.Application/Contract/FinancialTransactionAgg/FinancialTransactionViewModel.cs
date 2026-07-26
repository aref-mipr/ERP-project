namespace ERP.Application.Contract.FinancialTransactionAgg
{
    public class FinancialTransactionViewModel
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string TransactionTime { get; set; }
        public string TransactionType { get; set; }
    }
}
