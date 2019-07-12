using ApplicationCore.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DALTest.Entities
{
    public class Student:IEntityId<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public int? TeamId { get; set; }
        /// <summary>
        /// 用value-conversions，参考：https://docs.microsoft.com/zh-cn/ef/core/modeling/value-conversions
        /// 枚举示例
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// 参加的社团，多个社团以英文逗号隔开
        /// 数组示例
        /// </summary>
        public List<string> Clubs { get; set; }
        


        #region 导航属性
        /// <summary>
        /// one-one,many-one
        /// </summary>
        public Team Team { get; set; }
        /// <summary>
        /// one-one
        /// </summary>
        public IdentityCard identityCard { get; set; }
        /// <summary>
        /// one-many,one-one
        /// </summary>
        public List<BankCard> BankCards { get; set; }
        #endregion
    }

    /// <summary>
    /// 性别枚举
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// 男
        /// </summary>
        Male,
        /// <summary>
        /// 女
        /// </summary>
        Female
    }
}
