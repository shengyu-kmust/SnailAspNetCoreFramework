using ApplicationCore.Enum;
using CommonAbstract;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using Utility;

namespace Web.DTO.Sample
{
    public class StudentQueryDto : IQueryDto<Student>
    {
        [QueryFilterType(nameof(Name),BinaryExpressionFilterType.Contain)]
        public string Name { get; set; }

        public Expression<Func<Student, bool>> GeneratePredicateExpression()
        {
            #region 第一种方法，适合比较复杂的查询条件，要自己定义
            //Expression<Func<Student, bool>> expression = a => 1 == 1;
            //expression = expression.AndIf(!string.IsNullOrEmpty(Name), a => a.Name.Contains(Name));
            //return expression;
            #endregion
            #region 第二种方法，适合比较简单的查询条件
            //return SimpleEntityExpressionGenerator.GenerateAndExpressionFromDto<Student>(this); 
            #endregion
            return a => a.Gender == Gender.Female;
        }

        public Func<IQueryable<Student>, IQueryable<Student>> IncludeFunc()
        {
            return a => a.Include(i => i.Card).Include(i => i.Team);
        }

        public Func<IQueryable<Student>, IQueryable<Student>> OrderFunc()
        {
            return a => a.OrderBy(i => i.Id);
        }
    }
}
