using And.Ecommerce.Core.Model;
using And.Ecommerce.Core.Model.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace And.Ecommerce.UI.WEB.Controllers
{
    public class ProductController : AndControllerBase
    {
        private AndDB db = new AndDB();
        public ActionResult Index()
        {
            ViewBag.IsLogin = Session["LoginUser"] != null ? true : false;

            var products = db.Products.Include(p=>p.Category);
            return View(products.ToList());
        }

        // GET: Product
        [Route("products/{title}/{id}")]
        public ActionResult Details(string title, int? id)
        {
            //var db = new AndDB();
            //var prod = db.Products.Where(x => x.ID == id).FirstOrDefault();
            //return View(prod);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Include("ProductImages").FirstOrDefault(p => p.ID == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
    }
}