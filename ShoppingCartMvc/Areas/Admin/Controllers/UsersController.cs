using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using ShoppingCartMvc.Models;

namespace ShoppingCartMvc.Areas.Admin.Controllers
{
   // [Authorize(Roles="Admin")]
    public class UsersController : Controller
    {
        private IdentityDbContext db = new IdentityDbContext();

          public UsersController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())),
              new RoleManager<IdentityRole>(new  RoleStore<IdentityRole>(new ApplicationDbContext())))
        {
        }

          public UsersController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> role)
        {
            UserManager = userManager;
            Roles = role;
        }

       
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public RoleManager<IdentityRole> Roles { get; set; }
        //public UsersController(UserManager<ApplicationUser> userManager)
        //{
        //    UserManager = userManager;
        //}
        // GET: Admin/Users
        public ActionResult Index()
        {           
            return View(UserManager.Users.ToList());
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var  applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }
        private bool HasPassword(string id)
        {
            var applicationUser = db.Users.Find(id);
             
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (applicationUser != null && user != null)
            {
                if (applicationUser.Id.Equals(user.Id))
                    return user.PasswordHash != null;
                return false;
            }
            return false;
        }
        public ActionResult ChangePwd(string id, ManageMessageId? message)
        {
            ViewBag.StatusMessage =
               message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
               : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
               : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
               : message == ManageMessageId.Error ? "An error has occurred."
               : "";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            ViewBag.HasLocalPassword = HasPassword(id);
            ViewBag.ReturnUrl = Url.Action("Index");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePwd(string id, ManageUserViewModel model)
        {
            bool hasPassword = HasPassword(id);
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Index");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    UserManager.RemovePassword(id);
                    IdentityResult result = await UserManager.AddPasswordAsync(id, model.ConfirmPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        
        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            ViewBag.ListRoles = Roles.Roles.ToList();
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model, string[] roleId)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (roleId != null)
                    {
                        user = UserManager.FindByEmail(model.Email);
                        foreach (var item in roleId)
                        {
                            if (user != null)
                                UserManager.AddToRole(user.Id, item.ToString());
                        }
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var  applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var list = applicationUser.Roles;

            var rolesForUser = UserManager.GetRoles(id);

            ViewBag.ListMyRoles = rolesForUser;
            ViewBag.ListRoles = Roles.Roles.ToList();



            return View(applicationUser);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityUser applicationUser, string[] roleId)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationUser).State = EntityState.Modified;
                db.SaveChanges();
                var rolesForUser = UserManager.GetRoles(applicationUser.Id);
                foreach (var item in rolesForUser)
                {
                    UserManager.RemoveFromRole(applicationUser.Id, item.ToString());
                }
                if (roleId != null)
                {
                    foreach (var item in roleId)
                    {
                        UserManager.AddToRole(applicationUser.Id, item.ToString());
                    }
                }
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }
        // GET: Admin/Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var  applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var  applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
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
