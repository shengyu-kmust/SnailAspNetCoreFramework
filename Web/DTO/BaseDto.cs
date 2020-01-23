using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTO
{
    public class BaseDto : IDto, IIdField<string>
    {
        public string Id { get; set; }
    }
}
