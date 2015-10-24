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
    public class OrderItemController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: /OrderItem/
        public ActionResult Index()
        {
            var orderitems = db.OrderItems.Include(o => o.Orther).Include(o => o.Product);
            return View(orderitems.ToList());
        }

        // GET: /OrderItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderitem = db.OrderItems.Find(id);
            if (orderitem == null)
            {
                return HttpNotFound();
            }
            return View(orderitem);
        }

        // GET: /OrderItem/Create
        public ActionResult Create()
        {
            ViewBag.OrtherID = new SelectList(db.Orthers, "OrtherID", "ShippingMethod");
            ViewBag.ProductID = new SelectList(db.Products, "ProductsID", "ImageUrl");
            return View();
        }

        // POST: /OrderItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="OrtherID,ProductID,Name,Price,Quantily")] OrderItem orderitem)
        {
            if (ModelState.IsValid)
            {
                db.OrderItems.Add(orderitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrtherID = new SelectList(db.Orthers, "OrtherID", "ShippingMethod", orderitem.OrtherID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductsID", "ImageUrl", orderitem.ProductID);
            return View(orderitem);
        }

        // GET: /OrderItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderitem = db.OrderItems.Find(id);
            if (orderitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrtherID = new SelectList(db.Orthers, "OrtherID", "ShippingMethod", orderitem.OrtherID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductsID", "ImageUrl", orderitem.ProductID);
            return View(orderitem);
        }

        // POST: /OrderItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="OrtherID,ProductID,Name,Price,Quantily")] OrderItem orderitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrtherID = new SelectList(db.Orthers, "OrtherID", "ShippingMethod", orderitem.OrtherID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductsID", "ImageUrl", orderitem.ProductID);
            return View(orderitem);
        }

        // GET: /OrderItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderitem = db.OrderItems.Find(id);
            if (orderitem == null)
            {
                return HttpNotFound();
            }
            return View(orderitem);
        }

        // POST: /OrderItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderItem orderitem = db.OrderItems.Find(id);
            db.OrderItems.Remove(orderitem);
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
