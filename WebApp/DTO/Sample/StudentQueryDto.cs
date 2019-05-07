using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApp.DTO.Sample
{
    public class StudentQueryDto : IQueryDto
    {
        public Expression GeneratePredicateExpression()
        {
            throw new NotImplementedException();
        }

        public object Query()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
