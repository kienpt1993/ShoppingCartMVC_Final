using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



using ShoppingCartMvc.Models;
using ShopingCartEF;

namespace ShoppingCartMvc.Controllers
{
    public class CheckoutController : Controller
    {
        //
        // GET: /Checkout/
        public ActionResult Index()
        {

            var db = new ShoppingCartEntities();
            var payments = db.PaymenMethods.ToList();
            ViewBag.Payments = payments;
            return View(GetCart());
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
        public ActionResult Checkout()
        {


            return View(new Orther { });
        }
        [HttpPost]
        public ActionResult AddToOrder(int PaymentMethod)
        {
            var db = new ShoppingCartEntities();
            if (GetCart() != null)
            {
                var orther = new Orther
                {
                    Amout = GetCart().SubTotal(),
                    DateOrdered = DateTime.Now,
                    Status = "Chưa thanh toán",
                    PaymenMethodsID = PaymentMethod

                };
                db.Orthers.Add(orther);
                db.SaveChanges();
                Session.Abandon();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}