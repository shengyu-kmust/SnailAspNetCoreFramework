using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Entity
{
    /// <summary>
    /// 学生卡
    /// </summary>
    [Table("Card")]
    public class Card
    {
        public int Id { get; set; }
        public string CardNo { get; set; }

        #region one-to-one
        public int StudentId { get; set; }
        public Student Student { get; set; }

        #endregion

    }
}
