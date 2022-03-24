using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.EntityLayer
{
    [Table("tblComments")]
    public class Comment:BaseEntity
    {
        [StringLength(300),Required]
        public string Text { get; set; }

        public virtual Note Note { get; set; }
    }
}
