using Snail.Core;
using System.Collections.Generic;

namespace Web.Controllers
{
    public interface ICrudController<TEntity, TSaveDto, TResultDto,TQueryDto>
    {
        List<TResultDto> QueryList(TQueryDto queryDto);
        IPageResult<TResultDto> QueryPage(TQueryDto queryDto);
        TResultDto Find(string id);
        void Save(TSaveDto saveDto);
        void Remove(List<string> ids);
    }
}
