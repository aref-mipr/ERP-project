using ERP.Domain.Criteria;

namespace ERP.Domain.Entity
{
    public class SideExpenseModel
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime ExpenseRecordingTime { get; private set; }
        public List<FinancialTransactionModel> FinancialTransactions { get; private set; }

        protected SideExpenseModel() { }

        public SideExpenseModel(SideExpenseCriteria sideExpenseCriteria)
        {
            Title = sideExpenseCriteria.Title;
            Description = sideExpenseCriteria.Description;
            Amount = sideExpenseCriteria.Amount;
            ExpenseRecordingTime = DateTime.Now;
            FinancialTransactions = new List<FinancialTransactionModel>();
        }

        public void Edit(SideExpenseCriteria sideExpenseCriteria)
        {
            Title = sideExpenseCriteria.Title;
            Description = sideExpenseCriteria.Description;
            Amount = sideExpenseCriteria.Amount;
        }
    }
}
