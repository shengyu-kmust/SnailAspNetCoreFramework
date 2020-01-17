using ApplicationCore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
namespace Infrastructure.Data.Config
{
    public class SampleEntityConfiguration : BaseConfiguration,IEntityTypeConfiguration<SampleEntity>
    {
        public void Configure(EntityTypeBuilder<SampleEntity> builder)
        {
            base.Config(builder);

            builder.ToTable("SampleEntity");
            builder.Property(a => a.Gender).HasConversion<int>();
            
        }
    }
}
