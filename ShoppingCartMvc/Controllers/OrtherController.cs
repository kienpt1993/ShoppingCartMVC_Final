﻿using System;
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
    public class OrtherController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: /Orther/
        public ActionResult Index()
        {
            var orthers = db.Orthers.Include(o => o.Customer).Include(o => o.PaymenMethod);
            return View(orthers.ToList());
        }

        // GET: /Orther/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orther orther = db.Orthers.Find(id);
            if (orther == null)
            {
                return HttpNotFound();
            }
            return View(orther);
        }

        // GET: /Orther/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name");
            ViewBag.PaymentMethod = new SelectList(db.PaymenMethods, "PaymenMethodsID", "Name");
            return View();
        }

        // POST: /Orther/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="OrtherID,CustomerID,DateOrdered,DateRicived,ShippingMethod,PaymentMethod,PaymentType,Status,Amout")] Orther orther)
        {
            if (ModelState.IsValid)
            {
                db.Orthers.Add(orther);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", orther.CustomerID);
            ViewBag.PaymentMethod = new SelectList(db.PaymenMethods, "PaymenMethodsID", "Name", orther.PaymenMethodsID);
            return View(orther);
        }

        // GET: /Orther/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orther orther = db.Orthers.Find(id);
            if (orther == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", orther.CustomerID);
            ViewBag.PaymentMethod = new SelectList(db.PaymenMethods, "PaymenMethodsID", "Name", orther.PaymenMethodsID);
            return View(orther);
        }

        // POST: /Orther/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="OrtherID,CustomerID,DateOrdered,DateRicived,ShippingMethod,PaymentMethod,PaymentType,Status,Amout")] Orther orther)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orther).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", orther.CustomerID);
            ViewBag.PaymentMethod = new SelectList(db.PaymenMethods, "PaymenMethodsID", "Name", orther.PaymenMethodsID);
            return View(orther);
        }

        // GET: /Orther/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orther orther = db.Orthers.Find(id);
            if (orther == null)
            {
                return HttpNotFound();
            }
            return View(orther);
        }

        // POST: /Orther/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orther orther = db.Orthers.Find(id);
            db.Orthers.Remove(orther);
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
