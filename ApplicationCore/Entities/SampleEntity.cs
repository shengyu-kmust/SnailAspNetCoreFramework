using ApplicationCore.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Entity
{
    public class SampleEntity:BaseEntity
    {
        public EGender Gender { get; set; }
    }
}
