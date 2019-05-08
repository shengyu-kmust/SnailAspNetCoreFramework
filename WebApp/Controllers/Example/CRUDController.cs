using DAL.Sample;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Utility.Page;
using WebApp.DTO.Sample;
using WebApp.Services;

namespace WebApp.Controllers.Example
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRUDController : ControllerBase
    {
        private CRUDService<Student> CRUDService;

        /// <summary>
        /// 查询 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<StudentResultDto> Query(StudentQueryDto query)
        {
            CRUDService.Query<StudentResultDto>(query)
        }

        /// <summary>
        /// 分页查询 
        /// </summary>
        /// <returns></returns>
        public PageResult<StudentResultDto> QueryPage(StudentQueryPageDto queryPage)
        {
            return null;
        }
    }


   
}