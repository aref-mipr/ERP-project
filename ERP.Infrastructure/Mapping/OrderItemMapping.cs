using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Mapping
{
    public class OrderItemMapping: IEntityTypeConfiguration<OrderItemModel>
    {
        public void Configure(EntityTypeBuilder<OrderItemModel> builder)
        {
            builder.ToTable("OrderItems");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OrderId);
            builder.Property(x => x.ProductItemId);
            builder.Property(x => x.Price);
            builder.Property(x => x.Returned);
            builder.Property(x => x.CreationTime);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ProductItem)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.ProductItemId);

            builder.HasMany(x => x.FinancialTransactions)
                .WithOne(x => x.OrderItem)
                .HasForeignKey(x => x.OrderItemId);
        }
    }
}
