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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string id, string password)
        {
            var eoUser = db.EOs.FirstOrDefault(e => e.ID == id);

            if (eoUser != null && password == "password")
            {
                Session["EOUserID"] = eoUser.ID;
                Session["EOUserName"] = eoUser.Name;

                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid EO ID or password.";
            return View();
        }

        public ActionResult Dashboard()
        {
            if (Session["EOUserID"] == null)
            {
                return RedirectToAction("Login");
            }

            var allRequests = db.PermitRequests
                .Include(p => p.EnvironmentalPermit)
                .Include(p => p.RE)
                .ToList();

            var reviewQueue = new List<PermitRequest>();

            foreach (var req in allRequests)
            {
                var latestStatus = db.RequestStatus
                    .Where(rs => rs.requestID == req.requestNo)
                    .ToList()
                    .OrderByDescending(rs =>
                        rs.permitRequestStatus.StartsWith("Permit Issued") ? 6 :
                        rs.permitRequestStatus.StartsWith("Accepted") ? 5 :
                        rs.permitRequestStatus.StartsWith("Rejected") ? 4 :
                        rs.permitRequestStatus.StartsWith("Being Reviewed") ? 3 :
                        rs.permitRequestStatus.StartsWith("Submitted") ? 2 :
                        rs.permitRequestStatus.StartsWith("Pending Payment") ? 1 : 0)
                    .ThenByDescending(rs => rs.date)
                    .FirstOrDefault();

                if(latestStatus != null &&
                   (latestStatus.permitRequestStatus.StartsWith("Submitted") ||
                    latestStatus.permitRequestStatus.StartsWith("Being Reviewed")))
{
                    reviewQueue.Add(req);
                }
            }

            return View(reviewQueue);
        }

        public ActionResult Logout()
        {
            Session["EOUserID"] = null;
            Session["EOUserName"] = null;
            return RedirectToAction("Login");
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