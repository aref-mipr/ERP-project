namespace ERP.Application.Contract.FinancialTransactionAgg
{
    public interface IApplicationFinancialTransaction
    {
        void Create(CreateFinancialTransactionDto command);
        void RegisterDescription(long id, string description);
        FinancialTransactionViewModel GetBy(long id);
        string GetDescritpion(long id);
        List<FinancialTransactionViewModel> GetAll();
        List<FinancialTransactionViewModel> GetBudgets();
        List<FinancialTransactionTypeModel> CreateStatuses();
    }
}
