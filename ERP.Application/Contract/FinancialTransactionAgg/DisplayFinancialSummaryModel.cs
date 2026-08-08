namespace ERP.Application.Contract.FinancialTransactionAgg
{
    public class DisplayFinancialSummaryModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public DisplayFinancialSummaryModel(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}
