﻿using Snail.Core.Enum;

namespace Web.DTO
{
    public class UserResultDto:BaseDto
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public EGender Gender { get; set; }
    }
}