using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Snail.Core.Entity;

namespace Infrastructure.Data.Config
{
    public class BaseConfiguration
    {
        public void Config<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity:class
        {
            if (typeof(IEntityId<string>).IsAssignableFrom(typeof(TEntity)))
            {
                builder.Property("Id").HasColumnType("varchar(50)");
                builder.HasKey("Id");
            }
            if (typeof(IEntitySoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                builder.Property("IsDeleted").HasColumnType("int").HasConversion<int>();
            }
            if (typeof(IEntityAudit<string>).IsAssignableFrom(typeof(TEntity)))
            {
                builder.Property("CreaterId").HasMaxLength(50);
                builder.Property("UpdaterId").HasMaxLength(50);
            }
        }
    }
}
