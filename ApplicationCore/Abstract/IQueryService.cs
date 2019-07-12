using System;
using System.Collections.Generic;
using System.Text;
using Utility.Page;

namespace ApplicationCore.Abstract
{
    public interface IQueryService<Source>
    {
        void InitQuerySource();
        PageResult<TResult> QueryPage<TResult, TQueryDto>(TQueryDto queryDto) where TResult : class where TQueryDto : IPagination;
        List<TResult> Query<TResult, TQueryDto>(TQueryDto queryDto) where TResult : class;

    }
}
