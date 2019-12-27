using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure
{
    /// <summary>
    /// 参考：https://docs.microsoft.acom/zh-cn/ef/core/miscellaneous/cli/dbcontext-creation
    /// 作用：用于配置Migration，当用Add-Migration命令时，会基于此类生成dbContext去对数据库进行操作
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UniversalQueueContext>
    {
        public UniversalQueueContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UniversalQueueContext>();
            optionsBuilder.UseMySql("Server=localhost;Port=3306;Database=UniversalQueue3.0;User Id=root;Password = root;");
            return new UniversalQueueContext(optionsBuilder.Options);
        }
    }
}
