using ApplicationCore.IServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Test
{
    public class ConfigServiceTest:IntegrateTestBase
    {
        private readonly IConfigService configService;
        public ConfigServiceTest()
        {
            configService = serviceProvider.GetService<IConfigService>();
        }

        [Fact]
        public void Test()
        {
            try
            {
                var result = configService.QueryList(a => true && a.Name.Contains("配置")).ToList();
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }
        [Fact]
        public void Test1()
        {
            var a = 1;
        }
    }
}
