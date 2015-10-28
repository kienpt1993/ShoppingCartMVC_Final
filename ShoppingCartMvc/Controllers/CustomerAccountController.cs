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
                    int result = db.SaveChanges();
                    if (result >0)
                    {
                        System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                            new System.Net.Mail.MailAddress("luckyboyuit06@gmail.com", "Web Registration"),
                            new System.Net.Mail.MailAddress(customer.Email));
                        m.Subject = "Email confirmation";
                        m.Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to comlete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", customer.Email, Url.Action("ConfirmEmail", "Account", new { Token = customer.CustomerID, Email = customer.Email }, Request.Url.Scheme));
                        m.IsBodyHtml = true;
                        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                        smtp.Credentials = new System.Net.NetworkCredential("luckyboyuit06@gmail.com", "fxiabhvdgejdcexf");
                        smtp.EnableSsl = true;
                        smtp.Send(m);
                        return RedirectToAction("Confirm", "Account", new { Email = customer.Email });
                    }
                    else
                    {
                        
                    }

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
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult MyLogout()
        {
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }

    }
}