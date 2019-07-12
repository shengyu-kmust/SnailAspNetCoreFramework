using ApplicationCore.Abstract;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using Utility.Page;

namespace ApplicationCore
{
    public abstract class QueryService<Source> : IQueryService<Source>
    {
        protected IMapper _mapper;
        protected IQueryable<Source> _querySource;

        public QueryService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public abstract void InitQuerySource();

        public PageResult<TResult> QueryPage<TResult, TQueryDto>(TQueryDto queryDto) where TResult : class where TQueryDto : IPagination
        {
            var query = BuildPredicateAndSelector<TResult, TQueryDto>(queryDto);
            queryDto.PageIndex = queryDto.PageIndex == 0 ? 1 : queryDto.PageIndex;
            queryDto.PageSize = queryDto.PageSize == 0 ? 25 : queryDto.PageSize;
            query = (queryDto.PageIndex <= 1) ? query.Take(queryDto.PageSize) : query.Skip(queryDto.PageSize * (queryDto.PageIndex - 1)).Take(queryDto.PageSize);
            var items = query.AsNoTracking().ToList();
            var total = query.AsNoTracking().Count();
            return new PageResult<TResult>
            {
                Items = items,
                PageIndex = queryDto.PageIndex,
                PageSize = queryDto.PageSize,
                Total = total
            };
        }
        public List<TResult> Query<TResult, TQueryDto>(TQueryDto queryDto) where TResult : class
        {
            return BuildPredicateAndSelector<TResult, TQueryDto>(queryDto).ToList();
        }

        private IQueryable<TResult> BuildPredicateAndSelector<TResult, TQueryDto>(TQueryDto queryDto) where TResult : class
        {
            IQueryable<Source> source = _querySource;
            // 查询条件
            if (queryDto is IPredicateConvert<TQueryDto, Source> predicateConvert)
            {
                source = source.Where(predicateConvert.GetExpression());
            }
            else
            {
                source = source.Where(SimpleEntityExpressionGenerator.GenerateAndExpressionFromDto<Source>(queryDto));
            }
            IQueryable<TResult> result;
            // 结果映射
            if (queryDto is ISelectorBuilder<Source, TResult> selectorBuilder)
            {
                result = source.Select(selectorBuilder.GetSelector());
            }
            else
            {
                result = _mapper.ProjectTo<TResult>(source);
            }
            return result.AsNoTracking();
        }
      
    }
}
