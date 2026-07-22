using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Mapping
{
    public class EmployeeMapping: IEntityTypeConfiguration<EmployeeModel>
    {
        public void Configure(EntityTypeBuilder<EmployeeModel> builder)
        {
            builder.ToTable("Employees");
            builder.HasKey(e => e.Id);
            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.Property(x => x.Phone).HasMaxLength(11);
            builder.Property(x => x.Position);
            builder.Property(x => x.Description);
            builder.Property(x => x.SalaryMonthly);
            builder.Property(x => x.SalaryPaymentDay);
            builder.Property(x => x.EmployeeCode);
            builder.Property(x => x.SalaryPayed);
            builder.Property(x => x.CreationTime);
            builder.Property(x => x.LastSalaryPaymentDate);
            builder.Property(x => x.EmployeeStatus);

            builder.HasMany(x => x.FinancialTransactions)
                .WithOne(x => x.Employee)
                .HasForeignKey(x => x.EmployeeId);
        }
    }
}
