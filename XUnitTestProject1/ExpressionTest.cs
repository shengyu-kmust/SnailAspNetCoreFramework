using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace XUnitTestProject1
{
    public class ExpressionTest
    {
        [Fact]
        public void Test()
        {
            var para = Expression.Parameter(typeof(Student), "a");
            var para2= Expression.Parameter(typeof(Student), "a");
            var expression1 =Expression.Lambda(Expression.Equal(Expression.Property(para,"Name"),Expression.Constant("")),para);
            var expression2 = Expression.Lambda(Expression.Equal(Expression.Property(para2, "Name"), Expression.Constant("")), para2);
            var body = Expression.AndAlso(expression1.Body, expression2.Body);
            var lamb=Expression.Lambda(body, expression1.Parameters);
            var func = lamb.Compile();
            var result=func.DynamicInvoke(new Student() { Name = "2" });
        }
    }
   
}
