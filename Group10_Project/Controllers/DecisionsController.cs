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
    public class DecisionsController : Controller
    {
        private Group10_DBEntities db = new Group10_DBEntities();

        // GET: Decisions
        public ActionResult Index()
        {
            var decisions = db.Decisions.Include(d => d.EO).Include(d => d.PermitRequest);
            return View(decisions.ToList());
        }

        // GET: Decisions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Decision decision = db.Decisions.Find(id);
            if (decision == null)
            {
                return HttpNotFound();
            }
            return View(decision);
        }

        // GET: Decisions/Create
        public ActionResult Create()
        {
            ViewBag.EOID = new SelectList(db.EOs, "ID", "Name");
            ViewBag.permitRequestID = new SelectList(db.PermitRequests, "requestNo", "activityDescription");
            return View();
        }

        // POST: Decisions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,dateOfDecision,finalDecision,description,EOID,permitRequestID")] Decision decision)
        {
            if (ModelState.IsValid)
            {
                db.Decisions.Add(decision);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EOID = new SelectList(db.EOs, "ID", "Name", decision.EOID);
            ViewBag.permitRequestID = new SelectList(db.PermitRequests, "requestNo", "activityDescription", decision.permitRequestID);
            return View(decision);
        }

        // GET: Decisions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Decision decision = db.Decisions.Find(id);
            if (decision == null)
            {
                return HttpNotFound();
            }
            ViewBag.EOID = new SelectList(db.EOs, "ID", "Name", decision.EOID);
            ViewBag.permitRequestID = new SelectList(db.PermitRequests, "requestNo", "activityDescription", decision.permitRequestID);
            return View(decision);
        }

        // POST: Decisions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,dateOfDecision,finalDecision,description,EOID,permitRequestID")] Decision decision)
        {
            if (ModelState.IsValid)
            {
                db.Entry(decision).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EOID = new SelectList(db.EOs, "ID", "Name", decision.EOID);
            ViewBag.permitRequestID = new SelectList(db.PermitRequests, "requestNo", "activityDescription", decision.permitRequestID);
            return View(decision);
        }

        // GET: Decisions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Decision decision = db.Decisions.Find(id);
            if (decision == null)
            {
                return HttpNotFound();
            }
            return View(decision);
        }

        // POST: Decisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Decision decision = db.Decisions.Find(id);
            db.Decisions.Remove(decision);
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
