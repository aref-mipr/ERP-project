namespace ERP.Presentation.Authentication
{
    public interface IAuthenticationService
    {
        Task SignIn(string userId, string fullName);
        Task SignOut();
    }
}
