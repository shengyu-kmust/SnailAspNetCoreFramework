using Microsoft.AspNetCore.Mvc;
using Snail.Core;

namespace Web.Controllers.Example
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CRUDController : ControllerBase
    {
        //private CRUDService<Student> CRUDService;

        /// <summary>
        /// 查询 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        //public List<StudentResultDto> Query(StudentQueryDto query)
        //{
        //    //return CRUDService.Query(query);
        //}

        /// <summary>
        /// 分页查询 
        /// </summary>
        /// <returns></returns>
        //public PageResult<StudentResultDto> QueryPage(StudentQueryPageDto queryPage)
        //{
        //    return null;
        //}

        //[HttpPost]
        //public object Update(StudentSaveDto saveDto)
        //{
        //    return saveDto;
        //}
    }


   
}