using And.Ecommerce.Core.Model;
using And.Ecommerce.Core.Model.Entity;
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
            //try
            //{
            //    user.IsActive = true;
            //    user.IsAdmin = false;
            //    user.CreateDate = DateTime.Now.Date;
            //    user.CreateUserID = 1;
            //    db.Users.Add(user);
            //    db.SaveChanges();
            //    return Redirect("/");
            //}
            //catch (Exception)
            //{

            //    return View();
            //}
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
    }
}