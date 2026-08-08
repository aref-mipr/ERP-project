using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using ERP.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using static ERP.Domain.Entity.EmployeeModel;

namespace ERP.Infrastructure.Repository
{
    public class RepositoryEmployee : IRepositoryEmployee
    {
        private readonly ERPContext _context;
        public RepositoryEmployee(ERPContext context)
        {
            _context = context;
        }

        public void Create(EmployeeModel employee)
        {
            _context.Employees.Add(employee);
        }
        public EmployeeModel GetBy(int id)
        {
            return _context.Employees.FirstOrDefault(x => x.Id == id);
        }
        public List<EmployeeModel> GetAll()
        {
            return _context.Employees.AsNoTracking().ToList();
        }

        public bool IsExist(int id)
        {
            return _context.Employees.Any(x => x.Id == id);
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
