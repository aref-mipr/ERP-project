namespace ERP.Application.Contract.EmployeeAgg
{
    public class EmployeeViewModel: CreateEmployeeDto
    {
        public string FullName { get; set; }
        public string CreationTime { get; set; }
        public string LastSalaryPaymentTime { get; set; }
        public string EmployeeStatus { get; set; }
        public bool SalaryPayed { get; set; }
    }
}
