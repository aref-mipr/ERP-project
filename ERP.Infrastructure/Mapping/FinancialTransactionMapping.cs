using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Mapping
{
    public class FinancialTransactionMapping: IEntityTypeConfiguration<FinancialTransactionModel>
    {
        public void Configure(EntityTypeBuilder<FinancialTransactionModel> builder)
        {
            builder.ToTable("FinancialTransactions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ProductItemId);
            builder.Property(x => x.ProductId);
            builder.Property(x => x.OrderId);
            builder.Property(x => x.OrderItemId);
            builder.Property(x => x.EmployeeId);
            builder.Property(x => x.SideExpenseId);
            builder.Property(x => x.Amount);
            builder.Property(x => x.Description);
            builder.Property(x => x.TransactionTime);
            builder.Property(x => x.TransactionType);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.FinancialTransactions)
                .HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.ProductItem)
                .WithMany(x => x.FinancialTransactions)
                .HasForeignKey(x => x.ProductItemId);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.FinancialTransactions)
                .HasForeignKey(x => x.OrderId);

            builder.HasOne(x => x.OrderItem)
                .WithMany(x => x.FinancialTransactions)
                .HasForeignKey(x => x.OrderItemId);

            builder.HasOne(x => x.Employee)
                .WithMany(x => x.FinancialTransactions)
                .HasForeignKey(x => x.EmployeeId);

            builder.HasOne(x => x.SideExpense)
                .WithMany(x => x.FinancialTransactions)
                .HasForeignKey(x => x.SideExpenseId);
        }
    }
}
