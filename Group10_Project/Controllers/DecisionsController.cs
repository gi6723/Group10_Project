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

        // GET: Decisions/Create?requestId=001
        public ActionResult Create(string requestId)
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
                .Include(p => p.EnvironmentalPermit)
                .Include(p => p.RE)
                .FirstOrDefault(p => p.requestNo == requestId);

            if (permitRequest == null)
            {
                return HttpNotFound();
            }

            ViewBag.RequestId = permitRequest.requestNo;
            ViewBag.PermitName = permitRequest.EnvironmentalPermit != null
                ? permitRequest.EnvironmentalPermit.permitName
                : permitRequest.permitTypeID;
            ViewBag.REName = permitRequest.RE != null
                ? permitRequest.RE.contactPersonName
                : permitRequest.permitREID;
            ViewBag.EOName = Session["EOUserName"] != null ? Session["EOUserName"].ToString() : "";

            var existingReviewStatus = db.RequestStatus
                .Where(rs => rs.requestID == permitRequest.requestNo)
                .ToList()
                .OrderByDescending(rs => rs.date)
                .FirstOrDefault(rs => rs.permitRequestStatus.StartsWith("Being Reviewed"));

            var latestStatus = db.RequestStatus
                .Where(rs => rs.requestID == permitRequest.requestNo)
                .ToList()
                .OrderByDescending(rs => rs.date)
                .FirstOrDefault();

            if (latestStatus != null &&
                latestStatus.permitRequestStatus.StartsWith("Submitted") &&
                existingReviewStatus == null)
            {
                var reviewStatus = new RequestStatus
                {
                    permitRequestStatus = "Being Reviewed - " + permitRequest.requestNo,
                    date = DateTime.Today,
                    description = "EO opened the application for review.",
                    requestID = permitRequest.requestNo
                };

                db.RequestStatus.Add(reviewStatus);
                db.SaveChanges();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string requestId, [Bind(Include = "finalDecision,description")] Decision decision)
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
                .Include(p => p.EnvironmentalPermit)
                .Include(p => p.RE)
                .FirstOrDefault(p => p.requestNo == requestId);

            if (permitRequest == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                string currentEOId = Session["EOUserID"].ToString();

                decision.ID = "DEC_" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
                decision.dateOfDecision = DateTime.Now;
                decision.EOID = currentEOId;
                decision.permitRequestID = permitRequest.requestNo;

                db.Decisions.Add(decision);
                db.SaveChanges();

                // Insert new unique status row
                DateTime today = DateTime.Today;
                string uniqueDecisionStatus = decision.finalDecision + " - " + permitRequest.requestNo;

                var newStatus = new RequestStatus
                {
                    permitRequestStatus = uniqueDecisionStatus,
                    date = today,
                    description = decision.description,
                    requestID = permitRequest.requestNo
                };

                db.RequestStatus.Add(newStatus);

                // Optional email/archive record
                var emailArchive = new EmailArchive
                {
                    emailID = "EMAIL_" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                    emailDate = DateTime.Now,
                    reason = "EO decision recorded: " + decision.finalDecision,
                    REID = permitRequest.permitREID
                };

                db.EmailArchives.Add(emailArchive);

                db.SaveChanges();

                // make actual permit if decision is approved
                if(decision.finalDecision.Equals("Accepted"))
                {
                    var finalPermit = new Permit
                    {
                        permitID = "P_" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                        dateOfIssue = DateTime.Now,
                        duration = "1 Year",
                        description = permitRequest.activityDescription,
                        issuedBy = currentEOId,
                        issuedTo = permitRequest.permitREID,
                        relatedTo = permitRequest.requestNo
                    };

                    db.Permits.Add(finalPermit);
                    db.SaveChanges();
                }

                return RedirectToAction("Dashboard", "EO");
            }

            ViewBag.RequestId = permitRequest.requestNo;
            ViewBag.PermitName = permitRequest.EnvironmentalPermit != null
                ? permitRequest.EnvironmentalPermit.permitName
                : permitRequest.permitTypeID;
            ViewBag.REName = permitRequest.RE != null
                ? permitRequest.RE.contactPersonName
                : permitRequest.permitREID;
            ViewBag.EOName = Session["EOUserName"] != null ? Session["EOUserName"].ToString() : "";

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
