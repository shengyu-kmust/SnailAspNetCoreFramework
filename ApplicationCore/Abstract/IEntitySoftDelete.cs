using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Abstract
{
    public interface IEntitySoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
