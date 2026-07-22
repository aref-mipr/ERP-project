using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Mapping
{
    public class ProductItemMapping: IEntityTypeConfiguration<ProductItemModel>
    {
        public void Configure(EntityTypeBuilder<ProductItemModel> builder)
        {
            builder.ToTable("ProductItems");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ProductId);
            builder.Property(x => x.ProductItemCode);
            builder.Property(x => x.Description);
            builder.Property(x => x.ProductItemStatus);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.ProductItems)
                .HasForeignKey(x => x.ProductId);

            builder.HasMany(x => x.FinancialTransactions)
                .WithOne(x => x.ProductItem)
                .HasForeignKey(x => x.ProductItemId);

            builder.HasMany(x => x.OrderItems)
                .WithOne(x => x.ProductItem)
                .HasForeignKey(x => x.ProductItemId);
        }
    }
}
