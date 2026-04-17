using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Group10_Project.Models;

namespace Group10_Project.Controllers
{
    public class EnvironmentalPermitController : Controller
    {
        private Group10_DBEntities db = new Group10_DBEntities();

        // GET: EnvironmentalPermit
        public ActionResult Index()
        {
            var environmentalPermits = db.EnvironmentalPermits.Include(e => e.OPS_CPP);
            return View(environmentalPermits.ToList());
        }

        // GET: EnvironmentalPermit/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnvironmentalPermit environmentalPermit = db.EnvironmentalPermits.Find(id);
            if (environmentalPermit == null)
            {
                return HttpNotFound();
            }
            return View(environmentalPermit);
        }

        // GET: EnvironmentalPermit/Create
        public ActionResult Create()
        {
            ViewBag.paymentCtrlBy = new SelectList(db.OPS_CPP, "ID", "Name");
            return View();
        }

        // POST: EnvironmentalPermit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "permitID,permitName,permitFee,description,paymentCtrlBy")] EnvironmentalPermit environmentalPermit)
        {
            if (ModelState.IsValid)
            {
                db.EnvironmentalPermits.Add(environmentalPermit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.paymentCtrlBy = new SelectList(db.OPS_CPP, "ID", "Name", environmentalPermit.paymentCtrlBy);
            return View(environmentalPermit);
        }

        // GET: EnvironmentalPermit/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnvironmentalPermit environmentalPermit = db.EnvironmentalPermits.Find(id);
            if (environmentalPermit == null)
            {
                return HttpNotFound();
            }
            ViewBag.paymentCtrlBy = new SelectList(db.OPS_CPP, "ID", "Name", environmentalPermit.paymentCtrlBy);
            return View(environmentalPermit);
        }

        // POST: EnvironmentalPermit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "permitID,permitName,permitFee,description,paymentCtrlBy")] EnvironmentalPermit environmentalPermit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(environmentalPermit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.paymentCtrlBy = new SelectList(db.OPS_CPP, "ID", "Name", environmentalPermit.paymentCtrlBy);
            return View(environmentalPermit);
        }

        // GET: EnvironmentalPermit/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnvironmentalPermit environmentalPermit = db.EnvironmentalPermits.Find(id);
            if (environmentalPermit == null)
            {
                return HttpNotFound();
            }
            return View(environmentalPermit);
        }

        // POST: EnvironmentalPermit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            EnvironmentalPermit environmentalPermit = db.EnvironmentalPermits.Find(id);
            db.EnvironmentalPermits.Remove(environmentalPermit);
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
