using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class BusinessException:Exception
    {
        public BusinessException(string errorMsg):base(errorMsg)
        {

        }
    }
}
