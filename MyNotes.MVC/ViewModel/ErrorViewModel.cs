using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyNotes.EntityLayer.Messages;

namespace MyNotes.MVC.ViewModel
{
    public class ErrorViewModel : NotifyViewModelBase<ErrorMessageObj>
    {
        public ErrorViewModel()
        {
            Title = "An error occurred.";
        }
    }
}