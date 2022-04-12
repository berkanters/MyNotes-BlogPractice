using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Design;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Model;
using MyNotes.BusinessLayer.ValueObject;
using MyNotes.EntityLayer;

namespace MyNotes.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryManager cm = new CategoryManager();
        // GET: Category
        public ActionResult Index()
        {
            var cat = cm.IndexCat();
            return View(cat);

            //return View(cm.List());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            //var cat = cm.Find(s => s.Id == id);
            CategoryViewModel cmv = cm.FindCat(id);
            if (cmv==null)
            {
                return HttpNotFound();
            }
            //CategoryViewModel cmv = new CategoryViewModel();
            //cmv.Category.Id = cat.Id;
            //cmv.Category.Title = cat.Title;
            //cmv.Category.Description = cat.Description;
            //cmv.Category.ModifiedUserName = cat.ModifiedUserName;
            //cmv.Category.CreatedOn = cat.CreatedOn;
            //cmv.Category.ModifiedOn = cat.ModifiedOn;
            return View(cmv);
            
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CategoryViewModel cat)
        {
            ModelState.Remove("Category.CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                cm.InsertCat(cat);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }
            return View(cat);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CategoryViewModel category = cm.GetEditCat(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel cat)
        {
            ModelState.Remove("Category.CreatedOn");
            ModelState.Remove("Category.ModifiedOn");
            ModelState.Remove("Category.ModifiedUserName");
            if (ModelState.IsValid)
            {
                //CategoryViewModel category = cm.GetEditCat(cat.Category.Id);
                //category.Category.Title = cat.Category.Title;
                //category.Category.Description = cat.Category.Description;
                //CategoryViewModel categoryViewMo = cm.UpdateCat(category);
                //cm.Update(category);
                //cm.UpdateCat(cat);
                //CacheHelper.RemoveCategoriesFromCache();
                //return RedirectToAction("Index");
                cm.UpdateCat(cat);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }
            return View(cat);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CategoryViewModel category = cm.GetEditCat(id);
            if (category==null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            cm.DeleteCat(id);
            CacheHelper.RemoveCategoriesFromCache();
            return RedirectToAction("Index");
        }
    }
}