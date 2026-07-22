using ERP.Domain.Criteria;

namespace ERP.Domain.Entity
{
    public class AdminModel
    {
        public int Id { get; private set; }
        public int EmployeeId { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public AccessLevels AccessLevel { get; private set; }
        public DateTime CreationTime { get; private set; }
        public EmployeeModel Employee { get; private set; }
        public enum AccessLevels
        {
            Base = 1,
            Accountant = 2,
            OrderManager = 3,
            CustomerManager = 4,
            HumanResourcesManager = 5,
            Suspended = 6,
            SuperAdmin = 10,
        }

        protected AdminModel() { }

        public AdminModel(AdminCriteria adminCriteria)
        {
            EmployeeId = adminCriteria.EmployeeId;
            Username = adminCriteria.Username;
            PasswordHash = adminCriteria.PasswordHash;
            AccessLevel = AccessLevels.Base;
            CreationTime = DateTime.Now;
        }
    }
}
