using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyNotes.BusinessLayer;

namespace MyNotes.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryManager cm = new CategoryManager();
        // GET: Category
        public ActionResult Index()
        {
            return View(cm.List());
        }
    }
}