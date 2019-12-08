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

           // var productImages = db.ProductImages.Where(p => p.CreateUserID == this.LoginUserID).ToList();
            var products = db.Products.Include("ProductImages").Where(p => p.CreateUserID == this.LoginUserID).ToList();
            return View(products);
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
        public ActionResult AddAvatar(HttpPostedFileBase file)
        {
            var user = db.Users.FirstOrDefault(u => u.ID == LoginUserID);
            ImageHelper.Save(user, file, "UserAvatars");
            db.SaveChanges();
            Session["LoggedUser"] = user;
            return Json("File Uploaded successfully");
        }

        public ActionResult AddProduct()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(Product product)
        {
            product.CreateUserID = this.LoginUserID;
            product.CreateDate = DateTime.Now.Date;
            product.IsActive = true;
            if (ModelState.IsValid)
            {
               
               
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("UploadFiles",new { product.ID});

            }
            return View();
        }

        public ActionResult UploadFiles(int id)
        {
            ViewBag.salam = "salam";
            ViewBag.ProductId = id;
            return View();
        }

        [HttpPost]
        public ActionResult UploadFiles(int id,HttpPostedFileBase[] files)
        {
            ViewBag.salam = "sagol";

            foreach (var file in files)
            {
                var productImage = new ProductImage
                {
                    CreateUserID = this.LoginUserID,
                    CreateDate = DateTime.Now.Date,
                    ProductId = id,
                };
                ImageHelper.Save(productImage, file);
                db.ProductImages.Add(productImage);
                db.SaveChanges();

            }


            return Json("Success"); 
        }
    }
}