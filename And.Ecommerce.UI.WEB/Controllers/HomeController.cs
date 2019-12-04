using And.Ecommerce.Core.Model;
using And.Ecommerce.Core.Model.Entity;
using And.Ecommerce.UI.WEB.Models;
using Evmatic.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace And.Ecommerce.UI.WEB.Controllers
{
    public class HomeController : AndControllerBase
    {
        AndDB db = new AndDB();

        // GET: Home
        public ActionResult Index()
        {
            ViewBag.IsLogin = Session["LoginUser"] != null ? true : false;
            var products = db.Products.Take(6).OrderBy(p => p.ID).ToList();
            return View(products);
            //ViewBag.IsLogin = this.IsLogin;
            //var data = db.Products.OrderByDescending(x => x.CreateDate).Take(5).ToList();


        }

        public PartialViewResult GetMenu()
        {

            var menus = db.Categories.Where(x => x.ParentID == 0).ToList();
            return PartialView(menus);
        }

        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]

        public ActionResult Login(string email, string password)
        {
            var data = db.Users.Where(u => u.Email == email && u.Password == password
                            && u.IsActive == true && u.IsAdmin == false).ToList();
            if (data.Count == 1)
            {
                var user = data.FirstOrDefault();
                Session["IsLogin"] = true;
                Session["LoginUser"] = user;
                Session["LoginUserID"] = user.ID;
                return RedirectToAction("Index", "Home", null);
            }
            ViewData["Error"] = "Wrong Credientials";
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Register(User user)
        {
            
            user.IsActive = true;
            user.IsAdmin = false;
            user.CreateDate = DateTime.Now.Date;
            user.CreateUserID = 1;

            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            // Redirecting to Login page after deleting Session
            return RedirectToAction("Index", "Home");
        }



        public ActionResult ForgetPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(string email)
        {
            if (email != null || email == string.Empty)
            {
                if (db.Users.Any(u => u.Email == email))
                {
                    var user = db.Users.FirstOrDefault(u => u.Email == email);
                    ViewData["Success"] = "We have sent email to your email address";
                    var resetCode = Guid.NewGuid().ToString();
                    SendCode(user,user.Email,resetCode, "ResetPassword");
                    user.ResetPasswordCode = resetCode;
                    db.SaveChanges();
                }
                else
                {
                    ViewData["Error"] = "User was not found.";

                }
            }
            
            return View();
        }

        [NonAction]
        public void SendCode(User user, string email, string code, string purpose = "VerifyAccount")
        {
            var verifyUrl = $"/Home/{purpose}/" + code;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            switch (purpose)
            {
                case "VerifyAccount": MailHelper.Send(user, "Verify Your Account",
                    $"Please verify your account: <a href='{link}'>Verify link</a>"
                    );break;

                case "ResetPassword": MailHelper.Send(user, "Reset Password",
                    $"Please reset your password your account: <a href='{link}'>Reset Password link</a>"
                    );break;
            }
        }

        public ActionResult ResetPassword(string id)
        {
            if (db.Users.Any(u => u.ResetPasswordCode == id))
            {
                var user = db.Users.FirstOrDefault(u => u.ResetPasswordCode == id);

                if (user != null)
                {
                    ResetPasswordModel reset = new ResetPasswordModel();
                    reset.ResetCode = id;
                    return View(reset);
                }
                
            }
            else
            {
                return HttpNotFound();

            }
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.ResetPasswordCode == model.ResetCode);

                if (user != null)
                {
                    user.Password = model.Password;
                    user.ResetPasswordCode = string.Empty;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();

                    ViewBag.Success = "Your password has been changed successfully";
                }
               
            }
            else
            {
                ViewBag.Error = "There is a problem in your credentials.";
            }
            return View();
        }
    }
}
