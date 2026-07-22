using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repository
{
    public class RepositoryCustomer : IRepositoryCustomer
    {
        private readonly ERPContext _context;
        public RepositoryCustomer(ERPContext context)
        {
            _context = context;
        }
        public void Create(CustomerModel customer)
        {
            _context.Customers.Add(customer);
        }
        public CustomerModel GetBy(int id)
        {
            return _context.Customers.FirstOrDefault(x => x.Id == id);
        }

        public List<CustomerModel> GetAll()
        {
            return _context.Customers.AsNoTracking().ToList();
        }

        public bool IsExist(int id)
        {
            return _context.Customers.Any(x => x.Id == id);
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
