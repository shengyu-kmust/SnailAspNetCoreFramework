using ApplicationCore.Enums;
using Snail.Core.Dto;
using Snail.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Snail.Core.Enum;

namespace ApplicationCore.Dtos
{
    public class UserSourceDto:DefaultBaseDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(50)]
        public string Name  {get;set;}
        /// <summary>
        /// 登录账号
        /// </summary>
        [MaxLength(50)]
        public string LoginName  {get;set;}
        /// <summary>
        /// 出生日期
        /// </summary>
        [MaxLength(50)]
        public DateTime? BirthDate  {get;set;}
        /// <summary>
        /// 性别
        /// </summary>
        [MaxLength(50)]
        public EGender Gender  {get;set;}
        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(50)]
        public string Pwd  {get;set;}
        /// <summary>
        /// 职务
        /// </summary>
        [MaxLength(50)]
        public string Position  {get;set;}
        /// <summary>
        /// 职称
        /// </summary>
        [MaxLength(50)]
        public string Title  {get;set;}
        /// <summary>
        /// 头像
        /// </summary>
        [MaxLength(50)]
        public string AvatarUrl  {get;set;}
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(50)]
        public string Des  {get;set;}
        /// <summary>
        /// 用户类型
        /// </summary>
        [MaxLength(50)]
        public EUserType UserType  {get;set;}
        /// <summary>
        /// 所属科室
        /// </summary>
        public  List<string> TargetIds  {get;set;}
    }
}
