using ERP.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Mapping
{
    public class UserMapping: IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.Property(x => x.UserName).HasMaxLength(50);
            builder.Property(x => x.PasswordHashed);
            builder.Property(x => x.PhoneNumber).HasMaxLength(11);
            builder.Property(x => x.ProfilePicture);
            builder.Property(x => x.Active);
        }
    }
}
