using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Group10_Project.Models;

namespace Group10_Project.Controllers
{
    public class PaymentsController : Controller
    {
        private Group10_DBEntities db = new Group10_DBEntities();

        // GET: Payments
        public ActionResult Index()
        {
            return View(db.Payments.ToList());
        }

        // GET: Payments/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create?requestId=001
        public ActionResult Create(string requestId)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "REs");
            }

            if (string.IsNullOrEmpty(requestId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Request ID is required.");
            }

            var permitRequest = db.PermitRequests
                .Include(p => p.EnvironmentalPermit)
                .FirstOrDefault(p => p.requestNo == requestId);

            if (permitRequest == null)
            {
                return HttpNotFound();
            }

            // Optional security check: ensure logged-in RE owns this request
            string currentUserId = Session["UserID"].ToString();
            if (permitRequest.permitREID != currentUserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            ViewBag.RequestId = permitRequest.requestNo;
            ViewBag.PermitName = permitRequest.EnvironmentalPermit != null
                ? permitRequest.EnvironmentalPermit.permitName
                : permitRequest.permitTypeID;
            ViewBag.PermitFee = permitRequest.permitFee;

            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string requestId, [Bind(Include = "paymentMethod,last4CardDigit,cardHolderName")] Payment payment)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "REs");
            }

            if (string.IsNullOrEmpty(requestId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Request ID is required.");
            }

            var permitRequest = db.PermitRequests
                .Include(p => p.EnvironmentalPermit)
                .FirstOrDefault(p => p.requestNo == requestId);

            if (permitRequest == null)
            {
                return HttpNotFound();
            }

            string currentUserId = Session["UserID"].ToString();
            if (permitRequest.permitREID != currentUserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (ModelState.IsValid)
            {
                // Auto-generate payment system fields
                payment.paymentID = "PAY_" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
                payment.paymentDate = DateTime.Now;
                payment.paymentApproved = true;

                db.Payments.Add(payment);
                db.SaveChanges();

                // Link payment to permit request
                permitRequest.permitPayment = payment.paymentID;
                db.Entry(permitRequest).State = EntityState.Modified;

                DateTime today = DateTime.Today;

                var paymentStatus = new RequestStatus
                {
                    permitRequestStatus = "Submitted - " + permitRequest.requestNo,
                    date = today,
                    description = "Payment received successfully. Application submitted.",
                    requestID = permitRequest.requestNo
                };

                db.RequestStatus.Add(paymentStatus);

                // Optional acknowledgement/email archive entry
                var emailArchive = new EmailArchive
                {
                    emailID = "EMAIL_" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                    emailDate = DateTime.Now,
                    reason = "Payment received and application submitted.",
                    REID = currentUserId
                };

                db.EmailArchives.Add(emailArchive);

                db.SaveChanges();

                return RedirectToAction("Dashboard", "REs");
            }

            ViewBag.RequestId = permitRequest.requestNo;
            ViewBag.PermitName = permitRequest.EnvironmentalPermit != null
                ? permitRequest.EnvironmentalPermit.permitName
                : permitRequest.permitTypeID;
            ViewBag.PermitFee = permitRequest.permitFee;

            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }

            return View(payment);
        }

        // POST: Payments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "paymentID,paymentDate,paymentMethod,last4CardDigit,cardHolderName,paymentApproved")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
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