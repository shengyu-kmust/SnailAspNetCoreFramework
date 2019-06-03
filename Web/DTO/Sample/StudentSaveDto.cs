using ApplicationCore.Abstract;
using ApplicationCore.Entity;
using ApplicationCore.Enum;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

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
