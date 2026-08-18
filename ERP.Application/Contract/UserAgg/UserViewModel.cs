using ERP.Domain.Criteria;

namespace ERP.Application.Contract.UserAgg
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public UserCriteria UsersCriteria { get; set; }
    }
}
