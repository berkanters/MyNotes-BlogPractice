using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.BusinessLayer.ValueObject
{
    public class LoginViewModel
    {
        [DisplayName("Kullanıcı Adı"),Required(ErrorMessage = "{0} alanı boş geçemezsiniz..."),StringLength(30,ErrorMessage = "{0} max {1} karakter olmaı...")]
        public string UserName { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçemezsiniz..."), StringLength(30, ErrorMessage = "{0} max {1} karakter olmaı..."),DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
