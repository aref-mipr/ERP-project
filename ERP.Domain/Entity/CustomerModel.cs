using ERP.Domain.Criteria;

namespace ERP.Domain.Entity
{
    public class CustomerModel
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }
        public string? Email { get; private set; }
        public int SubscriptionCode { get; private set; }
        public DateTime CreationTime { get; private set; }
        public ICollection<OrderModel> Orders { get; private set; }

        protected CustomerModel() { }

        public CustomerModel(CustomerCriteria customerCriteria)
        {
            FirstName = customerCriteria.FirstName;
            LastName = customerCriteria.LastName;
            Phone = customerCriteria.Phone;
            Email = customerCriteria.Email;
            SubscriptionCode = customerCriteria.SubscriptionCode;
            CreationTime = DateTime.Now;
            Orders = new List<OrderModel>();
        }

        public void Edit(CustomerCriteria customerCriteria)
        {
            FirstName = customerCriteria.FirstName;
            LastName = customerCriteria.LastName;
            Phone = customerCriteria.Phone;
            Email = customerCriteria.Email;
        }
    }
}
