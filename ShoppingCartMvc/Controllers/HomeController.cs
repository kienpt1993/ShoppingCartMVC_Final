using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCartMvc;
using ShopingCartEF;
using ShoppingCartMvc.Models;
namespace ShoppingCartMvc.Controllers
{
    public class HomeController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();
        public ActionResult Index()
        {

            var model = new Category_Brand()
            {
                pro = db.Products.ToList(),
                brand = db.Brands.ToList(),
                cate = db.Categories.ToList()
            };
            
            return View(model);
        }
        public ActionResult Welcome(string id)
        {
            ViewBag.TenDangNhap = id;
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}