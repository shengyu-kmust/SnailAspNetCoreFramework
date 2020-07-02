
using ApplicationCore.Dtos;
using System;
using System.ComponentModel.DataAnnotations;
namespace ApplicationCore.Entity
{
    public class DemoSaveDto:BaseIdDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(50)]
        public string Name  {get;set;}
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender  {get;set;}
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age  {get;set;}
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Birthday  {get;set;}
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enabled  {get;set;}
    }
}
