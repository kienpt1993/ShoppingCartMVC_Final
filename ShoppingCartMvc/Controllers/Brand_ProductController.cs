using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopingCartEF;

namespace ShoppingCartMvc.Controllers
{
    public class Brand_ProductController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: /Brand_Product/
        public ActionResult Index()
        {
            var band_product = db.Band_Product.Include(b => b.Brand).Include(b => b.Product);
            return View(band_product.ToList());
        }

        // GET: /Brand_Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Band_Product band_product = db.Band_Product.Find(id);
            if (band_product == null)
            {
                return HttpNotFound();
            }
            return View(band_product);
        }

        // GET: /Brand_Product/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "ImageUrl");
            ViewBag.ProductsID = new SelectList(db.Products, "ProductsID", "ImageUrl");
            return View();
        }

        // POST: /Brand_Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="BrandID,ProductsID,Note")] Band_Product band_product)
        {
            if (ModelState.IsValid) // day la code validate
            {
                db.Band_Product.Add(band_product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "ImageUrl", band_product.BrandID);
            ViewBag.ProductsID = new SelectList(db.Products, "ProductsID", "ImageUrl", band_product.ProductsID);
            return View(band_product);
        }

        // GET: /Brand_Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Band_Product band_product = db.Band_Product.Find(id);
            if (band_product == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "ImageUrl", band_product.BrandID);
            ViewBag.ProductsID = new SelectList(db.Products, "ProductsID", "ImageUrl", band_product.ProductsID);
            return View(band_product);
        }

        // POST: /Brand_Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="BrandID,ProductsID,Note")] Band_Product band_product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(band_product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "ImageUrl", band_product.BrandID);
            ViewBag.ProductsID = new SelectList(db.Products, "ProductsID", "ImageUrl", band_product.ProductsID);
            return View(band_product);
        }

        // GET: /Brand_Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Band_Product band_product = db.Band_Product.Find(id);
            if (band_product == null)
            {
                return HttpNotFound();
            }
            return View(band_product);
        }

        // POST: /Brand_Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Band_Product band_product = db.Band_Product.Find(id);
            db.Band_Product.Remove(band_product);
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
