using Group10_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RE re, string siteAddress, string siteContactPerson)
        {
            if (ModelState.IsValid)
            {
                var existingRE = db.REs.FirstOrDefault(u => u.ID == re.ID);

                if (existingRE != null)
                {
                    ModelState.AddModelError("ID", "This email is already registered. Please login or use a different ID.");
                    return View(re);
                }

                re.createdDate = DateTime.Now;
                db.REs.Add(re);
                db.SaveChanges();

                var newSite = new RESite
                {
                    siteAddress = siteAddress,
                    siteContactPerson = siteContactPerson,
                    ID = re.ID
                };

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

        public ActionResult Dashboard()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }

            string id = Session["UserID"].ToString();

            var re = db.REs.Find(id);
            if (re == null)
            {
                return HttpNotFound();
            }

            var permitRequests = db.PermitRequests
                .Include(p => p.EnvironmentalPermit)
                .Where(p => p.permitREID == id)
                .ToList();

            var model = new REDashboardViewModel
            {
                REID = re.ID,
                ContactPersonName = re.contactPersonName,
                Email = re.email,
                OrganizationName = re.organizationName,
                OrganizationAddress = re.organizationAddress
            };

            foreach (var req in permitRequests)
            {
                var latestStatus = db.RequestStatus
                    .Where(rs => rs.requestID == req.requestNo)
                    .ToList()
                    .OrderByDescending(rs =>
                        rs.permitRequestStatus == "Issued" ? 6 :
                        rs.permitRequestStatus.StartsWith("Approved") ? 5 :
                        rs.permitRequestStatus.StartsWith("Rejected") ? 4 :
                        rs.permitRequestStatus.StartsWith("Being Reviewed") ? 3 :
                        rs.permitRequestStatus == "Submitted" ? 2 :
                        rs.permitRequestStatus.StartsWith("Pending Payment") ? 1 : 0)
                    .ThenByDescending(rs => rs.date)
                    .FirstOrDefault();

                string displayStatus = "Pending Payment";

                if (latestStatus != null)
                {
                    if (latestStatus.permitRequestStatus.StartsWith("Pending Payment"))
                    {
                        displayStatus = "Pending Payment";
                    }
                    else if (latestStatus.permitRequestStatus.StartsWith("Being Reviewed"))
                    {
                        displayStatus = "Being Reviewed";
                    }
                    else if (latestStatus.permitRequestStatus.StartsWith("Approved"))
                    {
                        displayStatus = "Approved";
                    }
                    else if (latestStatus.permitRequestStatus.StartsWith("Rejected"))
                    {
                        displayStatus = "Rejected";
                    }
                    else
                    {
                        displayStatus = latestStatus.permitRequestStatus;
                    }
                }

                var item = new REPermitRequestItemViewModel
                {
                    RequestNo = req.requestNo,
                    PermitName = req.EnvironmentalPermit != null ? req.EnvironmentalPermit.permitName : req.permitTypeID,
                    DateOfRequest = req.dateOfRequest,
                    ActivityDescription = req.activityDescription,
                    ActivityStartDate = req.activityStartDate,
                    ActivityDuration = req.activityDuration,
                    PermitFee = req.permitFee,

                    CurrentStatus = displayStatus,
                    StatusDate = latestStatus != null ? (DateTime?)latestStatus.date : null,
                    StatusDescription = latestStatus != null ? latestStatus.description : "No status recorded yet.",

                    CanPay = latestStatus == null || latestStatus.permitRequestStatus.StartsWith("Pending Payment"),
                    CanViewPermit = latestStatus != null && latestStatus.permitRequestStatus == "Issued",
                    CanViewDecision = latestStatus != null &&
                                      (latestStatus.permitRequestStatus.StartsWith("Approved") ||
                                       latestStatus.permitRequestStatus.StartsWith("Rejected"))
                };

                model.Requests.Add(item);
            }

            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string id, string password)
        {
            var user = db.REs.FirstOrDefault(u => u.ID == id && u.password == password);

            if (user != null)
            {
                Session["UserID"] = user.ID;
                Session["UserName"] = user.organizationName;

                return RedirectToAction("Dashboard", "REs");
            }
            else
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
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

        public ActionResult PasswordChangeView()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            return View();
        }
        [HttpPost]
        public ActionResult PasswordChangeView(string currentPassword, string newPassword, string confirmPassword)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            // Check confirm password
            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "New passwords do not match";
                return View();
            }

            using (var db = new Group10_DBEntities())
            {
                string userId = Session["UserId"].ToString();

                var user = db.REs.FirstOrDefault(u => u.ID == userId);

                if (user == null)
                    return HttpNotFound();

                // Check current password
                if (user.password != currentPassword)
                {
                    ViewBag.Error = "Current password is incorrect";
                    return View();
                }

                // Update password
                user.password = newPassword;
                db.SaveChanges();
            }

            ViewBag.Message = "Password updated successfully!";
            return View();
        }
    }
}