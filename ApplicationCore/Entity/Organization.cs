using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Entity
{
    public class Organization:BaseEntity
    {
        public string Name { get; set; }
        public int ParentId { get; set; }
    }
}
