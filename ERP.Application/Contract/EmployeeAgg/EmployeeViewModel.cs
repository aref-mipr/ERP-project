
namespace ERP.Application.Contract.EmployeeAgg
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string CreationTime { get; set; }
        public string LastSalaryPaymentTime { get; set; }
        public decimal AmountOwed { get; set; }
        public string EmployeeStatus { get; set; }
        public bool SalaryPayed { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public string? Description { get; set; }
        public decimal SalaryMonthly { get; set; }
        public int EmployeeCode { get; set; }
    }
}
