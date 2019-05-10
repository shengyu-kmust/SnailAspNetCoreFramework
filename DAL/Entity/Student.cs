using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entity
{
    [Table("Student")]
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        #region One-to-one
        public Card Card { get; set; }
        #endregion

        #region one-to-many
        public int TeamId { get; set; }
        public Team Team { get; set; }
        #endregion

    }

}
