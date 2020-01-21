using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTO.Sample
{
    public class SampleSaveDto : IIdField<string>
    {
        public string Id { get; set; }
    }
}
