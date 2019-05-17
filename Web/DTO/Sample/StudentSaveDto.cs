using ApplicationCore.Enum;
using CommonAbstract;
using DAL.Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.DTO.Sample
{
    public class StudentSaveDto : ISaveDto<Student>
    {
      
        [BindRequired]
        public int? Id { get; set; }
        [BindNever]
        public string Name { get; set; }
        public DateTime? BirthDay { get; set; }
        public Gender? Gender { get; set; }

        public Student ConvertToEntity()
        {
            return new Student
            {
                Id = this.Id??0,
                Name = this.Name,
                BirthDay = this.BirthDay,
                Gender = this.Gender
            };
        }

        public List<string> GetUpdateProperties()
        {
            return new List<string> { nameof(Gender) };
        }
    }
}
