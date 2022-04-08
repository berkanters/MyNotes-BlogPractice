using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MyNotes.BusinessLayer.Abstract;
using MyNotes.BusinessLayer.ValueObject;
using MyNotes.Common.Helper;
using MyNotes.EntityLayer;
using MyNotes.EntityLayer.Messages;


namespace MyNotes.BusinessLayer
{
    public class MyNotesUserManager:ManagerBase<MyNotesUser>
    {
        private BusinessLayerResult<MyNotesUser> res = new BusinessLayerResult<MyNotesUser>();

        public BusinessLayerResult<MyNotesUser> LoginUser(LoginViewModel data)
        {
            res.Result = Find(s => s.UserName == data.UserName && s.Password == data.Password);
            if (res.Result!=null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive,"Kullanıcı Adı Aktifleştirilmemiş");
                    res.AddError(ErrorMessageCode.CheckYourEmail,"Mail Adresinizi kontrol edin");
                }
               
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı yada şifreniz yanlış lütfen kontrol edin");
            }
            return res;
        }

        public BusinessLayerResult<MyNotesUser> RegisterUser(RegisterViewModel data)
        {
            MyNotesUser user = Find(s => s.UserName == data.Username || s.Email == data.Email);
            if (user!=null)
            {
                if (user.UserName==data.Username)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExist,"Bu Kullanıcı Daha Önce Alınmış");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu Email Daha Önce kullanılmış");
                }

            }
            else
            {
                int dbResult = base.Insert(new MyNotesUser()
                {
                    Email = data.Email,
                    Name = data.Name,
                    LastName = data.Lastname,
                    Password = data.Password,
                    UserName = data.Username,
                    IsActive = false,
                    IsAdmin = false,
                    ActivateGuid = Guid.NewGuid()
                });
                if (dbResult>0)
                {
                    res.Result = Find(s => s.Email == data.Email && s.UserName == data.Username);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Name} {res.Result.LastName} <br/> Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>";
                    MailHelper.SendMail(body, res.Result.Email, "MyNotes Hesap Aktifleştirme");

                }
            }
            return res;
        }

        public BusinessLayerResult<MyNotesUser> ActivateUser(Guid id)
        {
            res.Result = Find(s => s.ActivateGuid == id);
            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif");
                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Aktifleştirilecek kullanıcı bulunamadı");
            }
            return res;
        }
    }
}
