using System;
using System.Collections.Generic;
using System.Text;

namespace DALTest.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public int TeamId { get; set; }

        #region 导航属性
        /// <summary>
        /// one-one,many-one
        /// </summary>
        public Team Team { get; set; }
        /// <summary>
        /// one-one
        /// </summary>
        public IdentityCard identityCard { get; set; }
        /// <summary>
        /// one-many,one-one
        /// </summary>
        public List<BankCard> BankCards { get; set; }
        #endregion
    }
}
