namespace ERP.Application.Contract.CustomerAgg
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string CreationTime { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public int SubscriptionCode { get; set; }
    }
}
