namespace ERP.Application.Contract.FinancialTransactionAgg
{
    public class FinancialTransactionViewModel : CreateFinancialTransactionDto
    {
        public string TransactionTime { get; set; }
        public string TransactionType { get; set; }
    }
}
