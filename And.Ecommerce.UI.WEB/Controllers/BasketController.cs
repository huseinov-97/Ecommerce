using And.Ecommerce.Core.Model;
using And.Ecommerce.Core.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace And.Ecommerce.UI.WEB.Controllers
{
    public class BasketController : AndControllerBase
    {
        // GET: Basket
        [HttpPost]
        public JsonResult AddProduct(int productID)
        {
            var db = new AndDB();
            Basket basket;
            if (db.Baskets.Any(b => b.ProductID == productID && b.UserID == this.LoginUserID))
            {
                basket = db.Baskets.FirstOrDefault(b => b.ProductID == productID && b.UserID == this.LoginUserID);
                basket.Quantity++;
            }
            else
            {
                basket = new Basket()
                {
                    CreateDate = DateTime.Now.Date,
                    CreateUserID = this.LoginUserID,
                    ProductID = productID,
                    Quantity = 1,
                    UserID = this.LoginUserID,
                };
                db.Baskets.Add(basket);
            }
            var result = db.SaveChanges();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            var db = new AndDB();
            var data = db.Baskets.Include("Product").Where(x => x.UserID == LoginUserID).ToList();
            return View(data);
        }
        public ActionResult Delete(int id)
        {
            var db = new AndDB();
            var deleteItem = db.Baskets.Where(x => x.ID == id).FirstOrDefault();
            db.Baskets.Remove(deleteItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //public ActionResult Delete(int id)
        //{
        //    var db = new AndDB();
        //    if (db.Baskets.Any(b => b.ProductID == id && b.UserID == this.LoginUserID))
        //    {
        //        var basket = db.Baskets.FirstOrDefault(b => b.ProductID == id && b.UserID == this.LoginUserID);
        //        db.Baskets.Remove(basket);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}
        public ActionResult Decrease(int id)
        {
            var db = new AndDB();
            var decreaseItem = db.Baskets.Where(x => x.ID == id).FirstOrDefault();
               
                if (decreaseItem.Quantity >= 1)
                {
                decreaseItem.Quantity--;
                    if (decreaseItem.Quantity == 0)
                    {
                        db.Baskets.Remove(decreaseItem);
                    }

                }
                db.SaveChanges();
            
            return RedirectToAction("Index");
        }
    }
}