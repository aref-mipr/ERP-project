using ERP.Domain.Entity;

namespace ERP.Domain.Interface.Repository
{
    public interface IRepositoryUser
    {
        void Register(UserModel user);
        UserModel GetBy(int id);
        UserModel GetBy(string userName);
        List<UserModel> GetAll();
        bool Exist(int id);
        bool HasUser();
        void SaveChange();

    }
}
