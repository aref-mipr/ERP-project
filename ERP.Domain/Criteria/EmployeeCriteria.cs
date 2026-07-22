using static ERP.Domain.Entity.EmployeeModel;

namespace ERP.Domain.Criteria
{
    public class EmployeeCriteria
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public string? Description { get; set; }
        public decimal SalaryMonthly { get; set; }
        public int EmployeeCode { get; set; }
    }
}
