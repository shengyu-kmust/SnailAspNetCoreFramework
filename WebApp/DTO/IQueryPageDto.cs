using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.Page;

namespace WebApp.DTO
{
    public interface IQueryPageDto<T>:IQueryDto<T>,IPagination where T:BaseEntity
    {
    }
}
