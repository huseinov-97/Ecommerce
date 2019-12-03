using And.Ecommerce.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace And.Ecommerce.UI.WEB.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        AndDB db = new AndDB();
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: Admin/AdminLogin
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string Email, string Password)
        {
            var data = db.Users.Where(u => u.Email == Email && u.Password == Password
            && u.IsActive==true 
            && u.IsAdmin==true).ToList();
            if (data.Count == 1)
            {
                Session["AdminLogin"] = data.FirstOrDefault();
                return RedirectToAction("Index", "Default");
            }
            else
            {
                return View();
            }
            
        }
    }
}