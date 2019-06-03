using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Abstract
{
    public interface IEntityId<T>:IBaseEntity
    {
        T Id { get; set; }
    }
}
