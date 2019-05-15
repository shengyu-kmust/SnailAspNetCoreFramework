using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAbstract
{
    public interface IEntityId<T>:IBaseEntity
    {
        T Id { get; set; }
    }
}
