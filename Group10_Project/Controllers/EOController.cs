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
    public class EOController : Controller
    {
        private Group10_DBEntities db = new Group10_DBEntities();

        // GET: EO
        public ActionResult Index()
        {
            return View(db.EOs.ToList());
        }

        // GET: EO/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EO eO = db.EOs.Find(id);
            if (eO == null)
            {
                return HttpNotFound();
            }
            return View(eO);
        }

        // GET: EO/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EO/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] EO eO)
        {
            if (ModelState.IsValid)
            {
                db.EOs.Add(eO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eO);
        }

        // GET: EO/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EO eO = db.EOs.Find(id);
            if (eO == null)
            {
                return HttpNotFound();
            }
            return View(eO);
        }

        // POST: EO/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] EO eO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eO);
        }

        // GET: EO/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EO eO = db.EOs.Find(id);
            if (eO == null)
            {
                return HttpNotFound();
            }
            return View(eO);
        }

        // POST: EO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            EO eO = db.EOs.Find(id);
            db.EOs.Remove(eO);
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
