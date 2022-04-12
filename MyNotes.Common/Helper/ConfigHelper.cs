using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Common.Helper
{
    public class ConfigHelper
    {
        //public static string Get(string key)
        //{
        //    return ConfigurationManager.AppSettings[key];
        //}
        //Çoğu şey için çalışıyor ama bazı int değerleri için aşağıdaki generic yapıyı kurmak daha mantıklı.
        public static T Get<T>(string key)
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }
       

    }
}
