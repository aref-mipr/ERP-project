using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using static ERP.Domain.Entity.FinancialTransactionModel;

namespace ERP.Application.Service
{
    public class ApplicationFinancialTransaction: IApplicationFinancialTransaction
    {
        private readonly IRepositoryFinancialTransaction _repositoryFinancialTransaction;
        private readonly IEnumExtension _enumExtension;
        public ApplicationFinancialTransaction(IRepositoryFinancialTransaction repositoryFinancialTransaction, IEnumExtension enumExtension)
        {
            _repositoryFinancialTransaction = repositoryFinancialTransaction;
            _enumExtension = enumExtension;
        }

        public void Create(CreateFinancialTransactionDto command)
        {
            var financialTransaction = new FinancialTransactionModel(command.FinancialTransactionsCriteria);
            _repositoryFinancialTransaction.Create(financialTransaction);
        }

        public void RegisterDescription(long id, string description)
        {
            var transaction = _repositoryFinancialTransaction.GetBy(id);
            if(transaction == null)
                throw new NullReferenceException();

            transaction.RegisterOrEditDescription(description);
            _repositoryFinancialTransaction.SaveChange();
        }

        public FinancialTransactionViewModel GetBy(long id)
        {
            var financialTransaction = _repositoryFinancialTransaction.GetBy(id);
            return new FinancialTransactionViewModel
            {
                Id = financialTransaction.Id,
                TransactionTime = financialTransaction.TransactionTime.ToString("mm : HH , yyyy/MM/dd"),
                TransactionType = _enumExtension.TransactionTypesToPersianString(financialTransaction.TransactionType),
                Amount = financialTransaction.Amount,
                Description = financialTransaction.Description,
            };
        }
        public string GetDescritpion(long id)
        {
            var transaction = _repositoryFinancialTransaction.GetBy(id);
            if (transaction == null)
                throw new NullReferenceException();

            return _repositoryFinancialTransaction.GetBy(id).Description;
        }
        public List<FinancialTransactionViewModel> GetAll()
        {
            return _repositoryFinancialTransaction.GetAll().Select(x => new FinancialTransactionViewModel
            {
                Id = x.Id,
                TransactionType = _enumExtension.TransactionTypesToPersianString(x.TransactionType),
                Amount = x.Amount,
                Description = x.Description,
            }).OrderBy(x => x.Id).ToList();
        }

        public List<FinancialTransactionViewModel> GetBudgets()
        {
            return _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType == TransactionTypes.OpeningBalance ||
                x.TransactionType == TransactionTypes.IncreaseBudget).Select(x => new FinancialTransactionViewModel
                {
                    Id = x.Id,
                    TransactionTime = x.TransactionTime.ToString("mm : HH , yyyy/MM/dd"),
                    TransactionType = _enumExtension.TransactionTypesToPersianString(x.TransactionType),
                    Amount = x.Amount,
                }).OrderBy(x => x.Id).ToList();
        }

        public List<FinancialTransactionTypeModel> CreateStatuses()
        {
            var statuses = new List<FinancialTransactionTypeModel>();

            foreach (TransactionTypes status in Enum.GetValues(typeof(TransactionTypes)))
            {
                string displayName = status switch
                {
                    TransactionTypes.OpeningBalance => "سرمایه اولیه",
                    TransactionTypes.Purchase => "خرید کالا",
                    TransactionTypes.ReturnedProduct => "مرجوع شده توسط فروشگاه",
                    TransactionTypes.Sale => "ثبت سفارش",
                    TransactionTypes.ReturnedOrderItem => "مرجوع شده توسط مشتری",
                    TransactionTypes.Salary => "پرداخت دستمزد ",
                    TransactionTypes.Expence => "هزینه جانبی",
                    TransactionTypes.Adjustment => "اصلاحیه",
                    TransactionTypes.IncreaseBudget => "افزایش سرمایه",
                    TransactionTypes.OnerWithdrawal => "برداشت شخصی از سرمایه",
                    _ => status.ToString()
                };

                statuses.Add(new FinancialTransactionTypeModel(((int)status).ToString(), displayName));
            }

            return statuses;
        }
    }
}
