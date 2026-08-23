using ERP.Application.Contract.FilterAgg;

namespace ERP.Application.Contract.BudgetAgg
{
    public interface IApplicationBudget
    {
        void Create(decimal Amount);
        void Register(decimal amount);
        void WithDrawal(decimal amount);
        BudgetViewModel GetBy(long id);
        List<BudgetViewModel> GetAll(FilterParamsDto filterParams);
        decimal CalculateChangeBudget(long id);
        public decimal GetTotalBudget();
        int GetCount();
        decimal CalculateCapitalInDate(int year, int mounth, int day);
        List<string> WeeksForChart();
        List<decimal> CapitalOfWeek();
    }
}
