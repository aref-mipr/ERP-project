using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.FilterAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Application.Contract.SideExpenseAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using System.Globalization;
using static ERP.Domain.Entity.FinancialTransactionModel;

namespace ERP.Application.Service
{
    public class ApplicationSideExpense : IApplicationSideExpense
    {
        private readonly IRepositorySideExpense _repositorySideExpense;
        private readonly IApplicationFinancialTransaction _applicationFinancialTransaction;
        private readonly IApplicationBudget _applicationBudget;
        public ApplicationSideExpense(IRepositorySideExpense repositorySideExpense,
            IApplicationFinancialTransaction applicationFinancialTransaction, IApplicationBudget applicationBudget)
        {
            _repositorySideExpense = repositorySideExpense;
            _applicationFinancialTransaction = applicationFinancialTransaction;
            _applicationBudget = applicationBudget;
        }
        public void Create(CreateSideExpenseDto command)
        {
            var sideExpense = new SideExpenseModel(command.SideExpensesCriteria);
            _repositorySideExpense.Create(sideExpense);
            _repositorySideExpense.SaveChange();
            var commandTransaction = new CreateFinancialTransactionDto
            {
                FinancialTransactionsCriteria = new FinancialTransactionCriteria
                {
                    SideExpenseId = sideExpense.Id,
                    TransactionType = TransactionTypes.Expence,
                    Amount = -sideExpense.Amount,
                }
            };
            _applicationBudget.Register(commandTransaction.FinancialTransactionsCriteria.Amount);
            _applicationFinancialTransaction.Create(commandTransaction);
            _repositorySideExpense.SaveChange();
        }

        public void Edit(EditSideExpenseDto command)
        {
            var quary = _repositorySideExpense.GetBy(command.Id);
            if (quary == null)
                throw new NullReferenceException();

            if(command.SideExpensesCriteria.Amount != quary.Amount)
            {
                var commandTransaction = new CreateFinancialTransactionDto
                {
                    FinancialTransactionsCriteria = new FinancialTransactionCriteria
                    {
                        SideExpenseId = command.Id,
                        TransactionType = TransactionTypes.Adjustment,
                        Amount = quary.Amount - command.SideExpensesCriteria.Amount,
                    }
                };
                _applicationBudget.Register(commandTransaction.FinancialTransactionsCriteria.Amount);
                _applicationFinancialTransaction.Create(commandTransaction);
            }
            quary.Edit(command.SideExpensesCriteria);
            _repositorySideExpense.SaveChange();
        }

        public List<SideExpenseViewModel> GetAll(FilterParamsDto filterParams)
        {
            var sideExpenses = _repositorySideExpense.GetAll().AsQueryable();
            var persianDate = new PersianCalendar();

            if (!string.IsNullOrWhiteSpace(filterParams.Subject))
                sideExpenses = sideExpenses
                    .Where(x => x.Title.Contains(filterParams.Subject));

            return sideExpenses.OrderByDescending(x => x.ExpenseRecordingTime)
                .Skip(filterParams.Skip)
                .Take(filterParams.Take)
                .Select(x => new SideExpenseViewModel
                {
                    Id = x.Id,
                    ExpenseRecordingTime =
                        $"{x.ExpenseRecordingTime:HH:mm} , " +
                        $"{persianDate.GetYear(x.ExpenseRecordingTime):0000}/" +
                        $"{persianDate.GetMonth(x.ExpenseRecordingTime):00}/" +
                        $"{persianDate.GetDayOfMonth(x.ExpenseRecordingTime):00}",
                    Title = x.Title,
                    Amount = x.Amount,
                }).ToList();
        }

        public SideExpenseViewModel GetBy(int id)
        {
            var sideExpense = _repositorySideExpense.GetBy(id);
            var persianDate = new PersianCalendar();

            return new SideExpenseViewModel
            {
                Id = id,
                ExpenseRecordingTime =
                    $"{sideExpense.ExpenseRecordingTime:HH:mm} , " +
                    $"{persianDate.GetYear(sideExpense.ExpenseRecordingTime):0000}/" +
                    $"{persianDate.GetMonth(sideExpense.ExpenseRecordingTime):00}/" +
                    $"{persianDate.GetDayOfMonth(sideExpense.ExpenseRecordingTime):00}",
                Title = sideExpense.Title,
                Description = sideExpense.Description,
                Amount = sideExpense.Amount,
            };
        }

        public EditSideExpenseDto GetForEdit(int id)
        {
            var sideExpense = _repositorySideExpense.GetBy(id);
            return new EditSideExpenseDto
            {
                Id = id,
                SideExpensesCriteria = new SideExpenseCriteria
                {
                    Title = sideExpense.Title,
                    Description = sideExpense.Description,
                    Amount = sideExpense.Amount,
                }
            };
        }

        public int GetCount(string? subject = null)
        {
            var sideExpenses = _repositorySideExpense.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(subject))
                sideExpenses = sideExpenses
                    .Where(x => x.Title.Contains(subject));

            return sideExpenses.Count();
        }
    }
}
