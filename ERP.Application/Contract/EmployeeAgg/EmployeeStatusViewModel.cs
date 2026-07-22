namespace ERP.Application.Contract.EmployeeAgg
{
    public class EmployeeStatusViewModel
    {
        public string Value { get; set; }
        public string Text { get; set; }

        public EmployeeStatusViewModel(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}
