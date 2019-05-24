using CommonAbstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DALTest.Entities
{
    public class IdentityCard: IBaseEntity
    {
        public int Id { get; set; }
        public string CardNo { get; set; }
        public int StudentId { get; set; }
        #region 导航属性
        public Student Student { get; set; }

        #endregion
    }
}
