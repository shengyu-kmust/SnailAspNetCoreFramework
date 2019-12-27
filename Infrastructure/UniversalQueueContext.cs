using ApplicationCore;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure
{
    public partial class UniversalQueueContext : DbContext
    {
        //public UniversalQueueContext()
        //{
        //}

        public UniversalQueueContext(DbContextOptions<UniversalQueueContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Called> Called { get; set; }
        public virtual DbSet<Config> Config { get; set; }
        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<Queue> Queue { get; set; }
        public virtual DbSet<Target> Target { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<ToCall> ToCall { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<DailyNumber> DailyNumber { get; set; }
        public virtual DbSet<DailyNumberConfig> DailyNumberConfig { get; set; }
        public virtual DbSet<QueueBooking> QueueBooking { get; set; }
        public virtual DbSet<TargetUserRelations> TargetUserRelations { get; set; }
        public virtual DbSet<ConsumerThirdPartyInfo> ConsumerThirdPartyInfo { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 自动应用所有的IEntityTypeConfiguration配置
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
