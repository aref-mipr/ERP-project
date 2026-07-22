using ERP.Infrastructure.Mapping;
using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Context
{

    public class ERPContext: DbContext
    {
        public ERPContext(DbContextOptions<ERPContext> options): base(options)
        {

        }
        public DbSet<AdminModel> Admins { get; set; }
        public DbSet<BudgetModel> Budgets { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<FinancialTransactionModel> FinancialTransactions { get; set; }
        public DbSet<OrderItemModel> OrderItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<ProductCategoryModel> ProductCategories { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductItemModel> ProductItems { get; set; }
        public DbSet<SideExpenseModel> SideExpenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderMapping).Assembly);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));

                foreach (var property in properties)
                {
                    property.SetColumnType("decimal(18,2)");
                }
            }
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ERPContext).Assembly);
            base.OnModelCreating(modelBuilder);

        }
    }
}
