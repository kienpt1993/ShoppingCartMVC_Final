
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopingCartEF;
using PagedList.Mvc;
using ColorLife.Core.Mvc;

namespace ShoppingCartMvc.Areas.Admin.Controllers
{
    public class ProductsController : BaseController
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();
        // GET: Admin/Products
        public ActionResult Index(int? page, int? pageSize, string keyword)
        {
            int pageIndex = (page ?? 1);
            int pageSize1 = (pageSize ?? 10);


            var products = db.Products.Include(p => p.Brand).Include(p => p.Category);
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                //  products = products.Where(x => string.Format("{0} {1}", x.Name, x.Description).ToLower().Contains(keyword));
                products = products.Where(x => x.Name.ToLower().Contains(keyword) || x.Description.ToLower().Contains(keyword));
            }

            var model = products.ToList().ToPagedList(pageIndex, pageSize1);
            if (Request.IsAjaxRequest())
                return PartialView("_ListPartial", model);
            return View(model);
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            return View(new Product { ProductsID = 1 });
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Product product, string[] ImageGallery)
        {
            if (ModelState.IsValid)
            {

                db.Entry(product).State = EntityState.Modified;
                db.Products.Add(product);
                int kq = db.SaveChanges();
                if (kq > 0)
                {
                    foreach (var item in ImageGallery)
                    {
                        var image = new Image { ImageUrl = item, ProductID = product.ProductsID, ImageID=1 };
                        db.Images.Add(image);
                        //    db.Entry(image).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                //var image = new Image { ImageUrl = "Hihe", ProductID = product.ProductsID, ImageID=1 };
                //db.Images.Add(image);
                //   //db.Entry(image).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");

            }
            ModelState.AddModelError("Thông báo", "Có lỗi");
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandID", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryID", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductsID,CategoryID,BrandID,ImageUrl,Name,Description,Price,SalePrice,Detail,DateCreated,SortOrder,IsPublished")] Product product, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadFile != null)
                {

                    string filePath = HttpContext.Server.MapPath("/Images");
                    uploadFile.SaveAs(filePath + "/" + uploadFile.FileName);
                    product.ImageUrl = "/images/" + uploadFile.FileName;
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "Name", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryID", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
