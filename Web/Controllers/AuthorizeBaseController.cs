using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = ConstValues.PermissionPolicy)]
    [Route("api/[controller]/[action]")]
    public class AuthorizeBaseController : ControllerBase
    {
        public DatabaseContext _db;

        public AuthorizeBaseController(DatabaseContext db)
        {
            _db = db;
        }
    }
}