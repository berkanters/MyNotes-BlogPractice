using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyNotes.EntityLayer;

namespace MyNotes.BusinessLayer.Model
{
    public class CurrentSession
    {
        public static MyNotesUser User
        {
            get
            {
                return Get<MyNotesUser>("Login");
                //if (HttpContext.Current.Session["Login"]!=null)
                //{
                //    return HttpContext.Current.Session["Login"] as MyNotesUser;
                //}


            }
        }

        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key]!=null)
            {
                return (T)HttpContext.Current.Session[key];
            }

            return default(T);
        }

        public static void Set<T>(string key, T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }

        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key]!=null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}
