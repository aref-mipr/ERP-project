using ERP.Application.Contract.FilterAgg;

namespace ERP.Application.Contract.FinancialTransactionAgg
{
     public interface IApplicationFinancialTransaction
     {
         void Create(CreateFinancialTransactionDto command);
         void RegisterDescription(long id, string description);
         FinancialTransactionViewModel GetBy(long id);
         string GetDescritpion(long id);
        List<FinancialTransactionViewModel> GetAll();
         List<FinancialTransactionViewModel> GetAll(FilterParamsDto filterParams);
         List<FinancialTransactionViewModel> GetBudgets(FilterParamsDto filterParams);
        int GetCount(string? subject = null);
         List<FinancialTransactionTypeModel> CreateStatuses();
         List<DisplayFinancialSummaryModel> CreateFinancialSummaryDate();
         decimal CalculateTotalIncomeLastWeek();
         decimal CalculateTotalIncomeLastMonth();
         decimal CalculateTotalIncomeLastYear();
         decimal CalculateTotalIncomeAllTime();
         decimal CalculateTotalExpenseLastWeek();
         decimal CalculateTotalExpenseLastMonth();
         decimal CalculateTotalExpenseLastYear();
         decimal CalculateTotalExpenseAllTime();
     }
}
