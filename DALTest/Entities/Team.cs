using CommonAbstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DALTest.Entities
{
   public class Team: IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        #region 导航属性
        public List<Student> Students { get; set; }

        #endregion

    }
}
