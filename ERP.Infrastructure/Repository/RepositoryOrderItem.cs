using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using static ERP.Domain.Entity.ProductItemModel;

namespace ERP.Infrastructure.Repository
{
    public class RepositoryOrderItem: IRepositoryOrderItem
    {
        private readonly ERPContext _context;
        public RepositoryOrderItem(ERPContext context)
        {
            _context = context;
        }
        public void Create(OrderItemModel order)
        {
            _context.OrderItems.Add(order);
        }
        public void Remove(OrderItemModel orderItem)
        {
            _context.OrderItems.Remove(orderItem);
        }
        public OrderItemModel GetBy(long id)
        {
            return _context.OrderItems.Include(x => x.Order).FirstOrDefault(x => x.Id == id);
        }
        public List<OrderItemModel> GetAll()
        {
            return _context.OrderItems.AsNoTracking().Include(x => x.ProductItem).Include(x => x.ProductItem.Product).ToList();
        }
        public List<OrderItemModel> GetAllBy(int orderId)
        {
            return _context.OrderItems.AsNoTracking().Include(x => x.ProductItem).Include(x => x.ProductItem.Product)
                .Where(x => x.OrderId == orderId).ToList();
        }
        public List<OrderItemModel> GetAllWaitingOrderBy(int orderId)
        {
            return _context.OrderItems.Include(x => x.ProductItem)
                .Include(x => x.ProductItem.Product)
                .Where(x => x.OrderId == orderId && x.ProductItem.ProductItemStatus == ProductItemStatuses.WaitingOrder).ToList();
        }
        public bool IsExist(long id)
        {
            return _context.OrderItems.Any(x => x.Id == id);
        }
        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}