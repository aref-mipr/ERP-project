using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Mapping
{
    public class OrderMapping: IEntityTypeConfiguration<OrderModel>
    {
        public void Configure(EntityTypeBuilder<OrderModel> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CustomerId);
            builder.Property(x => x.OrderCode);
            builder.Property(x => x.Description);
            builder.Property(x => x.OrderStatus);
            builder.Property(x => x.InitialAmount);
            builder.Property(x => x.DiscountAmount);
            builder.Property(x => x.FinalAmount);
            builder.Property(x => x.CreationTime);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);

            builder.HasMany(x => x.FinancialTransactions)
                 .WithOne(x => x.Order)
                 .HasForeignKey(x => x.OrderId);
        }
    }
}
