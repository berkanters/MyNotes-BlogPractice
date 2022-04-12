using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyNotes.BusinessLayer.Model;
using MyNotes.Common;

namespace MyNotes.MVC.Init
{
    public class WebCommon:ICommon
    {
        public string GetCurrentUserName()
        {
            if (CurrentSession.User!=null)
            {
                var user = CurrentSession.User;
                return user.UserName;
            }
            return "system";
        }
    }
}