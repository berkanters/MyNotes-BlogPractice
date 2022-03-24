using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.EntityLayer
{
    [Table("tblMyNotesUsers")]
    public class MyNotesUser:BaseEntity
    {
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(30)]
        public string LastName { get; set; }
        [StringLength(30),Required]
        public string UserName { get; set; }
        [StringLength(100),Required]
        public string Email { get; set; }
        [StringLength(100),Required]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public Guid ActivateGuid { get; set; }
        public bool IsAdmin { get; set; }
        public virtual IList<Note> Notes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Liked> Likes{ get; set; }

    }
}
