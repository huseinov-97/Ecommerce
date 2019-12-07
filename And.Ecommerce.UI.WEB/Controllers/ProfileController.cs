using And.Ecommerce.Core.Model;
using And.Ecommerce.Core.Model.Entity;
using And.ECommerce.UI.WEB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace And.Ecommerce.UI.WEB.Controllers
{
    public class ProfileController : AndControllerBase
    {
        private AndDB db = new AndDB();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: Profile
        public ActionResult Index()
        {
            var currentUser = Session["LoggedUser"] as User;
            if (currentUser != null && !currentUser.IsEmailVerified)
            {
                ViewBag.NotVerified = "Please go to your email in order to verify your account, otherwise you cannot use website.";
            }
            return View();
        }

        public ActionResult AddAvatar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveAvatar()
        {
            var currentUser = this.LoginUserEntity;

            if (currentUser != null)
            {
                var userInDb = db.Users.FirstOrDefault(u => u.ID == this.LoginUserID);
                if (userInDb != null)
                {
                    userInDb.ImageUrl = "no-user.png";
                    db.SaveChanges();
                    Session["LoggedUser"] = userInDb;

                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase file)
        {
            var user = db.Users.FirstOrDefault(u => u.ID == LoginUserID);
            ImageHelper.Save(user, file, "UserAvatars");
            db.SaveChanges();
            Session["LoggedUser"] = user;
            return Json("File Uploaded successfully");
        }
    }
}