using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.BusinessLayer.ValueObject
{
    internal class RegisterViewModel
    {
        [DisplayName("Adı"), Required(ErrorMessage = "{0} alanı boş geçilemez"),StringLength(30,ErrorMessage = "{0} max {1} karakter olmalı")]
        public string Name { get; set; }
        [DisplayName("Soy Adı"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(30, ErrorMessage = "{0} max {1} karakter olmalı")]
        public string Lastname { get; set; }
        [DisplayName("Kullanucu Adı"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(30, ErrorMessage = "{0} max {1} karakter olmalı")]
        public string Username { get; set; }
        [DisplayName("Email"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(100, ErrorMessage = "{0} max {1} karakter olmalı"),EmailAddress(ErrorMessage = "{0} alanı için geçerli bir email adresi giriz")]
        public string Email { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(30, MinimumLength = 3,ErrorMessage = "{0} max {1} min {2} karakter olmalı"), DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(30, MinimumLength = 3, ErrorMessage = "{0} max {1} min {2} karakter olmalı"), DataType(DataType.Password),Compare("Password",ErrorMessage = "{0} ile {1} uyuşmuyor...")]
        public string Repassword { get; set; }
    }
}
