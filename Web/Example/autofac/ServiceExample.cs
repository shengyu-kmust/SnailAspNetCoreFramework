using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Example.autofac
{
    public class ServiceExample
    {
    }

    public interface IAService
    {
        string GetServiceName();
    }
    public interface IBService
    {
        string GetServiceName();
        string GetAServiceName();
    }
    public class AService : IAService
    {
        public string GetServiceName()
        {
            return nameof(AService);
        }
    }

    public class BService : IBService
    {
        public IAService aService  { get; set; }
        public string GetServiceName()
        {
            return nameof(BService);
        }

        public string GetAServiceName()
        {
            return $"{GetServiceName()}.{aService.GetServiceName()}";
        }

    }
}
