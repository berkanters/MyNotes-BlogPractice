﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyNotes.BusinessLayer;
using MyNotes.BusinessLayer.Model;
using MyNotes.BusinessLayer.ValueObject;
using MyNotes.EntityLayer;
using MyNotes.EntityLayer.Messages;
using MyNotes.MVC.ViewModel;


namespace MyNotes.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyNotesUserManager mum=new MyNotesUserManager();
        private readonly NoteManager nm=new NoteManager();
        private BusinessLayerResult<MyNotesUser> res;

        public ActionResult ByCategoryId(int? id)
        {
           
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Note> notes = nm.QList().Where(s => s.Category.Id == id && s.IsDraft == false)
                .OrderByDescending(s => s.ModifiedOn).ToList();
            ViewBag.CategoryId1 = id;
            return View("Index", notes);
            
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                res = mum.LoginUser(model);
                if (res.Errors.Count>0)
                {
                    if (res.Errors.Find(x=>x.Code==ErrorMessageCode.UserIsNotActive)!=null)
                    {
                        ViewBag.Setlink = "http://Home/USerActivate/1234-2345-3456789";
                    }
                    res.Errors.ForEach(s=>ModelState.AddModelError("Empty",s.Message));
                    return View(model);
                }

                //Session["Login"] = res.Result;
                CurrentSession.Set("Login",res.Result);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                MyNotesUserManager mum = new MyNotesUserManager();
                BusinessLayerResult<MyNotesUser> res = mum.RegisterUser(model);
                
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(s => ModelState.AddModelError("", s.Message));
                    return View(model);
                }
                OKViewModel notifyObj = new OKViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Index"
                };
                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz.");
                return View("Ok", notifyObj);
                //return RedirectToAction("Login");
            }
            return View(model);
        }

        public ActionResult RegsterOk()
        {
            return View();
        }
        public ActionResult UserActivate(Guid id)
        {
            this.res = mum.ActivateUser(id);
            
            BusinessLayerResult<MyNotesUser> res = mum.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("UserActivateCancel");
            }
            return RedirectToAction("UserActivateOk");
        }
        public ActionResult UserActivateOk()
        {
            return View();
        }
        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["errors"] != null)
            {
                errors = TempData["errors"] as List<ErrorMessageObj>;
            }
            return View(errors);
        }
        public ActionResult ShowProfile()
        {

            if (CurrentSession.User is MyNotesUser currentUser) res = mum.GetUserById(currentUser.Id);
            {
                if (res.Errors.Count > 0)
                { 
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Title = "Hata Oluştu",
                        Items = res.Errors
                    };
                    return View("Error", errorNotifyObj);
                }
               
            }
            return View(res.Result);
        }

        public ActionResult EditProfile()
        {
            if (CurrentSession.User is MyNotesUser currentUser) res = mum.GetUserById(currentUser.Id);
            {
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Title = "Hata Oluştu",
                        Items = res.Errors
                    };
                    return View("Error", errorNotifyObj);
                }
                return View(res.Result);
            }
            
            }

        [HttpPost]
        public ActionResult EditProfile(MyNotesUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUserName");
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                     ProfileImage.ContentType == "image/jpg" ||
                     ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFileName = filename;
                }
                //{
                //    ImageUploadResult uploadResult = ImageUploader.UploadSingleImage(ProfileImage, "~/Uploads/",
                //        new ResizeOptions()
                //        {
                //            Width = 200,
                //            Height = 200
                //        });
                //    if (uploadResult.Errors.Count > 0)
                //    {
                //        uploadResult.Errors.ForEach(x => ModelState.AddModelError("", x));
                //        return View(model);
                //    }
                //    model.ProfileImageFilename = uploadResult.UploadedImagePath;
                //}
                res = mum.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Title = "Profil Güncellenemedi",
                        Items = res.Errors,
                        RedirectingUrl = "/Home/EditProfile"
                    };
                    return RedirectToAction("Error", errorNotifyObj);
                }
                
            }
            CurrentSession.Set("login", res.Result);
            return RedirectToAction("ShowProfile");
        }

        public ActionResult DeleteProfile()
        {
            if (CurrentSession.User is MyNotesUser currentUser)
            {
                res = mum.RemoveUserById(currentUser.Id);
            }
            
            if (res.Errors.Count > 0)
            {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Title = "Hata Oluştu",
                        Items = res.Errors,
                        RedirectingUrl = "/Home/ShowProfile"
                    };
                    return View("Error", errorNotifyObj);
            }
            CurrentSession.Clear();
            return RedirectToAction("Index");
        }


        public ActionResult Index()
        {
            //Test test = new Test();
            return View(nm.QList().Where(s => s.IsDraft == false).OrderByDescending(s => s.ModifiedOn).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LogOut()
        {
            CurrentSession.Clear();
            return RedirectToAction("Index");
        }
    }
}