using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MyNotes.BusinessLayer.Abstract;
using MyNotes.BusinessLayer.ValueObject;
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
    }
}
