using EasyCaching.Core;
using EasyCaching.InMemory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ApplicationCoreTest
{
    public class EasyCacheTest
    {
        [Fact]
        public void Test()
        {
            try
            {
                var sc = new ServiceCollection();
                sc.AddEasyCaching(option =>
                {
                // use memory cache with a simple way
                option.UseInMemory("default");
                });
                var sp = sc.BuildServiceProvider();
                var provider = sp.GetService<IEasyCachingProvider>();
                provider.Set("1", 1, new TimeSpan(1, 1, 1));
                provider.Set("1", 2, new TimeSpan(1, 1, 1));
                var a=provider.TrySet("2", 1, new TimeSpan(1, 1, 1));
               var b= provider.TrySet("2", 2, new TimeSpan(1, 1, 1));

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }
    }
}
