using System.Collections.Generic;

namespace Web.CodeGenerater
{
    /// <summary>
    /// 代码生成配置文件
    /// </summary>
    public class CodeGenerateConfig
    {
        public string BasePath { get; set; }
        public List<EntityConfigModel> Entities { get; set; }
    }

    /// <summary>
    /// 配置里的entity节点
    /// </summary>
    public class EntityConfigModel
    {
        public string Name { get; set; }
        public string TableName { get; set; }
        public List<string> Columns { get; set; }
    }

    #region 用于在template.tt里使用的model
    #region 生成entity的model
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
    #endregion
    #region 生成dto的model

    public class DtoModel
    {
        public List<EntityFieldModel> Fields { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public string BaseClass { get; set; }
    }
    
    #endregion

    #endregion


}
