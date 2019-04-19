using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.DTO
{
    /// <summary>
    /// 增加、修改的传入对象继承此接口
    /// </summary>
    public interface ICommandDto:IValidatableObject
    {
    }
}
