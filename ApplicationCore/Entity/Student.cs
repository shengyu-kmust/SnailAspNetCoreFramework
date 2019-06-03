using ApplicationCore.Abstract;
using ApplicationCore.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// 1、请不要删除此实体，对于EF的用法，会用此实体做示例及用法说明，在开发正式应用时才删除
/// 2、举例如何用EF的convention模式（推荐）
/// </summary>
namespace ApplicationCore.Entity
{
    [Table("Student")]
    public class Student:IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDay { get; set; }
        [Column(TypeName ="nvarchar(20)")]
        public Gender? Gender { get; set; }


        #region One-to-one
        /// <summary>
        /// 如果include时，会是left join 操作
        /// </summary>
        public Card Card { get; set; }
        #endregion

        #region one-to-many
        /// <summary>
        /// 如果include时，int会是inner join操作,int?时会是left join操作
        /// </summary>
        public int TeamId { get; set; }
        public Team Team { get; set; }
        #endregion
    }

}
