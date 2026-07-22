using ERP.Domain.Criteria;

namespace ERP.Application.Contract.FinancialTransactionAgg
{
    public class CreateFinancialTransactionDto
    {
        public long Id { get; set; }
        public FinancialTransactionCriteria FinancialTransactionsCriteria { get; set; }
    }
}
