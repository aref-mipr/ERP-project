namespace ERP.Application.Contract.SideExpenseAgg
{
    public class SideExpenseViewModel
    {
        public int Id { get; set; }
        public string ExpenseRecordingTime { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
    }
}
