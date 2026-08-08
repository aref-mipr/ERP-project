namespace ERP.Application.Contract.FinancialTransactionAgg
{
    public class DisplayFinancialSummaryViewModel
    {
        public FinancialSummaryDates FinancialSummaryDate { get; set; }
        public enum FinancialSummaryDates
        {
            LastWeek = 1,
            LastMounth = 2,
            LastYear = 3,
            AllTime = 4,
        }
    }
}
