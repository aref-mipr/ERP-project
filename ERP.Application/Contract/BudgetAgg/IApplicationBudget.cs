namespace ERP.Application.Contract.BudgetAgg
{
    public interface IApplicationBudget
    {
        void Create(decimal Amount);
        void Register(decimal amount);
        void WithDrawal(decimal amount);
        BudgetViewModel GetBy(long id);
        List<BudgetViewModel> GetAll();
        decimal CalculateChangeBudget(long id);
    }
}
