using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repository
{
    public class RepositoryUser : IRepositoryUser
    {
        private readonly ERPContext _context;
        public RepositoryUser(ERPContext context)
        {
            _context = context;
        }

        public void Register(UserModel user)
        {
            _context.Users.Add(user);
        }

        public UserModel GetBy(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public UserModel GetBy(string userName)
        {
            return _context.Users.FirstOrDefault(x => x.UserName == userName);
        }

        public List<UserModel> GetAll()
        {
            return _context.Users.AsNoTracking().ToList();
        }

        public bool Exist(int id)
        {
            return _context.Users.Any(x => x.Id == id);
        }

        public bool HasUser()
        {
            return _context.Users.Any();
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
