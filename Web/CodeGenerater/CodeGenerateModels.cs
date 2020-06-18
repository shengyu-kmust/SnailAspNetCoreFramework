using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrPMS.Web.CodeGenerater
{
    public class CodeGenerateConfig
    {
        public List<EntityConfigModel> Entities { get; set; }
    }
    public class EntityConfigModel
    {
        public string Name { get; set; }
        public string TableName { get; set; }
        public List<string> Columns { get; set; }
    }

    public class EntityModel
    {
        public List<EntityFieldModel> Fields { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
    }
    public class EntityFieldModel
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 特性
        /// </summary>
        public List<string> Attributes { get; set; }
    }

}
