using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Mapping
{
    public class ProductMapping: IEntityTypeConfiguration<ProductModel>
    {
        public void Configure(EntityTypeBuilder<ProductModel> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ProductCategoryId);
            builder.Property(x => x.ProductCode);
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.Description);
            builder.Property(x => x.SellPrice);
            builder.Property(x => x.CostPrice);
            builder.Property(x => x.StockQuantity);
            builder.Property(x => x.CreationTime);

            builder.HasMany(x => x.ProductItems)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.ProductCateory)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ProductCategoryId);

            builder.HasMany(x => x.FinancialTransactions)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);
        }
    }
}
