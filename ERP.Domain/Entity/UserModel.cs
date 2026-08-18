using ERP.Domain.Criteria;

namespace ERP.Domain.Entity
{
    public class UserModel
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string UserName { get; private set; }
        public string PasswordHashed { get; private set; }
        public string PhoneNumber { get; private set; }
        public string? ProfilePicture { get; private set; }
        public bool Active { get; private set; }

        protected UserModel() { }

        public UserModel(UserCriteria userCriteria)
        {
            FirstName = userCriteria.FirstName;
            LastName = userCriteria.LastName;
            UserName = userCriteria.UserName;
            PasswordHashed = userCriteria.PasswordHashed;
            PhoneNumber = userCriteria.PhoneNumber;
            ProfilePicture = userCriteria.ProfilePicture;
            Active = true;
        }

        public void Edit(UserCriteria userCriteria)
        {
            FirstName = userCriteria.FirstName;
            LastName = userCriteria.LastName;
            UserName = userCriteria.UserName;
            PhoneNumber = userCriteria.PhoneNumber;
            ProfilePicture = userCriteria.ProfilePicture;
        }

        public void Deactive()
        {
            Active = false;
        }
    }
}
