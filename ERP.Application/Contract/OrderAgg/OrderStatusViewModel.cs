namespace ERP.Application.Contract.OrderAgg
{
    public class OrderStatusViewModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public OrderStatusViewModel(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}
