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

        // Helper methods for EO password management
        private string GetEOPasswordFilePath()
        {
            return Server.MapPath("~/App_Data/eo_password.txt");
        }

        private string GetCurrentEOPassword()
        {
            string path = GetEOPasswordFilePath();

            if (!System.IO.File.Exists(path))
            {
                System.IO.File.WriteAllText(path, "password");
            }

            return System.IO.File.ReadAllText(path).Trim();
        }

        private void SaveCurrentEOPassword(string newPassword)
        {
            string path = GetEOPasswordFilePath();
            System.IO.File.WriteAllText(path, newPassword.Trim());
        }

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

            if (eoUser != null && password == GetCurrentEOPassword())
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
            var acceptedQueue = new List<PermitRequest>();

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

                if (latestStatus == null)
                {
                    continue;
                }

                if (latestStatus.permitRequestStatus.StartsWith("Submitted") ||
                    latestStatus.permitRequestStatus.StartsWith("Being Reviewed"))
                {
                    reviewQueue.Add(req);
                }
                else if (latestStatus.permitRequestStatus.StartsWith("Accepted"))
                {
                    var existingPermit = db.Permits.FirstOrDefault(p => p.relatedTo == req.requestNo);

                    if (existingPermit == null)
                    {
                        acceptedQueue.Add(req);
                    }
                }
            }

            ViewBag.AcceptedQueue = acceptedQueue;

            return View(reviewQueue);
        }

        // GET: EO/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: EO/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string id, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(newPassword))
            {
                ViewBag.Error = "EO ID and new password are required.";
                return View();
            }

            var eoUser = db.EOs.FirstOrDefault(e => e.ID == id);

            if (eoUser == null)
            {
                ViewBag.Error = "EO ID not found.";
                return View();
            }

            SaveCurrentEOPassword(newPassword);
            ViewBag.Success = "Password updated successfully.";

            return View();
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