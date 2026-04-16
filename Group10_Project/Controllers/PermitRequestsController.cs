using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Group10_Project.Models;

namespace Group10_Project.Controllers
{
    public class PermitRequestsController : Controller
    {
        private Group10_DBEntities db = new Group10_DBEntities();

        // GET: PermitRequests
        public ActionResult Index()
        {
            var permitRequests = db.PermitRequests
                .Include(p => p.EnvironmentalPermit)
                .Include(p => p.Payment)
                .Include(p => p.RE);

            return View(permitRequests.ToList());
        }

        // GET: PermitRequests/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PermitRequest permitRequest = db.PermitRequests.Find(id);
            if (permitRequest == null)
            {
                return HttpNotFound();
            }

            return View(permitRequest);
        }

        // GET: PermitRequests/Create
        public ActionResult Create()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "REs");
            }

            ViewBag.permitTypeID = new SelectList(db.EnvironmentalPermits, "permitID", "permitName");
            return View();
        }

        // POST: PermitRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "activityDescription,activityStartDate,activityDuration,permitTypeID")] PermitRequest permitRequest)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "REs");
            }

            string currentUserId = Session["UserID"].ToString();

            if (ModelState.IsValid)
            {
                // Auto-generate request number
                string newRequestNo;
                do
                {
                    newRequestNo = "REQ_" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                }
                while (db.PermitRequests.Any(p => p.requestNo == newRequestNo));

                permitRequest.requestNo = newRequestNo;

                // Set the logged-in RE automatically
                permitRequest.permitREID = currentUserId;

                // Set request date automatically
                permitRequest.dateOfRequest = DateTime.Now;

                // No payment yet on initial creation
                permitRequest.permitPayment = null;

                // Pull permit fee from selected permit type
                var selectedPermit = db.EnvironmentalPermits.FirstOrDefault(p => p.permitID == permitRequest.permitTypeID);
                if (selectedPermit != null)
                {
                    permitRequest.permitFee = selectedPermit.permitFee;
                }

                db.PermitRequests.Add(permitRequest);
                db.SaveChanges();

                // Because RequestStatus has a flawed composite PK (permitRequestStatus + date),
                // make the initial status unique per request.
                DateTime today = DateTime.Today;
                var uniquePendingStatus = "Pending Payment - " + permitRequest.requestNo;

                var initialStatus = new RequestStatus
                {
                    permitRequestStatus = uniquePendingStatus,
                    date = today,
                    description = "Application submitted and awaiting payment.",
                    requestID = permitRequest.requestNo
                };

                db.RequestStatus.Add(initialStatus);
                db.SaveChanges();

                return RedirectToAction("Dashboard", "REs");
            }

            ViewBag.permitTypeID = new SelectList(db.EnvironmentalPermits, "permitID", "permitName", permitRequest.permitTypeID);
            return View(permitRequest);
        }

        // GET: PermitRequests/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PermitRequest permitRequest = db.PermitRequests.Find(id);
            if (permitRequest == null)
            {
                return HttpNotFound();
            }

            ViewBag.permitTypeID = new SelectList(db.EnvironmentalPermits, "permitID", "permitName", permitRequest.permitTypeID);
            ViewBag.permitPayment = new SelectList(db.Payments, "paymentID", "paymentMethod", permitRequest.permitPayment);
            ViewBag.permitREID = new SelectList(db.REs, "ID", "contactPersonName", permitRequest.permitREID);
            return View(permitRequest);
        }

        // POST: PermitRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "requestNo,dateOfRequest,activityDescription,activityStartDate,activityDuration,permitFee,permitTypeID,permitREID,permitPayment")] PermitRequest permitRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(permitRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.permitTypeID = new SelectList(db.EnvironmentalPermits, "permitID", "permitName", permitRequest.permitTypeID);
            ViewBag.permitPayment = new SelectList(db.Payments, "paymentID", "paymentMethod", permitRequest.permitPayment);
            ViewBag.permitREID = new SelectList(db.REs, "ID", "contactPersonName", permitRequest.permitREID);
            return View(permitRequest);
        }

        // GET: PermitRequests/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PermitRequest permitRequest = db.PermitRequests.Find(id);
            if (permitRequest == null)
            {
                return HttpNotFound();
            }

            return View(permitRequest);
        }

        // POST: PermitRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PermitRequest permitRequest = db.PermitRequests.Find(id);
            db.PermitRequests.Remove(permitRequest);
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