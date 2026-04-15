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
    public class RequestStatusController : Controller
    {
        private Group10_DBEntities db = new Group10_DBEntities();

        // GET: RequestStatus
        public ActionResult Index()
        {
            var requestStatus = db.RequestStatus.Include(r => r.PermitRequest);
            return View(requestStatus.ToList());
        }

        // GET: RequestStatus/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestStatus requestStatu = db.RequestStatus.Find(id);
            if (requestStatu == null)
            {
                return HttpNotFound();
            }
            return View(requestStatu);
        }

        // GET: RequestStatus/Create
        public ActionResult Create()
        {
            ViewBag.requestID = new SelectList(db.PermitRequests, "requestNo", "activityDescription");
            return View();
        }

        // POST: RequestStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "permitRequestStatus,date,description,requestID")] RequestStatus requestStatu)
        {
            if (ModelState.IsValid)
            {
                db.RequestStatus.Add(requestStatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.requestID = new SelectList(db.PermitRequests, "requestNo", "activityDescription", requestStatu.requestID);
            return View(requestStatu);
        }

        // GET: RequestStatus/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestStatus requestStatu = db.RequestStatus.Find(id);
            if (requestStatu == null)
            {
                return HttpNotFound();
            }
            ViewBag.requestID = new SelectList(db.PermitRequests, "requestNo", "activityDescription", requestStatu.requestID);
            return View(requestStatu);
        }

        // POST: RequestStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "permitRequestStatus,date,description,requestID")] RequestStatus requestStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestStatu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.requestID = new SelectList(db.PermitRequests, "requestNo", "activityDescription", requestStatu.requestID);
            return View(requestStatu);
        }

        // GET: RequestStatus/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestStatus requestStatu = db.RequestStatus.Find(id);
            if (requestStatu == null)
            {
                return HttpNotFound();
            }
            return View(requestStatu);
        }

        // POST: RequestStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RequestStatus requestStatu = db.RequestStatus.Find(id);
            db.RequestStatus.Remove(requestStatu);
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
