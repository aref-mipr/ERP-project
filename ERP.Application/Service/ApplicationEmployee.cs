using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.EmployeeAgg;
using ERP.Application.Contract.FilterAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using System.Globalization;
using static ERP.Domain.Entity.EmployeeModel;
using static ERP.Domain.Entity.FinancialTransactionModel;

namespace ERP.Application.Service
{
    public class ApplicationEmployee: IApplicationEmployee
    {
        private readonly IRepositoryEmployee _repositoryEmployee;
        private readonly IEnumExtension _enumExtension;
        private readonly IApplicationFinancialTransaction _applicationFinancialTransaction;
        private readonly IApplicationBudget _applicationBudget;
        public ApplicationEmployee(IRepositoryEmployee repositoryEmployee, IEnumExtension enumExtension,
            IApplicationFinancialTransaction applicationFinancialTransaction, IApplicationBudget applicationBudget)
        {
            _repositoryEmployee = repositoryEmployee;
            _enumExtension = enumExtension;
            _applicationFinancialTransaction = applicationFinancialTransaction;
            _applicationBudget = applicationBudget;
        }

        public void Create(CreateEmployeeDto command)
        {
            command.EmployeesCriteria.EmployeeCode = _repositoryEmployee.GetAll().Count() + 1;
            var employee = new EmployeeModel(command.EmployeesCriteria);
            _repositoryEmployee.Create(employee);
            _repositoryEmployee.SaveChange();
        }
        public void Edit(EditEmployeeDto command)
        {
            var quary = _repositoryEmployee.GetBy(command.Id);
            if (quary == null)
                throw new NullReferenceException();

            quary.Edit(command.EmployeesCriteria);
            _repositoryEmployee.SaveChange();
        }
        public EmployeeViewModel GetBy(int id)
        {
            var employee = _repositoryEmployee.GetBy(id);
            if (employee == null)
                throw new NullReferenceException();

            var persianDate = new PersianCalendar();

            return new EmployeeViewModel
            {
                Id = employee.Id,
                CreationTime =
                    $"{persianDate.GetYear(employee.CreationTime):0000}/" +
                    $"{persianDate.GetMonth(employee.CreationTime):00}/" +
                    $"{persianDate.GetDayOfMonth(employee.CreationTime):00}",
                LastSalaryPaymentTime = 
                    $"{persianDate.GetYear(employee.CreationTime):0000}/" +
                    $"{persianDate.GetMonth(employee.CreationTime):00}/" +
                    $"{persianDate.GetDayOfMonth(employee.CreationTime):00}",
                AmountOwed = employee.AmountOwed,
                EmployeeStatus = _enumExtension.EmployeeStatusesToPersianString(employee.EmployeeStatus),
                FullName = $"{employee.FirstName} {employee.LastName}",
                Phone = employee.Phone,
                Position = employee.Position,
                Description = employee.Description,
                SalaryMonthly = employee.SalaryMonthly,
                EmployeeCode = employee.EmployeeCode,
            };
        }
        public EditEmployeeDto GetForEdit(int id)
        {
            var employee = _repositoryEmployee.GetBy(id);
            if (employee == null)
                throw new NullReferenceException();
            return new EditEmployeeDto
            {
                Id = employee.Id,
                EmployeesCriteria = new EmployeeCriteria
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Phone = employee.Phone,
                    Position = employee.Position,
                    SalaryMonthly = employee.SalaryMonthly,
                    Description = employee.Description,
                }
            };
        }
        public List<EmployeeViewModel> GetAll(FilterParamsDto filterParams)
        {
            var employees = _repositoryEmployee.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterParams.Subject))
                employees = employees
                    .Where(x => x.FirstName.Contains(filterParams.Subject) ||
                    x.LastName.Contains(filterParams.Subject) ||
                    x.EmployeeCode.ToString().Contains(filterParams.Subject));

            return employees.OrderByDescending(x => x.EmployeeCode)
                .Skip(filterParams.Skip)
                .Take(filterParams.Take)
                .Select(x => new EmployeeViewModel
                {
                    Id = x.Id,
                    FullName = $"{x.FirstName} {x.LastName}",
                    EmployeeStatus = _enumExtension.EmployeeStatusesToPersianString(x.EmployeeStatus),
                    SalaryPayed = x.SalaryPayed,
                    Phone = x.Phone,
                    Position = x.Position,
                    EmployeeCode = x.EmployeeCode,
                }).ToList();
        }

        public List<EmployeeViewModel> GetAllActive()
        {
            return _repositoryEmployee.GetAll()
                .Where(x => x.EmployeeStatus == EmployeeStatuses.Active || x.EmployeeStatus == EmployeeStatuses.ReEmployment)
                .Select(x => new EmployeeViewModel { })
                .OrderByDescending(x => x.EmployeeCode).ToList();
        }

        public int GetCount(string? subject = null)
        {
            var employees = _repositoryEmployee.GetAll().AsQueryable();
            if (!string.IsNullOrWhiteSpace(subject))
                employees = employees.Where(x => x.FirstName.Contains(subject) ||
                x.LastName.Contains(subject) ||
                    x.EmployeeCode.ToString().Contains(subject));

            return employees.Count();
        }

        public void CheckSalaryStatus()
        {
            var employees = _repositoryEmployee.GetAll()
                .Where(x => x.EmployeeStatus == EmployeeStatuses.Active || x.EmployeeStatus == EmployeeStatuses.ReEmployment);
            foreach(var employee in employees)
            {
                var currentMonthlyDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, employee.SalaryPaymentDay);

                if (DateTime.Now < currentMonthlyDate)
                    currentMonthlyDate.AddMonths(-1);

                if(employee.LastSalaryPaymentDate < currentMonthlyDate)
                {
                    employee.SalaryDue();

                    int arrearsMonth = 12*(DateTime.Now.Year - employee.LastSalaryPaymentDate.Year) +
                        DateTime.Now.Month - employee.LastSalaryPaymentDate.Month;

                    employee.CalculateAmountOwed(employee.SalaryMonthly, arrearsMonth);
                }
                    
            }
            _repositoryEmployee.SaveChange();
        }
        public void PaySalary(int id)
        {
            var employee = _repositoryEmployee.GetBy(id);
            if (employee == null)
                throw new NullReferenceException();

            var commandTransaction = new CreateFinancialTransactionDto
            {
                FinancialTransactionsCriteria = new FinancialTransactionCriteria
                {
                    EmployeeId = id,
                    TransactionType = TransactionTypes.Salary,
                    Amount = -employee.AmountOwed,
                }
            };
            employee.PaySalary();
            _applicationBudget.Register(commandTransaction.FinancialTransactionsCriteria.Amount);
            _applicationFinancialTransaction.Create(commandTransaction);
            _repositoryEmployee.SaveChange();
        }

        public void ChangeStatus(int id, EmployeeStatuses status)
        {
            var quary = _repositoryEmployee.GetBy(id);
            if (quary == null)
                throw new NullReferenceException();

            if(status == EmployeeStatuses.ReEmployment)
            {
                quary.ChangeSalaryPaymentDay();
            }

            if (status != EmployeeStatuses.ReEmployment)
                throw new Exception();

            quary.ChangeStatuses(status);
            _repositoryEmployee.SaveChange();
        }

        public List<EmployeeStatusViewModel> CreateStatuses()
        {
            var statuses = new List<EmployeeStatusViewModel>();

            foreach (EmployeeStatuses status in Enum.GetValues(typeof(EmployeeStatuses)))
            {
                string displayName = status switch
                {
                    EmployeeStatuses.Active => "مشغول به کار",
                    EmployeeStatuses.Resigned => "استعغا داده",
                    EmployeeStatuses.Suspended => "تعلیق شده",
                    EmployeeStatuses.Fired => "اخراج شده",
                    EmployeeStatuses.ReEmployment => "استخدام مجدد",
                    _ => status.ToString()
                };

                statuses.Add(new EmployeeStatusViewModel(((int)status).ToString(), displayName));
            }

            return statuses;
        }
    }
}
