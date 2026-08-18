using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.CustomerAgg;
using ERP.Application.Contract.EmployeeAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Application.Contract.OrderAgg;
using ERP.Application.Contract.UserAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using static ERP.Application.Contract.FinancialTransactionAgg.DisplayFinancialSummaryViewModel;
using static ERP.Domain.Entity.FinancialTransactionModel;
using static ERP.Domain.Entity.OrderModel;

namespace ERP.Presentation.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IRepositoryBudget _repositoryBudget;
        private readonly IApplicationBudget _applicationBudget;
        private readonly IApplicationOrder _applicationOrder;
        private readonly IApplicationCustomer _applicationCustomer;
        private readonly IApplicationEmployee _applicationEmployee;
        private readonly IApplicationFinancialTransaction _applicationFinancialTransaction;
        private readonly IApplicationUser _applicationUser;
        private readonly IEnumExtension _enumExtension;
        public IndexModel(IRepositoryBudget repositoryBudget, IApplicationBudget applicationBudget,
            IApplicationOrder applicationOrder, IApplicationCustomer applicationCustomer,
            IApplicationEmployee applicationEmployee, IEnumExtension enumExtension,
            IApplicationFinancialTransaction applicationFinancialTransaction, IApplicationUser applicationUser)
        {
            _repositoryBudget = repositoryBudget;
            _applicationBudget = applicationBudget;
            _applicationOrder = applicationOrder;
            _applicationCustomer = applicationCustomer;
            _applicationEmployee = applicationEmployee;
            _applicationFinancialTransaction = applicationFinancialTransaction;
            _applicationUser = applicationUser;
            _enumExtension = enumExtension;
        }

        public decimal TotalBudget { get; set; }
        public bool InitialCapital { get; set; }

        public List<OrderViewModel> Orders { get; set; }
        public List<OrderViewModel> LastOrders { get; set; }
        public List<CustomerViewModel> Customers { get; set; }
        public List<EmployeeViewModel> Employees { get; set; }
        public List<FinancialTransactionViewModel> Transactions { get; set; }
        public UserViewModel UserLogin { get; set; }

        [BindProperty]
        public FinancialSummaryDates Date { get; set; }
        public SelectList DateList { get; set; }

        public List<string> Weeks { get; set; }
        public List<decimal> Capitals { get; set; }
        public decimal MaxCapital { get; set; }
        public decimal MinCapital { get; set; }
        public decimal Middle { get; set; }

        public decimal TotalIncomeLastWeek { get; set; }
        public decimal TotalIncomeLastMonth { get; set; }
        public decimal TotalIncomeLastYear { get; set; }
        public decimal TotalIncomeAllTime { get; set; }
        public decimal TotalExpenseLastWeek { get; set; }
        public decimal TotalExpenseLastMonth { get; set; }
        public decimal TotalExpenseLastYear { get; set; }
        public decimal TotalExpenseAllTime { get; set; }

        
        public void OnGet()
        {
            ViewData["PageTitle"] = "داشبورد";
            ViewData["DashboardActive"] = "active";
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            UserLogin = _applicationUser.GetBy(userId);

            InitialCapital = _repositoryBudget.HasInitialCapital();
            TotalBudget = _applicationBudget.GetTotalBudget();
            Orders = _applicationOrder.GetAllApproved();
            TempData["NumberOfOrders"] = Orders.Count();
            LastOrders = _applicationOrder.GetAll().Take(5).ToList();
            TempData["ApprovedOrder"] = _enumExtension.OrderStatusesToPersianString(OrderStatuses.Approved);
            TempData["ApprovedOrderStyle"] = "approved";
            TempData["CanceledOrder"] = _enumExtension.OrderStatusesToPersianString(OrderStatuses.Canceled);
            TempData["CanceledOrderStyle"] = "canceled";

            Customers = _applicationCustomer.GetAll();
            TempData["NumberOfCustomers"] = Customers.Count();

            Employees = _applicationEmployee.GetAllActive();
            TempData["NumberOfEmployees"] = Employees.Count();

            Transactions = _applicationFinancialTransaction.GetAll().Take(5).ToList();
            TempData["OpeningBalance"] = _enumExtension.TransactionTypesToPersianString(TransactionTypes.OpeningBalance);
            TempData["Purchase"] = _enumExtension.TransactionTypesToPersianString(TransactionTypes.Purchase);
            TempData["ReturnedProduct"] = _enumExtension.TransactionTypesToPersianString(TransactionTypes.ReturnedProduct);
            TempData["Sale"] = _enumExtension.TransactionTypesToPersianString(TransactionTypes.Sale);
            TempData["ReturnedOrderItem"] = _enumExtension.TransactionTypesToPersianString(TransactionTypes.ReturnedOrderItem);
            TempData["Salary"] = _enumExtension.TransactionTypesToPersianString(TransactionTypes.Salary);
            TempData["Expence"] = _enumExtension.TransactionTypesToPersianString(TransactionTypes.Expence);
            TempData["Adjustment"] = _enumExtension.TransactionTypesToPersianString(TransactionTypes.Adjustment);
            TempData["IncreaseBudget"] = _enumExtension.TransactionTypesToPersianString(TransactionTypes.IncreaseBudget);
            TempData["OnerWithdrawal"] = _enumExtension.TransactionTypesToPersianString(TransactionTypes.OnerWithdrawal);

            var dates = _applicationFinancialTransaction.CreateFinancialSummaryDate();
            DateList = new SelectList(dates, "Value", "Text");

            Weeks = _applicationBudget.WeeksForChart();
            Capitals = _applicationBudget.CapitalOfWeek();
            MaxCapital = Capitals.Max();
            MinCapital = Capitals.Where(amount => amount > 0).Min();
            Middle = Capitals.Max()/2;

            TotalIncomeLastMonth = _applicationFinancialTransaction.CalculateTotalIncomeLastMonth();
            TotalIncomeLastWeek = _applicationFinancialTransaction.CalculateTotalIncomeLastWeek();
            TotalIncomeLastYear = _applicationFinancialTransaction.CalculateTotalIncomeLastYear();
            TotalIncomeAllTime = _applicationFinancialTransaction.CalculateTotalIncomeAllTime();
            TotalExpenseLastMonth = _applicationFinancialTransaction.CalculateTotalExpenseLastMonth();
            TotalExpenseLastWeek = _applicationFinancialTransaction.CalculateTotalExpenseLastWeek();
            TotalExpenseLastYear = _applicationFinancialTransaction.CalculateTotalExpenseLastYear();
            TotalExpenseAllTime = _applicationFinancialTransaction.CalculateTotalExpenseAllTime();

            if (TempData["Date"] != null)
            {
                if ((int)TempData["Date"] == (int)FinancialSummaryDates.LastWeek)
                {
                    TempData["IncomeAmount"] = (TotalIncomeLastWeek / 10).ToString("N0");
                    TempData["ExpenceAmount"] = (TotalExpenseLastWeek / 10).ToString("N0");
                    TempData["ProfitAmount"] = (TotalIncomeLastWeek / 10 - TotalExpenseLastWeek / 10).ToString("N0");
                    TempData["Date"] = "هفت روز اخیر";
                }
                else if ((int)TempData["Date"] == (int)FinancialSummaryDates.LastYear)
                {
                    TempData["IncomeAmount"] = (TotalIncomeLastYear / 10).ToString("N0");
                    TempData["ExpenceAmount"] = (TotalExpenseLastYear / 10).ToString("N0");
                    TempData["ProfitAmount"] = (TotalIncomeLastYear / 10 - TotalExpenseLastYear / 10).ToString("N0");
                    TempData["Date"] = "سال اخیر";
                }
                else if ((int)TempData["Date"] == (int)FinancialSummaryDates.AllTime)
                {
                    TempData["IncomeAmount"] = (TotalIncomeAllTime / 10).ToString("N0");
                    TempData["ExpenceAmount"] = (TotalExpenseAllTime / 10).ToString("N0");
                    TempData["ProfitAmount"] = (TotalIncomeAllTime / 10 - TotalExpenseAllTime / 10).ToString("N0");
                    TempData["Date"] = "از ابتدا";
                }
                else
                {
                    TempData["IncomeAmount"] = (TotalIncomeLastMonth / 10).ToString("N0");
                    TempData["ExpenceAmount"] = (TotalExpenseLastMonth / 10).ToString("N0");
                    TempData["ProfitAmount"] = (TotalIncomeLastMonth / 10 - TotalExpenseLastMonth / 10).ToString("N0");
                    TempData["Date"] = "ماه اخیر";
                }
            }
            else
            {
                TempData["IncomeAmount"] = (TotalIncomeLastMonth / 10).ToString("N0");
                TempData["ExpenceAmount"] = (TotalExpenseLastMonth / 10).ToString("N0");
                TempData["ProfitAmount"] = (TotalIncomeLastMonth / 10 - TotalExpenseLastMonth / 10).ToString("N0");
                TempData["Date"] = "ماه اخیر";
            }

        }

        public IActionResult OnPost()
        {
            TempData["Date"] = Date;
            return RedirectToPage();
        }
    }
}
