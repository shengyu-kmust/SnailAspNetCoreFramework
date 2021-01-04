using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Snail.Core.Default;

namespace Infrastructure.EntityTypeConfigurations
{
    public class UserConfiguration : BaseConfiguration, IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            base.Config(builder);
            builder.ToTable("User");
            builder.Property(a => a.Gender).HasConversion<string>().HasMaxLength(50);
        }
    }
}
