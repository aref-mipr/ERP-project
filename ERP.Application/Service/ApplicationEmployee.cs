using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.EmployeeAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using static ERP.Domain.Entity.EmployeeModel;
using static ERP.Domain.Entity.FinancialTransactionModel;
using static ERP.Domain.Entity.OrderModel;

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
            if (_repositoryEmployee.IsExist(command.Id))
                throw new NullReferenceException();

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

            return new EmployeeViewModel
            {
                Id = employee.Id,
                CreationTime = employee.CreationTime.ToString("yyyy/MM/dd"),
                LastSalaryPaymentTime = employee.LastSalaryPaymentDate.ToString("yyyy/MM/dd"),
                EmployeeStatus = _enumExtension.EmployeeStatusesToPersianString(employee.EmployeeStatus),
                FullName = $"{employee.FirstName} {employee.LastName}",
                EmployeesCriteria = new EmployeeCriteria
                {
                    Phone = employee.Phone,
                    Position = employee.Position,
                    Description = employee.Description,
                    SalaryMonthly = employee.SalaryMonthly,
                    EmployeeCode = employee.EmployeeCode,
                }
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
        public List<EmployeeViewModel> GetAll()
        {
            var quary = _repositoryEmployee.GetAll();
            var employees = quary.Select(x => new EmployeeViewModel
            {
                Id = x.Id,
                FullName = $"{x.FirstName} {x.LastName}",
                EmployeeStatus = _enumExtension.EmployeeStatusesToPersianString(x.EmployeeStatus),
                SalaryPayed = x.SalaryPayed,
                EmployeesCriteria = new EmployeeCriteria
                {
                    Phone = x.Phone,
                    Position = x.Position,
                    EmployeeCode = x.EmployeeCode,
                }
            }).ToList();
            return employees;
        }

        public EmployeeStatuses GetPreviousStatus(EmployeeStatuses previousStatus)
        {
            return previousStatus;
        }
        public void CheckSalaryStatus()
        {
            var employees = _repositoryEmployee.GetAllActive();
            foreach(var employee in employees)
            {
                if(employee.LastSalaryPaymentDate.Month != DateTime.Now.Month &&
                    employee.SalaryPaymentDay <= DateTime.Now.Day)
                {
                    employee.SalaryDue();
                }
            }
            _repositoryEmployee.SaveChange();
        }
        public void PaySalary(int id)
        {
            var employee = _repositoryEmployee.GetBy(id);
            if (employee == null)
                throw new NullReferenceException();

            employee.PaySalary();
            var commandTransaction = new CreateFinancialTransactionDto
            {
                FinancialTransactionsCriteria = new FinancialTransactionCriteria
                {
                    EmployeeId = id,
                    TransactionType = TransactionTypes.Salary,
                    Mount = -employee.SalaryMonthly,
                }
            };
            _applicationBudget.Register(commandTransaction.FinancialTransactionsCriteria.Mount);
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
