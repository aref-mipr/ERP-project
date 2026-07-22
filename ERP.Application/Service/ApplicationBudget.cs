using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using static ERP.Domain.Entity.FinancialTransactionModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ERP.Application.Service
{
    public class ApplicationBudget : IApplicationBudget
    {
        private readonly IRepositoryBudget _repositoryBudget;
        private readonly IApplicationFinancialTransaction _applicationFinancialTransaction;
        public ApplicationBudget(IRepositoryBudget repositoryBudget, IApplicationFinancialTransaction applicationFinancialTransaction)
        {
            _repositoryBudget = repositoryBudget;
            _applicationFinancialTransaction = applicationFinancialTransaction;
        }
        public void Create(decimal Amount)
        {
            var commandTransaction = new CreateFinancialTransactionDto
            {
                FinancialTransactionsCriteria = new FinancialTransactionCriteria
                {
                    TransactionType = !_repositoryBudget.HasInitialCapital() ? TransactionTypes.OpeningBalance : TransactionTypes.IncreaseBudget,
                    Mount =  Amount,
                }
            };
            var lastBudget = _repositoryBudget.GetLast();
            if (lastBudget != null)
                Amount += lastBudget.TotalBudget;

            var budget = new BudgetModel(Amount);

            _applicationFinancialTransaction.Create(commandTransaction);
            _repositoryBudget.Create(budget);
            _repositoryBudget.SaveChange();
        }

        public void Register(decimal amount)
        {
            var lastBudget = _repositoryBudget.GetLast();
            decimal totalBudget = lastBudget.TotalBudget + amount;
            var budget = new BudgetModel(totalBudget);
            _repositoryBudget.Create(budget);
        }

        public void WithDrawal(decimal amount)
        {
            if(amount > _repositoryBudget.GetLast().TotalBudget)
                throw new ArgumentOutOfRangeException();

            var commandTransaction = new CreateFinancialTransactionDto
            {
                FinancialTransactionsCriteria = new FinancialTransactionCriteria
                {
                    TransactionType = TransactionTypes.OnerWithdrawal,
                    Mount = -amount,
                }
            };
            Register(commandTransaction.FinancialTransactionsCriteria.Mount);
            _applicationFinancialTransaction.Create(commandTransaction);
            _repositoryBudget.SaveChange();
        }
        public BudgetViewModel GetBy(long id)
        {
            var budget = _repositoryBudget.GetBy(id);
            if (budget == null)
                throw new NullReferenceException();

            return new BudgetViewModel
            {
                Id = budget.Id,
                TotalBudget = budget.TotalBudget,
                LastUpdate = budget.LastUpdate.ToString("mm : HH , yyyy/MM/dd"),
            };
        }

        public List<BudgetViewModel> GetAll()
        {
            return _repositoryBudget.GetAll().Select(x => new BudgetViewModel
            {
                Id = x.Id,
                TotalBudget = x.TotalBudget,
                ChangeMount = CalculateChangeBudget(x.Id),
                LastUpdate = x.LastUpdate.ToString("mm : HH , yyyy/MM/dd"),
            }).OrderBy(x => x.Id).ToList();
        }

        public decimal CalculateChangeBudget(long id)
        {
            var currentBudget = _repositoryBudget.GetBy(id);
            var command = _repositoryBudget.GetAll().Where(x => x.Id < currentBudget.Id).OrderByDescending(x => x.Id).FirstOrDefault();
            if (command == null)
                return currentBudget.TotalBudget;
            else
                return currentBudget.TotalBudget - command.TotalBudget;
        }
    }
}
