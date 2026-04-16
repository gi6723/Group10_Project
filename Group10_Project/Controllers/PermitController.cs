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
            // 1. Get the base query (don't execute .ToList() yet!)
            var permits = db.Permits
                .Include(p => p.EO)
                .Include(p => p.RE)
                .Include(p => p.PermitRequest);

            // 2. Check if an EO is logged in
            if (Session["EOID"] != null)
            {
                // EO sees everything - no filtering needed
                return View(permits.ToList());
            }

            // 3. Check if an RE is logged in
            if (Session["UserID"] != null)
            {
                string currentREID = Session["UserID"].ToString();

                // Filter: Only permits where the REID matches the logged-in user
                var myPermits = permits.Where(p => p.issuedTo == currentREID).ToList();

                return View(myPermits);
            }

            // 4. If all else fails return to dashboard
            return View("Dashboard");
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

        public ActionResult IssuedPermit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Search by the Foreign Key 'relatedTo' instead of the Primary Key
            var permit = db.Permits.FirstOrDefault(p => p.relatedTo == id);

            if (permit == null)
            {
                // This handles cases where a permit hasn't been issued yet
                return HttpNotFound("No permit record found for this request.");
            }

            return View("Details", permit);
        }

        // GET: Permit/Issue?requestId=REQ_123ABC
        public ActionResult Issue(string requestId)
        {
            if (Session["EOUserID"] == null)
            {
                return RedirectToAction("Login", "EO");
            }

            if (string.IsNullOrEmpty(requestId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Request ID is required.");
            }

            var permitRequest = db.PermitRequests
                .Include(p => p.RE)
                .Include(p => p.EnvironmentalPermit)
                .FirstOrDefault(p => p.requestNo == requestId);

            if (permitRequest == null)
            {
                return HttpNotFound();
            }

            var latestStatus = db.RequestStatus
                .Where(rs => rs.requestID == permitRequest.requestNo)
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

            if (latestStatus == null || !latestStatus.permitRequestStatus.StartsWith("Accepted"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Only accepted requests can be issued.");
            }

            var existingPermit = db.Permits.FirstOrDefault(p => p.relatedTo == permitRequest.requestNo);
            if (existingPermit != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "A permit has already been issued for this request.");
            }

            ViewBag.RequestId = permitRequest.requestNo;
            ViewBag.REName = permitRequest.RE != null ? permitRequest.RE.contactPersonName : permitRequest.permitREID;
            ViewBag.PermitName = permitRequest.EnvironmentalPermit != null
                ? permitRequest.EnvironmentalPermit.permitName
                : permitRequest.permitTypeID;
            ViewBag.ActivityDescription = permitRequest.activityDescription;

            return View();
        }

        // POST: Permit/Issue
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Issue(string requestId, string duration)
        {
            if (Session["EOUserID"] == null)
            {
                return RedirectToAction("Login", "EO");
            }

            if (string.IsNullOrEmpty(requestId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Request ID is required.");
            }

            var permitRequest = db.PermitRequests
                .Include(p => p.RE)
                .FirstOrDefault(p => p.requestNo == requestId);

            if (permitRequest == null)
            {
                return HttpNotFound();
            }

            var latestStatus = db.RequestStatus
                .Where(rs => rs.requestID == permitRequest.requestNo)
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

            if (latestStatus == null || !latestStatus.permitRequestStatus.StartsWith("Accepted"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Only accepted requests can be issued.");
            }

            var existingPermit = db.Permits.FirstOrDefault(p => p.relatedTo == permitRequest.requestNo);
            if (existingPermit != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "A permit has already been issued for this request.");
            }

            string currentEOId = Session["EOUserID"].ToString();

            var finalPermit = new Permit
            {
                permitID = "P_" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                dateOfIssue = DateTime.Now,
                duration = string.IsNullOrWhiteSpace(duration) ? "1 Year" : duration,
                description = permitRequest.activityDescription,
                issuedBy = currentEOId,
                issuedTo = permitRequest.permitREID,
                relatedTo = permitRequest.requestNo
            };

            db.Permits.Add(finalPermit);

            var issuedStatus = new RequestStatus
            {
                permitRequestStatus = "Permit Issued - " + permitRequest.requestNo,
                date = DateTime.Today,
                description = "Permit issued by EO.",
                requestID = permitRequest.requestNo
            };

            db.RequestStatus.Add(issuedStatus);

            var emailArchive = new EmailArchive
            {
                emailID = "EMAIL_" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                emailDate = DateTime.Now,
                reason = "Permit issued for accepted request.",
                REID = permitRequest.permitREID
            };

            db.EmailArchives.Add(emailArchive);

            db.SaveChanges();

            return RedirectToAction("Dashboard", "EO");
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