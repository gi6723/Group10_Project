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
    public class PermitController : Controller
    {
        private Group10_DBEntities db = new Group10_DBEntities();

        // GET: Permit
        public ActionResult Index()
        {
            var permits = db.Permits.Include(p => p.EO).Include(p => p.RE).Include(p => p.PermitRequest);
            return View(permits.ToList());
        }

        // GET: Permit/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permit permit = db.Permits.Find(id);
            if (permit == null)
            {
                return HttpNotFound();
            }
            return View(permit);
        }

        // GET: Permit/Create
        public ActionResult Create()
        {
            ViewBag.issuedBy = new SelectList(db.EOs, "ID", "Name");
            ViewBag.issuedTo = new SelectList(db.REs, "ID", "contactPersonName");
            ViewBag.relatedTo = new SelectList(db.PermitRequests, "requestNo", "activityDescription");
            return View();
        }

        // POST: Permit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "permitID,dateOfIssue,duration,description,issuedBy,issuedTo,relatedTo")] Permit permit)
        {
            if (ModelState.IsValid)
            {
                db.Permits.Add(permit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.issuedBy = new SelectList(db.EOs, "ID", "Name", permit.issuedBy);
            ViewBag.issuedTo = new SelectList(db.REs, "ID", "contactPersonName", permit.issuedTo);
            ViewBag.relatedTo = new SelectList(db.PermitRequests, "requestNo", "activityDescription", permit.relatedTo);
            return View(permit);
        }

        // GET: Permit/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permit permit = db.Permits.Find(id);
            if (permit == null)
            {
                return HttpNotFound();
            }
            ViewBag.issuedBy = new SelectList(db.EOs, "ID", "Name", permit.issuedBy);
            ViewBag.issuedTo = new SelectList(db.REs, "ID", "contactPersonName", permit.issuedTo);
            ViewBag.relatedTo = new SelectList(db.PermitRequests, "requestNo", "activityDescription", permit.relatedTo);
            return View(permit);
        }

        // POST: Permit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "permitID,dateOfIssue,duration,description,issuedBy,issuedTo,relatedTo")] Permit permit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(permit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.issuedBy = new SelectList(db.EOs, "ID", "Name", permit.issuedBy);
            ViewBag.issuedTo = new SelectList(db.REs, "ID", "contactPersonName", permit.issuedTo);
            ViewBag.relatedTo = new SelectList(db.PermitRequests, "requestNo", "activityDescription", permit.relatedTo);
            return View(permit);
        }

        // GET: Permit/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permit permit = db.Permits.Find(id);
            if (permit == null)
            {
                return HttpNotFound();
            }
            return View(permit);
        }

        // POST: Permit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Permit permit = db.Permits.Find(id);
            db.Permits.Remove(permit);
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
