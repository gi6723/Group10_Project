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
    public class OPS_CPPController : Controller
    {
        private Group10_DBEntities db = new Group10_DBEntities();

        // GET: OPS_CPP
        public ActionResult Index()
        {
            return View(db.OPS_CPP.ToList());
        }

        // GET: OPS_CPP/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPS_CPP oPS_CPP = db.OPS_CPP.Find(id);
            if (oPS_CPP == null)
            {
                return HttpNotFound();
            }
            return View(oPS_CPP);
        }

        // GET: OPS_CPP/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OPS_CPP/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] OPS_CPP oPS_CPP)
        {
            if (ModelState.IsValid)
            {
                db.OPS_CPP.Add(oPS_CPP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oPS_CPP);
        }

        // GET: OPS_CPP/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPS_CPP oPS_CPP = db.OPS_CPP.Find(id);
            if (oPS_CPP == null)
            {
                return HttpNotFound();
            }
            return View(oPS_CPP);
        }

        // POST: OPS_CPP/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] OPS_CPP oPS_CPP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oPS_CPP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oPS_CPP);
        }

        // GET: OPS_CPP/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPS_CPP oPS_CPP = db.OPS_CPP.Find(id);
            if (oPS_CPP == null)
            {
                return HttpNotFound();
            }
            return View(oPS_CPP);
        }

        // POST: OPS_CPP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OPS_CPP oPS_CPP = db.OPS_CPP.Find(id);
            db.OPS_CPP.Remove(oPS_CPP);
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
