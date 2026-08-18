namespace ERP.Application.Contract.UserAgg
{
    public interface IApplicationUser
    {
        void Register(RegisterUserDto command);
        UserViewModel Login(LoginUserViewModel command);
        void Edit(EditUserDto Command);
        UserViewModel GetBy(int id);
        UserViewModel GetBy(string userName);
        List<UserViewModel> GetAll();
        EditUserDto GetForEdit(int id);
        bool HasActiveUser();
    }
}
