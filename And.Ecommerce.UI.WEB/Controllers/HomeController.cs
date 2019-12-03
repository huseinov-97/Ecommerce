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
            //ViewBag.IsLogin = this.IsLogin;
            //var data = db.Products.OrderByDescending(x => x.CreateDate).Take(5).ToList();

            return View(/*data*/);
        }
        public PartialViewResult GetMenu()
        {

            //var menus = db.Categories.Where(x => x.ParentID == 0).ToList();
            return PartialView(/*menus*/);
        }
        [Route("login")]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
       [Route("login")]
        public ActionResult Login(string Email, string Password)
        {

            var users = db.Users.Where(x => x.Email == Email
             && x.Password == Password
             && x.IsActive == true
             && x.IsAdmin == false).ToList();
            if (users.Count == 1)
            {
                Session["LoginUserID"] = users.FirstOrDefault().ID;
                Session["LoginUser"] = users.FirstOrDefault();
                return Redirect("/");
            }
            else
            {
                ViewBag.Error = "Wrong Email address or Password";
                return View();

            }
        }
        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("Register")]
        public ActionResult Register(User user)
        {
            try
            {
                user.IsActive = true;
                user.IsAdmin = false;
                user.CreateDate = DateTime.Now.Date;
                user.CreateUserID = 1;
                db.Users.Add(user);
                db.SaveChanges();
                return Redirect("/");
            }
            catch (Exception)
            {

                return View();
            }
        }
    }
}