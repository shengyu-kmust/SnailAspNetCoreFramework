using Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace XUnitTestProject1
{

    public class EFTest
    {
        [Fact]
        public void Test()
        {
           
        }
    }


    #region entity
    /// <summary>
    /// 学生
    /// </summary>
    [Table("Student")]
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public int Sex { get; set; }

        #region One-to-one
        public Card Card { get; set; }
        #endregion

        #region one-to-many
        public int TeamId { get; set; }
        public Team Team { get; set; }
        #endregion

    }

    /// <summary>
    /// 班级
    /// </summary>
    [Table("Team")]
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public ICollection<Student> Students { get; set; }
    }

    /// <summary>
    /// 学生卡
    /// </summary>
    public class Card
    {
        public int Id { get; set; }
        public string CardNo { get; set; }
        public decimal Money { get; set; }

        #region one-to-one
        public int StudentId { get; set; }
        public Student Student { get; set; }

        #endregion

    }

    [Table("Teacher")]
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
    }



    #endregion

    #region DBContext

    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }

    }

    #endregion
}
