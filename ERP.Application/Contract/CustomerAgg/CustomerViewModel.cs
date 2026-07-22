using ERP.Domain.Criteria;

namespace ERP.Application.Contract.CustomerAgg
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string CreationTime { get; set; }
        public string FullName { get; set; }
        public CustomerCriteria CustomerCriterias { get; set; }
    }
}
