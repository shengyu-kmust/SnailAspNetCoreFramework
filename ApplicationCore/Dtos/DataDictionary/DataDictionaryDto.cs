using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Dtos.DataDictionary
{
    public class DataDictionaryDto : IDto, IIdField<string>
    {
        public string Id { get; set; }
    }
}
