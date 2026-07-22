using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Mapping
{
    public class BudgetMapping: IEntityTypeConfiguration<BudgetModel>
    {
        public void Configure(EntityTypeBuilder<BudgetModel> builder)
        {
            builder.ToTable("Budgets");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TotalBudget);
            builder.Property(x => x.LastUpdate);
        }
    }
}
