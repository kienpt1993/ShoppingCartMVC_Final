using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopingCartEF;
using System.IO;
using PagedList.Mvc;
using ColorLife.Core.Mvc;
using System.Text;

namespace ShoppingCartMvc.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: Admin/Categories
        public ActionResult Index(int? page, int? pageSize, string keyword)
        {
            int pageNumber = (page ?? 1);
            int pageSize2 = (pageSize ?? 20);
            var category = db.Categories.ToList();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                category = category.Where(x => x.Name.ToLower().Contains(keyword) || x.Name.StartsWith(keyword)).ToList();
                //var c = Encoding.UTF8.GetString(Encoding.Default.GetBytes(keyword));
                //category = category.Where(x => Encoding.UTF8.GetString(Encoding.Default.GetBytes(x.Name.ToLower())).StartsWith(c)).ToList();

            }
            var model = category.ToList().ToPagedList(pageNumber, pageSize2);
            if (Request.IsAjaxRequest())
                return PartialView("_ListCategries", model);

            return View(model);
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View(new Category { CategoryID = 1 });
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Có lỗi xảy ra. Dữ liệu nhập vào không hợp lệ");
            return View(category);
        }

        public ActionResult Create_Backup(Category category, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {

                if (uploadFile != null)
                {
                    // Path.Combine(HttpContext.Server.MapPath("/Images"), Path.GetFileName(uploadFile.FileName));                    
                    string filePath = Server.MapPath("/Images");
                    uploadFile.SaveAs(filePath + "/" + uploadFile.FileName);
                    category.ImageUrl = "/images/" + uploadFile.FileName;

                }
                db.Categories.Add(category);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Có lỗi xảy ra. Dữ liệu nhập vào không hợp lệ");
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,Name,Description,ImageUrl,ParentID,SortOrder,IsPublished")] Category category, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {

                if (uploadFile != null)
                {
                    //string filePath = Path.Combine(HttpContext.Server.MapPath("/Images"), Path.GetFileName(uploadFile.FileName));
                    string filePath = HttpContext.Server.MapPath("/Images");
                    uploadFile.SaveAs(filePath + "/" + uploadFile.FileName);
                    category.ImageUrl = "/images/" + uploadFile.FileName;

                }
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
