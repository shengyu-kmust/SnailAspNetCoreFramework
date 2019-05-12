using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAbstract
{
    public interface IEntityId<T>
    {
        T Id { get; set; }
    }
}
