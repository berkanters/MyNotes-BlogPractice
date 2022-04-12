using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyNotes.MVC.ViewModel
{
    public class OKViewModel:NotifyViewModelBase<string>
    {
        public OKViewModel()
        {
            Title = "İşlem Başarılı";
        }
    }
}