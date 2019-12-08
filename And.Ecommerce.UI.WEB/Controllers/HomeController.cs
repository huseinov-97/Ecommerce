using And.Ecommerce.Core.Model;
using And.Ecommerce.Core.Model.Entity;
using And.Ecommerce.UI.WEB.Helpers;
using And.Ecommerce.UI.WEB.Models;
using And.ECommerce.UI.WEB.Helpers;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace And.Ecommerce.UI.WEB.Controllers
{
    public class HomeController : Controller
    {
        AndDB db = new AndDB();

        // GET: Home
        public ActionResult Index()
        {
            var currentUser = Session["LoggedUser"] as User;
            ViewBag.IsLogin = currentUser != null ? true : false;
            if (currentUser != null && !currentUser.IsEmailVerified)
            {
                ViewBag.NotVerified = "Please go to your email in order to verify your account, otherwise you cannot use website.";
            }
            var products = db.Products.Include("ProductImages").Take(6).OrderBy(p => p.ID).ToList();
            return View(products);

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
            var pass = Crypto.Hash(password);
            var data = db.Users.Where(u => u.Email == email && u.Password == pass
                            && u.IsActive == true && u.IsAdmin == false).ToList();
            if (data.Count == 1)
            {
                var user = data.FirstOrDefault();

                Session["IsUserLoggedin"] = true;
                Session["LoggedUser"] = user;
                Session["LoggedUserId"] = user.ID;
                return RedirectToAction("Index", "Profile", null);
            }
            ViewData["Error"] = "Wrong Credientials";
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel registerUser)
        {
            var user = new User
            {
                ImageUrl = "no-user.png",
                IsActive = true,
                IsAdmin = false,
                CreateDate = DateTime.Now.Date,
                CreateUserID = 1
            };

            if (ModelState.IsValid)
            {
                user.VerificationCode = Guid.NewGuid().ToString();
                user.IsEmailVerified = false;

                // Sent Form 
                registerUser.Password = Crypto.Hash(registerUser.Password);
                registerUser.ComparePassword = Crypto.Hash(registerUser.ComparePassword);

                //Assigning values to real user which will be stored in database
                user.Name = registerUser.Name;
                user.LastName = registerUser.LastName;
                user.Email = registerUser.Email;
                user.Password = registerUser.Password;
                user.Telephone = registerUser.Telephone;

                db.Users.Add(user);
                db.SaveChanges();

                SendCode(user,user.Email,user.VerificationCode);
                ViewBag.Message = "Verification Link has been sent to your account. Please verify your account, otherwise you will not be able to enter your profile.";
                ViewBag.Status = true;
            }
            else
            {
                ViewBag.Status = false;
            }
            return View();
        }

        public ActionResult VerifyAccount(string id)
        {
            var user = db.Users.FirstOrDefault(u => u.VerificationCode == new Guid(id).ToString());
            if (user != null)
            {
                try
                {
                    ViewBag.Status = true;
                    ViewBag.Message = "Congratulations! You have verified your account!";
                    user.IsEmailVerified = true;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    string error = string.Empty;

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            error += $"- Property: \"{ ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"";
                        }
                    }
                    throw new Exception(error);
                }
            }
            else
            {
                ViewBag.Message = "Invalid Request";
                ViewBag.Status = false;
            }
            return View();
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            Session["IsUserLoggedin"] = null;
            Session["LoggedUser"] = null;
            Session["LoggedUserId"] = null;

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
