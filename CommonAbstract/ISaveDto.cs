using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAbstract
{
    public interface ISaveDto<T> where T:IBaseEntity
    {
        T ConvertToEntity();
        List<string> GetUpdateProperties();
    }
}
