using ShopingCartEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCartMvc.Controllers
{
    public class CustomerAccountController : Controller
    {
        //
        // GET: /CustomerAccount/
        public ActionResult Index()
        {
            return View();
        }
        // Action nay chi hien thi, khong xu ly gi ca
        public ActionResult MyRegister()
        {
            // kiem tra dang nhap ahay chua
            if (Session["UserName"] == null)
            {
                return View(new Customer { });
            }
            return RedirectToAction("Index", "Home");
          
        }
        // Action nayxu ly, khong hien thi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyRegister(Customer customer)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyLogin(Customer cus, string returnUrl)
        {
            using (ShoppingCartEntities db = new ShoppingCartEntities())
            {
                var v = db.Customers.Where(a => a.Email.Equals(cus.Email) && a.Password.Equals(cus.Password)).FirstOrDefault();
                if (v != null)
                {
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
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult MyLogout()
        {
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }

    }
}