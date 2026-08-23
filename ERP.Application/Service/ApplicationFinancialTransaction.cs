using ERP.Application.Contract.FilterAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using System.Globalization;
using static ERP.Application.Contract.FinancialTransactionAgg.DisplayFinancialSummaryViewModel;
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
            var persianDate = new PersianCalendar();
            return new FinancialTransactionViewModel
            {
                Id = financialTransaction.Id,
                TransactionTime =
                        $"{financialTransaction.TransactionTime:HH:mm} , " +
                        $"{persianDate.GetYear(financialTransaction.TransactionTime):0000}/" +
                        $"{persianDate.GetMonth(financialTransaction.TransactionTime):00}/" +
                        $"{persianDate.GetDayOfMonth(financialTransaction.TransactionTime):00}",
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
            var transactions = _repositoryFinancialTransaction.GetAll();
            var persianDate = new PersianCalendar();

            return transactions
               .OrderByDescending(x => x.TransactionTime)
               .Select(x => new FinancialTransactionViewModel
               {
                   Id = x.Id,
                   TransactionType = _enumExtension.TransactionTypesToPersianString(x.TransactionType),
                   Amount = x.Amount,
                   TransactionTime =
                       $"{x.TransactionTime:HH:mm} , " +
                       $"{persianDate.GetYear(x.TransactionTime):0000}/" +
                       $"{persianDate.GetMonth(x.TransactionTime):00}/" +
                       $"{persianDate.GetDayOfMonth(x.TransactionTime):00}",
                   ProductName = x.Product?.Name ?? "",
                   OrderCode = x.Order?.OrderCode ?? 0,
                   EmployeeName = $"{x.Employee?.FirstName ?? ""} {x.Employee?.LastName ?? ""}".Trim(),
                   SideExpense = x.SideExpense?.Title ?? "",
               }).ToList();
        }

        public List<FinancialTransactionViewModel> GetAll(FilterParamsDto filterParams)
        {
            var transactions = _repositoryFinancialTransaction.GetAll();
            var persianDate = new PersianCalendar();

            if (!string.IsNullOrWhiteSpace(filterParams.Subject))
                transactions = transactions.Where(x => _enumExtension.TransactionTypesToPersianString(x.TransactionType).Contains(filterParams.Subject)).ToList();

             return transactions
                .OrderByDescending(x => x.TransactionTime)
                .Skip(filterParams.Skip)
                .Take(filterParams.Take)
                .Select(x => new FinancialTransactionViewModel
            {
                Id = x.Id,
                TransactionType = _enumExtension.TransactionTypesToPersianString(x.TransactionType),
                Amount = x.Amount,
                TransactionTime =
                    $"{x.TransactionTime:HH:mm} , " +
                    $"{persianDate.GetYear(x.TransactionTime):0000}/" +
                    $"{persianDate.GetMonth(x.TransactionTime):00}/" +
                    $"{persianDate.GetDayOfMonth(x.TransactionTime):00}",
                ProductName = x.Product?.Name ?? "",
                OrderCode = x.Order?.OrderCode ?? 0,
                EmployeeName = $"{x.Employee?.FirstName ?? ""} {x.Employee?.LastName ?? ""}".Trim(),
                SideExpense = x.SideExpense?.Title ?? "",
            }).ToList();
        }

        public List<FinancialTransactionViewModel> GetBudgets(FilterParamsDto filterParams)
        {
            var transactions = _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType == TransactionTypes.OpeningBalance ||
                x.TransactionType == TransactionTypes.IncreaseBudget).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterParams.Subject))
                transactions = transactions.Where(x => _enumExtension.TransactionTypesToPersianString(x.TransactionType).Contains(filterParams.Subject));

            var persianDate = new PersianCalendar();

            return transactions
               .OrderByDescending(x => x.TransactionTime)
               .Skip(filterParams.Skip)
               .Take(filterParams.Take).Select(x => new FinancialTransactionViewModel
               {
                   Id = x.Id,
                   TransactionTime =
                       $"{x.TransactionTime:HH:mm} , " +
                       $"{persianDate.GetYear(x.TransactionTime):0000}/" +
                       $"{persianDate.GetMonth(x.TransactionTime):00}/" +
                       $"{persianDate.GetDayOfMonth(x.TransactionTime):00}",
                   TransactionType = _enumExtension.TransactionTypesToPersianString(x.TransactionType),
                   Amount = x.Amount,
               }).ToList();
        }

        public int GetCount(string? subject = null)
        {
            var transactions = _repositoryFinancialTransaction.GetAll().AsQueryable();
            if (!string.IsNullOrWhiteSpace(subject))
                transactions = transactions.Where(x => _enumExtension.TransactionTypesToPersianString(x.TransactionType).Contains(subject));

            return transactions.Count();
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

        public List<DisplayFinancialSummaryModel> CreateFinancialSummaryDate()
        {
            var summaryDates = new List<DisplayFinancialSummaryModel>();

            foreach(FinancialSummaryDates summaryDate in Enum.GetValues(typeof(FinancialSummaryDates)))
            {
                string displayName = summaryDate switch
                {
                    FinancialSummaryDates.LastWeek => "هفت روز اخیر",
                    FinancialSummaryDates.LastMounth => "ماه اخیر",
                    FinancialSummaryDates.LastYear => "سال اخیر",
                    FinancialSummaryDates.AllTime => "از ابتدا",
                    _ => summaryDates.ToString()
                };
                summaryDates.Add(new DisplayFinancialSummaryModel(((int)summaryDate).ToString(), displayName));
            }
            return summaryDates;
        }

        public decimal CalculateTotalIncomeLastWeek()
        {
            return _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OpeningBalance &&
                x.TransactionType != TransactionTypes.IncreaseBudget &&
                x.TransactionTime >= DateTime.Now.AddDays(-7) &&
                x.Amount > 0).ToList().Sum(x => x.Amount);
        }

        public decimal CalculateTotalIncomeLastMonth()
        {
            return _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OpeningBalance &&
                x.TransactionType != TransactionTypes.IncreaseBudget &&
                x.TransactionTime.Month == DateTime.Now.Month &&
                x.Amount > 0).ToList().Sum(x => x.Amount);
        }

        public decimal CalculateTotalIncomeLastYear()
        {
            var income = _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OpeningBalance &&
                x.TransactionType != TransactionTypes.IncreaseBudget &&
                x.TransactionTime.Year == DateTime.Now.Year &&
                x.Amount > 0).ToList();
            decimal result = income.Sum(x => x.Amount);

            return _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OpeningBalance &&
                x.TransactionType != TransactionTypes.IncreaseBudget &&
                x.TransactionTime.Year == DateTime.Now.Year &&
                x.Amount > 0).ToList().Sum(x => x.Amount);
        }

        public decimal CalculateTotalIncomeAllTime()
        {
            var income = _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OpeningBalance &&
                x.TransactionType != TransactionTypes.IncreaseBudget &&
                x.Amount > 0).ToList();
            decimal result = income.Sum(x => x.Amount);
            return _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OpeningBalance &&
                x.TransactionType != TransactionTypes.IncreaseBudget &&
                x.Amount > 0).ToList().Sum(x => x.Amount);
        }

        public decimal CalculateTotalExpenseLastWeek()
        {
            var expense = _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OnerWithdrawal &&
                x.TransactionTime >= DateTime.Now.AddDays(-7) &&
                x.Amount < 0).ToList();
            decimal result = expense.Sum(x => x.Amount * (-1));
            return _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OnerWithdrawal &&
                x.TransactionTime >= DateTime.Now.AddDays(-7) &&
                x.Amount < 0).ToList().Sum(x => x.Amount * (-1));
        }

        public decimal CalculateTotalExpenseLastMonth()
        {
            var expense = _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OnerWithdrawal &&
                x.TransactionTime.Month == DateTime.Now.Month &&
                x.Amount < 0).ToList();
            decimal result = expense.Sum(x => x.Amount * (-1));
            return _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OnerWithdrawal &&
                x.TransactionTime.Month == DateTime.Now.Month &&
                x.Amount < 0).ToList().Sum(x => x.Amount * (-1));
        }

        public decimal CalculateTotalExpenseLastYear()
        {
            var expense = _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OnerWithdrawal &&
                x.TransactionTime.Year == DateTime.Now.Year &&
                x.Amount < 0).ToList();
            decimal result = expense.Sum(x => x.Amount * (-1));
            return _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OnerWithdrawal &&
                x.TransactionTime.Year == DateTime.Now.Year &&
                x.Amount < 0).ToList().Sum(x => x.Amount * (-1));
        }

        public decimal CalculateTotalExpenseAllTime()
        {
            var expense = _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OnerWithdrawal &&
                x.Amount < 0).ToList();
            decimal result = expense.Sum(x => x.Amount * (-1));
            return _repositoryFinancialTransaction.GetAll()
                .Where(x => x.TransactionType != TransactionTypes.OnerWithdrawal &&
                x.Amount < 0).ToList().Sum(x => x.Amount * (-1));
        }

    }
}
