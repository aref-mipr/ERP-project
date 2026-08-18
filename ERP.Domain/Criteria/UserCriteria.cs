namespace ERP.Domain.Criteria
{
    public class UserCriteria
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string? PasswordHashed { get; set; }
        public string PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
