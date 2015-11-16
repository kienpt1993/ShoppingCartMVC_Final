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
using System.Text;
using System.Security.Cryptography;
using Facebook;
using System.Configuration;

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
      
        [HttpGet]
        public JsonResult CheckEmailExists(string email)
        {
            var exists = db.Customers.Any(x => x.Email.Equals(email.Trim()));
            return Json(exists, JsonRequestBehavior.AllowGet);
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
        // GET: /Account/ConfirmEmail 
        [AllowAnonymous]
        public  ActionResult ConfirmEmail(int token, string email)
        {
            var v = db.Customers.Where(a => a.CustomerID.Equals(token)).FirstOrDefault();
            if (v != null)
            {
                if (v.Email == email)
                {
                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    return RedirectToAction("Confirm", "Customer", new { Email = v.Email });
                }
            }
            else
            {
                return RedirectToAction("Confirm", "Customer", new { Email = "" });
            }

        }

        public static string EncodeMD52(string stringValue)
        {
            Byte[] original;
            Byte[] encode;
            MD5 md5 = new MD5CryptoServiceProvider();
            original = ASCIIEncoding.Default.GetBytes(stringValue);
            encode = md5.ComputeHash(original);
            return BitConverter.ToString(encode);
        }

        private string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
        private string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
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
                    customer.Password = EncodeMD52(customer.Password);
                    var isEmail = db.Customers.FirstOrDefault(a => a.Email.Equals(customer.Email));
                    if  (isEmail != null)
                    {
                        db.Customers.Add(customer);
                        int result = db.SaveChanges();
                        if (result > 0)
                        {
                            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                                new System.Net.Mail.MailAddress("luckyboyuit06@gmail.com", "Web Registration"),
                                new System.Net.Mail.MailAddress(customer.Email));
                            m.Subject = "Email confirmation";
                            m.Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to comlete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", customer.Email, Url.Action("ConfirmEmail", "Customer", new { Token = customer.CustomerID, Email = customer.Email }, Request.Url.Scheme));
                            m.IsBodyHtml = true;
                            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                            smtp.Credentials = new System.Net.NetworkCredential("luckyboyuit06@gmail.com", "fxiabhvdgejdcexf");
                            smtp.EnableSsl = true;
                            smtp.Send(m);
                            return RedirectToAction("Confirm", "Customer", new { Email = customer.Email });
                        }
                    }
                   
                    else
                    {
                        
                    }

                }
            }

            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            ViewBag.Email = Email; return View();
        }
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        //private Uri 

        //{
        //    get
        //    {
        //        var uriBuilder = new UriBuilder(Request.Url);
        //        uriBuilder.Query = null;
        //        uriBuilder.Fragment = null;
        //        uriBuilder.Path = Url.Action("FacebookCallback");
        //        return uriBuilder.Uri;

        //    }
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult FacebookCallback(string code, string returnUrl)
        //{
        //    var fb = new FacebookClient();
        //    dynamic result = fb.Post("oauth/access_token",
        //        new
        //        {
        //            client_id = ConfigurationManager.AppSettings["FbAppId"],
        //            client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
        //            redirect_uri = RedirectUri.AbsoluteUri,
        //            code = code
        //        });
        //    var accessToken = result.access_token;
        //    if (!string.IsNullOrEmpty(accessToken))
        //    {
        //        fb.AccessToken = accessToken;
        //        dynamic me = fb.Get("me?fields= first_name,middle_name,last_name, id,email");
        //        string email = me.email;
        //        string userName = me.email;
        //        string firstName = me.first_name;
        //        string middleName = me.middle_name;
        //        string lastName = me.last_name;
        //        using (ShoppingCartEntities db = new ShoppingCartEntities())
        //        {
        //            var customer = new Customer();
        //            customer.Email = email;
        //            customer.Name = firstName + "" + middleName + "" + lastName;
        //            var isEmail = db.Customers.FirstOrDefault(a => a.Email.Equals(email));
        //            if (isEmail != null)
        //            {
        //                db.Customers.Add(customer);
        //                var saveChange = db.SaveChanges();
        //                if (saveChange > 0)
        //                {
        //                    Session["Customer"] = saveChange;
        //                    Session["Login"] = true;
        //                    return RedirectToLocal(returnUrl);
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", "Invalid username or password.");
        //                }
        //            }
        //        }
        //    }
        //    return RedirectToLocal(returnUrl);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LoginFacebook()
        //{
        //    var fb = new FacebookClient();
        //    var loginUrl = fb.GetLoginUrl(
        //        new
        //        {
        //            client_id = ConfigurationManager.AppSettings["FbAppId"],
        //            client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
        //            redirect_uri = RedirectUri.AbsoluteUri,
        //            response_type = "code",
        //            scope = "email"
        //        });
        //    return Redirect(loginUrl.AbsoluteUri);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer cus, string returnUrl)
        {           
            using (ShoppingCartEntities db = new ShoppingCartEntities())
            {
                cus.Password = EncodeMD52(cus.Password);
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
           // doi mat khau xong gui email
            return View();
        }
        public ActionResult FogotPassword()
        {
          // quen mat khau, gui email
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
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
