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
    public class PaymenMethodController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: /PaymenMethod/
        public ActionResult Index()
        {
            return View(db.PaymenMethods.ToList());
        }

        // GET: /PaymenMethod/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymenMethod paymenmethod = db.PaymenMethods.Find(id);
            if (paymenmethod == null)
            {
                return HttpNotFound();
            }
            return View(paymenmethod);
        }

        // GET: /PaymenMethod/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /PaymenMethod/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="PaymenMethodsID,Name")] PaymenMethod paymenmethod)
        {
            if (ModelState.IsValid)
            {
                db.PaymenMethods.Add(paymenmethod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paymenmethod);
        }

        // GET: /PaymenMethod/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymenMethod paymenmethod = db.PaymenMethods.Find(id);
            if (paymenmethod == null)
            {
                return HttpNotFound();
            }
            return View(paymenmethod);
        }

        // POST: /PaymenMethod/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PaymenMethodsID,Name")] PaymenMethod paymenmethod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymenmethod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paymenmethod);
        }

        // GET: /PaymenMethod/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymenMethod paymenmethod = db.PaymenMethods.Find(id);
            if (paymenmethod == null)
            {
                return HttpNotFound();
            }
            return View(paymenmethod);
        }

        // POST: /PaymenMethod/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymenMethod paymenmethod = db.PaymenMethods.Find(id);
            db.PaymenMethods.Remove(paymenmethod);
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
