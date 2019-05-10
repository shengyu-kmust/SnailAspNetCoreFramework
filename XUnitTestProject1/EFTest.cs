using DinkToPdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq.Expressions;

namespace XUnitTestProject1
{
    
    public class EFTest
    {
        [Fact]
        public void Test()
        {
            var options= new DbContextOptionsBuilder<MyDbContext>().UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test;Trusted_Connection=True;").Options;
            using (MyDbContext db=new MyDbContext(options))
            {
                var result=db.Students.ToList();
                Expression<Func<Student, bool>> expression1 = a => a.Card.CardNo == "";
                Expression<Func<Student, bool>> expression2 = a => a.Card.CardNo == "";
                var body= Expression.AndAlso(expression1, expression2);
                Expression.Lambda(body, Expression.Parameter(typeof(Student)));
                //db.Students.Include(a=>a.BirthDay).OrderBy()
                //db.Students.Where()
            }
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
