using System.Net;
using And.Ecommerce.Core.Model;
using And.Ecommerce.Core.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace And.Ecommerce.UI.WEB.Controllers
{
    public class CategoryController : AndControllerBase
    {
        AndDB db = new AndDB();
        // GET: Category
        [Route("Category/{title}/{id}")]
   
        public ActionResult Index(int? id, string title)
        {
            var data = db.Products.Where(x => x.IsActive == true && x.CategoryID == id).ToList();
            ViewBag.category = db.Categories.Where(x => x.ID == id).FirstOrDefault();
            return View(data);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
 
}