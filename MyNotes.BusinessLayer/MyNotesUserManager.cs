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
            res.Result = Find(s => s.UserName == data.UserName && s.Password == data.Password && s.IsDelete != true);
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
                    ActivateGuid = Guid.NewGuid(),
                    ProfileImageFileName = "user.png"
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

        public BusinessLayerResult<MyNotesUser> Insert(MyNotesUser data)
        {
            MyNotesUser user = Find(s => s.UserName == data.UserName || s.Email==data.Email);
            res.Result = data;
            if (user != null)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExist, "Bu Kullanıcı Daha Önce Alınmış");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu Email Daha Önce kullanılmış");
                }

            }
            else
            {
                res.Result.ActivateGuid = Guid.NewGuid();
                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı Eklenemedi");

                }
            }
            return res;
        }
        public new BusinessLayerResult<MyNotesUser> Update(MyNotesUser data)
        {
            MyNotesUser db_user = Find(s => s.UserName == data.UserName || s.Email == data.Email);
            res.Result = data;
            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExist, "Bu Kullanıcı Daha Önce Alınmış");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu Email Daha Önce kullanılmış");
                }

            }
            else
            {
                res.Result = Find(s => s.Id == data.Id);
                res.Result.Email = data.Email;
                res.Result.Name = data.Name;
                res.Result.LastName = data.LastName;
                res.Result.Password = data.Password;
                res.Result.UserName = data.UserName;
                res.Result.IsActive = data.IsActive;
                res.Result.IsAdmin = data.IsAdmin;
                if (base.Update(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı Güncellenemedi");
                }
            }
            return res;
        }
        public BusinessLayerResult<MyNotesUser> UpdateProfile(MyNotesUser data)
        {
            MyNotesUser user = Find(s => s.Id != data.Id && (s.UserName == data.UserName || s.Email == data.Email));
            if (user != null && user.Id != data.Id)
            {
                if (user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Bu kullanici adi daha once kaydedilmis.");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu email daha once kaydedilmis.");
                }
                return res;
            }
            res.Result = Find(s => s.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.LastName = data.LastName;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;
            if (!string.IsNullOrEmpty(data.ProfileImageFileName))
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil guncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<MyNotesUser> RemoveUserById(int id)
        {
            res.Result = Find(x => x.Id == id);
            if (res.Result != null)
            {
                
                res.Result.IsDelete = true;
                if (base.Update(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı");
            }
            return res;
        }
        public BusinessLayerResult<MyNotesUser> GetUserById(int id)
        {
            res.Result = Find(x => x.Id == id);
            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanici bulunamadi");

            }

            return res;
        }

    }
}
