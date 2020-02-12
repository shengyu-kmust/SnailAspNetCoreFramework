using ApplicationCore.Entity;
using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class DataDictionary:BaseEntity
    {
        public string ParentId { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Des { get; set; }
        public EDataDictionaryOperType OperType { get; set; }
        public int Rank { get; set; }
    }

   
}
