using ApplicationCore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
namespace Infrastructure.Data.Config
{
    public class UserConfiguration : BaseConfiguration,IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            base.Config(builder);
            builder.ToTable("User");
            builder.Property(a => a.Gender).HasConversion<string>();
        }
    }
}
