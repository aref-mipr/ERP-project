using ERP.Domain.Criteria;

namespace ERP.Domain.Entity
{
    public class EmployeeModel
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }
        public string Position { get; private set; }
        public string? Description { get; private set; }
        public decimal SalaryMonthly { get; private set; }
        public int SalaryPaymentDay { get; private set; }
        public int EmployeeCode { get; private set; }
        public bool SalaryPayed { get; private set; }
        public DateTime CreationTime { get; private set; }
        public DateTime LastSalaryPaymentDate { get; private set; }
        public EmployeeStatuses EmployeeStatus { get; private set; }
        public AdminModel? Admin { get; private set; }
        public ICollection<FinancialTransactionModel> FinancialTransactions { get; private set; }

        public enum EmployeeStatuses
        {
            Active = 1,
            Resigned = 2,
            Suspended = 3,
            Fired = 4,
            ReEmployment = 5,
        }

        protected EmployeeModel() { }

        public EmployeeModel(EmployeeCriteria employeeCriteria)
        {
            FirstName = employeeCriteria.FirstName;
            LastName = employeeCriteria.LastName;
            Phone = employeeCriteria.Phone;
            Position = employeeCriteria.Position;
            Description = employeeCriteria.Description;
            SalaryMonthly = employeeCriteria.SalaryMonthly;
            EmployeeCode = employeeCriteria.EmployeeCode;
            EmployeeStatus = EmployeeStatuses.Active;
            SalaryPayed = true;
            SalaryPaymentDay = DateTime.Now.Day;
            CreationTime = DateTime.Now;
            LastSalaryPaymentDate = DateTime.Now;
            FinancialTransactions = new List<FinancialTransactionModel>();
        }

        public void Edit(EmployeeCriteria employeeCriteria)
        {
            FirstName = employeeCriteria.FirstName;
            LastName = employeeCriteria.LastName;
            Phone = employeeCriteria.Phone;
            Position = employeeCriteria.Position;
            Description = employeeCriteria.Description;
            SalaryMonthly = employeeCriteria.SalaryMonthly;
        }

        public void PaySalary()
        {
            SalaryPayed = true;
            LastSalaryPaymentDate = DateTime.Now;
        }
        public void SalaryDue()
        {
            SalaryPayed = false;
        }

        public void ChangeStatuses(EmployeeStatuses status)
        {
            EmployeeStatus = status;
        }

        public void ChangeSalaryPaymentDay()
        {
            SalaryPaymentDay = DateTime.Now.Day;
            LastSalaryPaymentDate = DateTime.Now;
        }
    }
}


/*

 */