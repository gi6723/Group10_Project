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
    public class EmailArchiveController : Controller
    {
        private Group10_DBEntities db = new Group10_DBEntities();

        // GET: EmailArchive
        public ActionResult Index()
        {
            var emailArchives = db.EmailArchives.Include(e => e.RE);
            return View(emailArchives.ToList());
        }

        // GET: EmailArchive/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailArchive emailArchive = db.EmailArchives.Find(id);
            if (emailArchive == null)
            {
                return HttpNotFound();
            }
            return View(emailArchive);
        }

        // GET: EmailArchive/Create
        public ActionResult Create()
        {
            ViewBag.REID = new SelectList(db.REs, "ID", "contactPersonName");
            return View();
        }

        // POST: EmailArchive/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "emailID,emailDate,reason,REID")] EmailArchive emailArchive)
        {
            if (ModelState.IsValid)
            {
                db.EmailArchives.Add(emailArchive);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.REID = new SelectList(db.REs, "ID", "contactPersonName", emailArchive.REID);
            return View(emailArchive);
        }

        // GET: EmailArchive/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailArchive emailArchive = db.EmailArchives.Find(id);
            if (emailArchive == null)
            {
                return HttpNotFound();
            }
            ViewBag.REID = new SelectList(db.REs, "ID", "contactPersonName", emailArchive.REID);
            return View(emailArchive);
        }

        // POST: EmailArchive/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "emailID,emailDate,reason,REID")] EmailArchive emailArchive)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emailArchive).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.REID = new SelectList(db.REs, "ID", "contactPersonName", emailArchive.REID);
            return View(emailArchive);
        }

        // GET: EmailArchive/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailArchive emailArchive = db.EmailArchives.Find(id);
            if (emailArchive == null)
            {
                return HttpNotFound();
            }
            return View(emailArchive);
        }

        // POST: EmailArchive/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            EmailArchive emailArchive = db.EmailArchives.Find(id);
            db.EmailArchives.Remove(emailArchive);
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
