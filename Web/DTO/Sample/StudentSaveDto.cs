using ApplicationCore.Enum;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.DTO.Sample
{
    public class StudentSaveDto
    {
        [BindRequired]
        public int? Id { get; set; }
        [BindNever]
        public string Name { get; set; }
        public DateTime? BirthDay { get; set; }
        public Gender? Gender { get; set; }

    }
}
