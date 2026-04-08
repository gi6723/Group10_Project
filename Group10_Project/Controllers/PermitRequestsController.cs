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
    public class PermitRequestsController : Controller
    {
        private Group10_DBEntities db = new Group10_DBEntities();

        // GET: PermitRequests
        public ActionResult Index()
        {
            var permitRequests = db.PermitRequests.Include(p => p.EnvironmentalPermit).Include(p => p.Payment).Include(p => p.RE);
            return View(permitRequests.ToList());
        }

        // GET: PermitRequests/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermitRequest permitRequest = db.PermitRequests.Find(id);
            if (permitRequest == null)
            {
                return HttpNotFound();
            }
            return View(permitRequest);
        }

        // GET: PermitRequests/Create
        public ActionResult Create()
        {
            ViewBag.permitTypeID = new SelectList(db.EnvironmentalPermits, "permitID", "permitName");
            ViewBag.permitPayment = new SelectList(db.Payments, "paymentID", "paymentMethod");
            ViewBag.permitREID = new SelectList(db.REs, "ID", "contactPersonName");
            return View();
        }

        // POST: PermitRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "requestNo,dateOfRequest,activityDescription,activityStartDate,activityDuration,permitFee,permitTypeID,permitREID,permitPayment")] PermitRequest permitRequest)
        {
            if (ModelState.IsValid)
            {
                db.PermitRequests.Add(permitRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.permitTypeID = new SelectList(db.EnvironmentalPermits, "permitID", "permitName", permitRequest.permitTypeID);
            ViewBag.permitPayment = new SelectList(db.Payments, "paymentID", "paymentMethod", permitRequest.permitPayment);
            ViewBag.permitREID = new SelectList(db.REs, "ID", "contactPersonName", permitRequest.permitREID);
            return View(permitRequest);
        }

        // GET: PermitRequests/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermitRequest permitRequest = db.PermitRequests.Find(id);
            if (permitRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.permitTypeID = new SelectList(db.EnvironmentalPermits, "permitID", "permitName", permitRequest.permitTypeID);
            ViewBag.permitPayment = new SelectList(db.Payments, "paymentID", "paymentMethod", permitRequest.permitPayment);
            ViewBag.permitREID = new SelectList(db.REs, "ID", "contactPersonName", permitRequest.permitREID);
            return View(permitRequest);
        }

        // POST: PermitRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "requestNo,dateOfRequest,activityDescription,activityStartDate,activityDuration,permitFee,permitTypeID,permitREID,permitPayment")] PermitRequest permitRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(permitRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.permitTypeID = new SelectList(db.EnvironmentalPermits, "permitID", "permitName", permitRequest.permitTypeID);
            ViewBag.permitPayment = new SelectList(db.Payments, "paymentID", "paymentMethod", permitRequest.permitPayment);
            ViewBag.permitREID = new SelectList(db.REs, "ID", "contactPersonName", permitRequest.permitREID);
            return View(permitRequest);
        }

        // GET: PermitRequests/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermitRequest permitRequest = db.PermitRequests.Find(id);
            if (permitRequest == null)
            {
                return HttpNotFound();
            }
            return View(permitRequest);
        }

        // POST: PermitRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PermitRequest permitRequest = db.PermitRequests.Find(id);
            db.PermitRequests.Remove(permitRequest);
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
