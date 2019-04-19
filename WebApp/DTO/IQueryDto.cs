using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApp.DTO
{
    /// <summary>
    /// 所有的查询条件
    /// </summary>
    public interface IQueryDto:IValidatableObject
    {
        Exception GeneratePredicateExpression();
        object Query();
    }
}
