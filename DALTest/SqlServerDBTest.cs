using DALTest.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace DALTest
{
    public class SqlServerDBTest
    {
        private TestDatabaseDbContext _db;
        public SqlServerDBTest()
        {
            _db = DatabaseDbContextHelper.GetSqlServerDbContext();
            Console.WriteLine("this is Console");
            Debug.WriteLine("this is debug");
        }

        [Fact]
        public void Test()
        {
            try
            {
                Func<Student, ResultDto> func = SqlServerDBTest.Convert;
                var result1 = _db.Students.AsNoTracking().Select(func).ToList();
                var result2 = _db.Students.AsNoTracking().Select(a => new ResultDto
                {
                    Id = a.Id,
                    Info = $"{a.Id}-{a.Name}-{a.BirthDay}-{a.Team.Name}",
                    Name = a.Name
                }).ToList();
            }
            catch (Exception ex)
            {
            }
        }

        public static ResultDto Convert(Student student)
        {
            return new ResultDto
            {
                Id = student.Id,
                Info = $"{student.Id}-{student.Name}-{student.BirthDay}-{student.Team.Name}",
                Name = student.Name
            };
        }
        public class ResultDto
        {
            public int Id { get; set; }
            public string Info { get; set; }
            public string Name { get; set; }
        }
    }
}
