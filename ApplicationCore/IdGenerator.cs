using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore
{

    public static class IdGenerator 
    {
        public static T GeneratorId<T>()
        {
            if (typeof(T).Equals(typeof(Guid)))
            {
                return (T)(object)Guid.NewGuid();
            }
            else if (typeof(T).Equals(typeof(string)))
            {
                return (T)(object)$"{DateTime.Now.ToString("yyMMddHHmmssfff")}";
            }
            throw new Exception($"无类型{typeof(T)}的id生成逻辑");
        }
    }
}
