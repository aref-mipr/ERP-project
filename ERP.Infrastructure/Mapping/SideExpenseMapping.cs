using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Mapping
{
    public class SideExpenseMapping: IEntityTypeConfiguration<SideExpenseModel>
    {
        public void Configure(EntityTypeBuilder<SideExpenseModel> builder)
        {
            builder.ToTable("SideExpenses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).HasMaxLength(100);
            builder.Property(x => x.Description);
            builder.Property(x => x.Amount);
            builder.Property(x => x.ExpenseRecordingTime);

            builder.HasMany(x => x.FinancialTransactions)
                .WithOne(x => x.SideExpense)
                .HasForeignKey(x => x.SideExpenseId);
        }
    }
}
