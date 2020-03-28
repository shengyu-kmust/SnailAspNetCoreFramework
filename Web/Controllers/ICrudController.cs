using Snail.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
