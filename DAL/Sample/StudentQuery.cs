using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DAL.Sample
{
    public class StudentQuery:BaseQuery<Student>
    {
        public string Name { get; set; }
        public int? Sex { get; set; }
        public override Expression<Func<Student, bool>> GeneratePredicate()
        {
            //Expression<Func<Student,bool> predicate = new Expression<Func<Student, bool>>();
            return null;
        }
    }
}
