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
    public class RESitesController : Controller
    {
        private Group10_DBEntities db = new Group10_DBEntities();

        // GET: RESites
        public ActionResult Index()
        {
            var rESites = db.RESites.Include(r => r.RE);
            return View(rESites.ToList());
        }

        // GET: RESites/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESite rESite = db.RESites.Find(id);
            if (rESite == null)
            {
                return HttpNotFound();
            }
            return View(rESite);
        }

        // GET: RESites/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.REs, "ID", "contactPersonName");
            return View();
        }

        // POST: RESites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,siteAddress,siteContactPerson")] RESite rESite)
        {
            if (ModelState.IsValid)
            {
                db.RESites.Add(rESite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.REs, "ID", "contactPersonName", rESite.ID);
            return View(rESite);
        }

        // GET: RESites/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESite rESite = db.RESites.Find(id);
            if (rESite == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.REs, "ID", "contactPersonName", rESite.ID);
            return View(rESite);
        }

        // POST: RESites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,siteAddress,siteContactPerson")] RESite rESite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rESite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.REs, "ID", "contactPersonName", rESite.ID);
            return View(rESite);
        }

        // GET: RESites/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RESite rESite = db.RESites.Find(id);
            if (rESite == null)
            {
                return HttpNotFound();
            }
            return View(rESite);
        }

        // POST: RESites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RESite rESite = db.RESites.Find(id);
            db.RESites.Remove(rESite);
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
