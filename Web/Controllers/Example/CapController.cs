using System;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Example
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CapController: ControllerBase
    {

        [HttpPost]
        public void Publish(string msg,[FromServices] ICapPublisher capPublisher)
        {
            capPublisher.Publish("controller_event_test", msg);
            capPublisher.Publish("service_event_test", msg);

        }

        [CapSubscribe("controller_event_test")]
        [HttpGet]
        public string Recieve(string msg)
        {
            Console.WriteLine($"controller_event_test:{msg}");
            return msg;
        }
    }
}
