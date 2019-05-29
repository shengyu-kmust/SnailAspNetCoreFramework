using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    public class TestController : AuthorizeBaseController
    {
        public TestController(DatabaseContext db) : base(db)
        {
        }

        [HttpPost("M1")]
        public ActionResult M1(Model model)
        {
            return Ok(model);

        }
        [AllowAnonymous]
        [Authorize(Policy = ConstValues.PermissionPolicy)]
        [HttpPost("M2")]
        public ActionResult M2([FromForm]string Name,[FromForm]string Pwd)
        {
            return Ok($"Name={Name},Pwd={Pwd}");
        }

        [HttpGet("M3")]
        public ActionResult M3()
        {
            return Ok("success M3");
        }

        [Authorize(Policy = ConstValues.PermissionPolicy)]
        [HttpGet("M4")]
        public ActionResult M4()
        {
            return Ok("success M4");
        }
    }

    public class Model
    {
        public string Name { get; set; }
        public string Pwd { get; set; }
    }
}