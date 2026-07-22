namespace ERP.Application.Contract.FinancialTransactionAgg
{
    public class FinancialTransactionTypeModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public FinancialTransactionTypeModel(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}
