using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Dtos
{
    public class BaseIdDto : IIdField<string>, IDto
    {
        public string Id { get; set; }
    }
}
