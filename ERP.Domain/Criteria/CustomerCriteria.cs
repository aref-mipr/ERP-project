namespace ERP.Domain.Criteria
{
    public class CustomerCriteria
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public int SubscriptionCode { get; set; }
    }
}
