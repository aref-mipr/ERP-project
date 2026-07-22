using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Mapping
{
    public class ProductCategoryMapping: IEntityTypeConfiguration<ProductCategoryModel>
    {
        public void Configure(EntityTypeBuilder<ProductCategoryModel> builder)
        {
            builder.ToTable("ProductCategories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ProductCategoryCode);
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.IsActive);
            builder.Property(x => x.CreationTime);

            builder.HasMany(x => x.Products)
                .WithOne(x => x.ProductCateory)
                .HasForeignKey(x => x.ProductCategoryId);
        }
    }
}
