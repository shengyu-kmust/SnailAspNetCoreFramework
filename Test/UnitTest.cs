using Snail.Common.Extenssions;
using System;
using Xunit;

namespace Test
{
    public class UnitTest
    {
        [Fact]
        public void Test()
        {
            try
            {
                "123".ConvertTo<int>();
                "123.33".ConvertTo<double>();
                "123.33".ConvertTo<decimal>();
                "2012-01-01".ConvertTo<DateTime>();
                "Monday".ConvertTo<DayOfWeek>();
                123.ConvertTo<string>();
                DayOfWeek.Monday.ConvertTo<string>();
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }
    }
}
