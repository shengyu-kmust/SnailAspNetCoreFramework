using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAbstract
{
    public interface IEntitySoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
