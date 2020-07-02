using ApplicationCore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
namespace Infrastructure.Data.Config
{
    public class DemoConfiguration : BaseConfiguration,IEntityTypeConfiguration<Demo>
    {
        public void Configure(EntityTypeBuilder<Demo> builder)
        {
            base.Config(builder);
            builder.ToTable("Demo");
        }
    }
}
