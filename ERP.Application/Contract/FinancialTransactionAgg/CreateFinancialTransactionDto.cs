using ERP.Domain.Criteria;

namespace ERP.Application.Contract.FinancialTransactionAgg
{
    public class CreateFinancialTransactionDto
    {
        public FinancialTransactionCriteria FinancialTransactionsCriteria { get; set; }
    }
}
