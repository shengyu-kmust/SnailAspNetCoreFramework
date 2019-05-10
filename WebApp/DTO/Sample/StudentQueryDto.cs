using DAL.Sample;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.DTO.Sample
{
    public class StudentQueryDto : IQueryDto<Student>
    {
        public Expression GeneratePredicateExpression()
        {
            throw new NotImplementedException();
        }

        public Func<IQueryable<Student>, IQueryable<Student>> IncludeFunc()
        {
            throw new NotImplementedException();
        }

        public Func<IQueryable<Student>, IQueryable<Student>> OrderFunc()
        {
            throw new NotImplementedException();
        }

        public object Query()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<Student, TResult>> SelectorExpression<TResult>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        Expression<Func<Student, bool>> IQueryDto<Student>.GeneratePredicateExpression()
        {
            throw new NotImplementedException();
        }
    }
}
