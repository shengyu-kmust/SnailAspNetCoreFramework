using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost("M1")]
        public ActionResult M1(Model model)
        {
            return Ok(model);

        }


        [HttpPost("M2")]
        public ActionResult M2([FromForm]string Name,[FromForm]string Pwd)
        {
            return Ok($"Name={Name},Pwd={Pwd}");
        }
    }

    public class Model
    {
        public string Name { get; set; }
        public string Pwd { get; set; }
    }
}