using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Mapping
{
    public class AdminMapping: IEntityTypeConfiguration<AdminModel>
    {
        public void Configure(EntityTypeBuilder<AdminModel> builder)
        {
            builder.ToTable("Admins");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.EmployeeId);
            builder.Property(x => x.Username).HasMaxLength(50);
            builder.Property(x => x.PasswordHash);
            builder.Property(x => x.AccessLevel);
            builder.Property(x => x.CreationTime);

            builder.HasOne(x => x.Employee)
                .WithOne(x => x.Admin)
                .HasForeignKey<AdminModel>(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
