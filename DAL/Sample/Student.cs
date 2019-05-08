using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Sample
{
    public class Student:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public int Sex { get; set; }

    }
}