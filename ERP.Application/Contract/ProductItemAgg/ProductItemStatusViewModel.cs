namespace ERP.Application.Contract.ProductItemAgg
{
    public class ProductItemStatusViewModel
    {
        public string Value { get; set; }
        public string Text { get; set; }

        public ProductItemStatusViewModel(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}
