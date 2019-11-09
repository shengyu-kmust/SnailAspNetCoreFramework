using System;
using System.Data;
using Xunit;

namespace ApplicationCoreTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            try
            {
                var dt = GetDataTable();
            }
            catch (Exception ex)
            {
                var es = ex;
            }
        }

        private DataTable GetDataTable()
        {
            using (var dt = new DataTable())
            {
                dt.TableName = "test";
                dt.Columns.Add("c1", typeof(string));
                dt.Columns.Add("c2", typeof(string));
                dt.Rows.Add("1", "1");
                dt.Rows.Add("2", "2");
                return dt;
            }
        }
    }
}
