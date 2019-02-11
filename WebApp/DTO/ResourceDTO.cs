using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.DTO
{
    public class ResourceDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 资源分为菜单、接口（操作类的按钮本质上也是接口类型）
        /// </summary>
        public string ResourceType { get; set; }
        /// <summary>
        /// 资源地址，可以菜单地址、接口地址
        /// </summary>
        public string ResourceAddress { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentId { get; set; }
    }
}
