using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTO.Config
{
    public class ConfigTreeResultDto:BaseDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string ExtraInfo { get; set; }
        /// <summary>
        /// 子资源
        /// </summary>
        public List<ConfigTreeResultDto> Children { get; set; }
        /// <summary>
        /// 是否有子元素
        /// </summary>
        public bool HasChildren { get { return Children != null && Children.Count > 0; } }
    }
}
