using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Abstract
{
    public interface ISaveDto<T> where T:IBaseEntity
    {
        T ConvertToEntity();
        List<string> GetUpdateProperties();
    }
}
