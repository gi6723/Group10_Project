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
    public class REsController : Controller
    {
        private Group10_DBEntities db = new Group10_DBEntities();

        // GET: REs
        public ActionResult Index()
        {
            var rEs = db.REs.Include(r => r.RESite);
            return View(rEs.ToList());
        }

        // GET: REs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RE rE = db.REs.Find(id);
            if (rE == null)
            {
                return HttpNotFound();
            }
            return View(rE);
        }

        // GET: REs/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.RESites, "ID", "siteAddress");
            return View();
        }

        // POST: REs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RE re, string siteAddress, string siteContactPerson)
        {
            if (ModelState.IsValid)
            {
                re.createdDate = DateTime.Now;
                db.REs.Add(re);
                // 1. Save the RE (The User Account) first
                db.REs.Add(re);
                db.SaveChanges(); // This pushes the RE to the DB and generates the ID

                // 2. Now create the Site and link it to the RE we just saved
                var newSite = new RESite();

                newSite.siteAddress = siteAddress;
                newSite.siteContactPerson = siteContactPerson;

                // This is the "Bridge" - link the Site to the RE's ID
                // Check your RESite.cs file to see if this is named RE_ID or ID
                newSite.ID = re.ID;

                db.RESites.Add(newSite);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(re);
        }

        // GET: REs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RE rE = db.REs.Find(id);
            if (rE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.RESites, "ID", "siteAddress", rE.ID);
            return View(rE);
        }

        // POST: REs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,contactPersonName,password,createdDate,email,organizationName,organizationAddress")] RE rE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.RESites, "ID", "siteAddress", rE.ID);
            return View(rE);
        }

        // GET: REs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RE rE = db.REs.Find(id);
            if (rE == null)
            {
                return HttpNotFound();
            }
            return View(rE);
        }

        // POST: REs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RE rE = db.REs.Find(id);
            db.REs.Remove(rE);
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

        public ActionResult Login()
        {
            return View();
        }
    }
}
