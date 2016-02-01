using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdVenture.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace AdVenture.Controllers
{
    public class BidsController : Controller
    {
        private VentureCapitalDbContext db = new VentureCapitalDbContext();

        // GET: Bids
        public ActionResult Index()
        {
            ApplicationUser currentUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
            var query = from b in db.Bids where b.investorID == currentUser.Id && b.status != "complete" select b;
            return View(query.ToList());
        }

        // GET: Bids/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bids bids = db.Bids.Find(id);
            if (bids == null)
            {
                return HttpNotFound();
            }
            return View(bids);
        }

        // GET: Bids/Create
        public ActionResult Create(int id)
        {
            Venture venture = db.Ventures.Find(id);
            ViewData["ventureID"] = venture.Id;
            
            return View();
        }

        // POST: Bids/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,investorID,ventureID,Company,bid,bidStake,createdOn,status")] Bids bids)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
                bids.createdOn = DateTime.Now;
                bids.status = "pending";
                bids.investorID = currentUser.Id;
                bids.ventureID = (int)TempData["ventureId"];
                bids.Company = db.Ventures.Find(bids.ventureID).CompanyName;
                db.Bids.Add(bids);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bids);
        }

        // GET: Bids/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bids bids = db.Bids.Find(id);
            if (bids == null)
            {
                return HttpNotFound();
            }
            return View(bids);
        }

        // POST: Bids/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,investorID,ventureID,bid,bidStake,createdOn,status")] Bids bids)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bids).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bids);
        }

        // GET: Bids/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bids bids = db.Bids.Find(id);
            if (bids == null)
            {
                return HttpNotFound();
            }
            return View(bids);
        }

        // POST: Bids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bids bids = db.Bids.Find(id);
            db.Bids.Remove(bids);
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
