namespace ERP.Application.Contract.BudgetAgg
{
    public class BudgetViewModel
    {
        public long Id { get; set; }
        public decimal ChangeMount { get; set; }
        public decimal TotalBudget { get; set; }
        public string LastUpdate { get; set; }
    }
}
