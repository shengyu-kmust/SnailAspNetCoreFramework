using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Dtos
{
    public class ResourceSaveDto: BaseDto
    {
        /// <summary>
        /// 资源键，如接口名，菜单名，唯一键
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 资源描述，如接口的名称、菜单的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父资源id
        /// </summary>
        public string ParentId { get; set; }
    }
}
