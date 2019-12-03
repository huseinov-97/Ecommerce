using And.Ecommerce.Core.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace And.Ecommerce.UI.WEB
{
    public class AndControllerBase : Controller
    {
        //User login control
        public bool IsLogin { get;private set; }
        public int LoginUserID { get; private set; }
        public User LoginUserEntity { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            //Session["LoginUserID"] 
            //Session["LoginUser"]
            //uye giris sonrasi deyisecek



            if (requestContext.HttpContext.Session["LoginUserID"] != null)
            {
                //kullanic giriss yapmis
                IsLogin = true;
                LoginUserID =(int)requestContext.HttpContext.Session["LoginUserID"];
                LoginUserEntity = (Core.Model.Entity.User)requestContext.HttpContext.Session["LoginUser"];
               
            }
 
            base.Initialize(requestContext);
        }
    }
}