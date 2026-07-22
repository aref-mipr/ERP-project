namespace ERP.Domain.Entity
{
    public class BudgetModel
    {
        public long Id { get; private set; }
        public decimal TotalBudget { get; private set; }
        public DateTime LastUpdate { get; private set; }

        protected BudgetModel() { }

        public BudgetModel(decimal totalBudget)
        {
            TotalBudget = totalBudget;
            LastUpdate = DateTime.Now;
        }
    }
}
