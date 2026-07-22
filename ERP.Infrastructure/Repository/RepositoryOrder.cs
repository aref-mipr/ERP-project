using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repository
{
    public class RepositoryOrder : IRepositoryOrder
    {
        private readonly ERPContext _context;
        public RepositoryOrder(ERPContext context)
        {
            _context = context;
        }
        public void Create(OrderModel order)
        {
            _context.Orders.Add(order);
        }
        public OrderModel GetBy(int id)
        {
            return _context.Orders.Include(x => x.Customer).FirstOrDefault(x => x.Id == id);
        }
        public List<OrderModel> GetAll()
        {
            return _context.Orders.AsNoTracking().Include(x => x.OrderItems).Include(x => x.Customer).ToList();
        }

        public bool IsExist(int id)
        {
            return _context.Orders.Any(x => x.Id == id);
        }
        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
