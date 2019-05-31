using CommonAbstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebTest
{
    public class Test
    {
        public void Method()
        {
            IListDataCaching<string, string> a = new ListDataCaching<string, string>(() => new Dictionary<string, string>(), i => "");
            IListDataCaching<string, string> b = new ListDataCaching<string, string>();
        }
    }
}
