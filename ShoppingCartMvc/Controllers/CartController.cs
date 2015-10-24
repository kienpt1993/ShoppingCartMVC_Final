using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCartMvc.Models;

namespace ShoppingCartMvc.Controllers
{
    public class CartController : Controller
    {
        ShopingCartEF.ShoppingCartEntities db = new ShopingCartEF.ShoppingCartEntities();
        //
        // GET: /Cart/
        public ActionResult Index()
        {
            return View(GetCart());
        }
        public RedirectToRouteResult AddToCart(int id, int? quantity)
        {
            int q = (quantity ?? 1);
            // Kiem tra san pham
           var product =  db.Products.Find(id);
           if (product != null)
               GetCart().AddToCart(product, q);

           return RedirectToAction("Index");
        }
        public MyCart GetCart()
        {
            MyCart cart = (MyCart)Session["Cart"];
            if (cart == null)
            {
                cart = new MyCart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}