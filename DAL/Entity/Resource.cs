namespace DAL.Entity
{
    public class Resource:BaseEntity
    {
        /// <summary>
        /// 资源键，如接口名，菜单名
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 资源值，如url地址
        /// </summary>
        public string Value { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int ParentId { get; set; }
    }
}
