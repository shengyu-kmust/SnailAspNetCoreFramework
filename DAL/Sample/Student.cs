using CommonAbstract;
using System;

namespace DAL.Sample
{
    public class Student:IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public int Sex { get; set; }

    }
}