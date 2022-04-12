using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.EntityLayer
{
    [Table("tblNotes")]
    public class Note:BaseEntity
    {
        [StringLength(50),Required]
        public string Title { get; set; }
        [StringLength(3000),Required]
        public string text { get; set; }
        public bool IsDraft { get; set; }
        public int LikeCount { get; set; }
        public int? CategoryId { get; set; }
        public virtual MyNotesUser Owner { get; set; }

        public virtual Category Category { get; set; }
        public virtual List<Comment> Comments { get; set; }=new List<Comment>();

        public List<Liked> Likes { get; set; } = new List<Liked>();


        //eager loading:Oluşturacağım nesnede alt bağlamlar var ise hepsini getirir

        //Öğrenciler ve sınıflar tablom var bunların arasında foreign key bağlantım var
        //Öğrenciler Listesi alıdğımda eğer eager loading kullanarak alıyorsam
        /*
         *ad
         * soyad
         * sinif
         *          {
         *              sinifadi
         *              kat
         *              eğitmen
         *           {
         * Hepsi gelir 
         *
         *
         *
         *
         */

        //Lazy loading: Oluşturulan nesnede getirilir ilgili alt bağlamlar yüklenmez 
        //Virtual yaptığımız zaman 
    }
}
