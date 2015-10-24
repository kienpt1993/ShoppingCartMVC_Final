using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopingCartEF;
using ShoppingCartMvc.Filters;

namespace ShoppingCartMvc.Controllers
{
    public class CustomerController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();


        // Trang ca nhan: MyProfile
        // Trang Login
        // Trang Register
        // Trang MyOrder
        // Trang ChangePassword,
        // Trang FogotPassword

        // GET: /Customer/
        [CustomerLoginFilter]
        public ActionResult Index()
        {
            
            return View();
        }
      
        public ActionResult Register()
        {
            // kiem tra dang nhap ahay chua
            if (Session["Customer"] == null)
            {
                return View(new Customer { });
            }
            return RedirectToAction("Index", "Home");

        }
        // Action nayxu ly, khong hien thi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                using (ShoppingCartEntities db = new ShoppingCartEntities())
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();

                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer cus, string returnUrl)
        {
            using (ShoppingCartEntities db = new ShoppingCartEntities())
            {
                var v = db.Customers.Where(a => a.Email.Equals(cus.Email) && a.Password.Equals(cus.Password)).FirstOrDefault();
                if (v != null)
                {
                    Session["Customer"] = v;
                    Session["UserName"] = v.Name.ToString();
                    Session["Login"] = true;
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(cus);

        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [CustomerLoginFilter]
        public ActionResult MyProfile()
        {
            
            return View();
        }
        [CustomerLoginFilter]
        public ActionResult ChangePassword()
        {
           
            return View();
        }
        public ActionResult FogotPassword()
        {
          
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyLogout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
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
