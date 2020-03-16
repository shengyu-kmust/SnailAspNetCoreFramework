using ApplicationCore.Enum;
using Snail.Common;
using Snail.Core;
using Snail.Core.Interface;
using System;
using System.Linq.Expressions;

namespace Web.DTO.Sample
{
    public class SampleEntityQueryDto : BaseDto, IPagination, IPredicateBuilder<SampleEntitySourceDto>
    {
        public string Name { get; set; }
        public EGender Gender { get; set; }
        public int Age { get; set; }
        public int PageSize { get;set; }
        public int PageIndex { get;set; }

        public Expression<Func<SampleEntitySourceDto, bool>> BuildPredicate()
        {
            Expression<Func<SampleEntitySourceDto, bool>> expression = a => true;
            expression = expression.AndIf(!string.IsNullOrEmpty(Id), a => a.Id == Id).AndIf(!string.IsNullOrEmpty(Name), a => a.Name.Contains(Name));
            return expression;
        }
    }
}
