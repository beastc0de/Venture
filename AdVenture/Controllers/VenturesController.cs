using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdVenture.Models;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet;


namespace AdVenture.Controllers
{ 
public class VenturesController : Controller
    {
        private VentureCapitalDbContext db = new VentureCapitalDbContext();
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Ventures
        public ActionResult Index()
        {
            return View(db.Ventures.ToList());
        }

        // GET: Ventures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venture venture = db.Ventures.Find(id);
            if (venture == null)
            {
                return HttpNotFound();
            }
            return View(venture);
        }

        // GET: Ventures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ventures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyName,CompanyDescription,OwnerName,CapitalRaised,Ask,createdOn,Sector,Verified")] Venture venture)
        {
            if (ModelState.IsValid)
            {
                venture.createdOn = DateTime.Now;
                ApplicationUser currentUser = _context.Users.Single(u => u.Id == HttpContext.User.Identity.GetUserId());
                //currentUser.investor.PersonalInvestments.Add(venture); 
                db.Ventures.Add(venture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(venture);
        }

        // GET: Ventures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venture venture = db.Ventures.Find(id);
            if (venture == null)
            {
                return HttpNotFound();
            }
            return View(venture);
        }

        // POST: Ventures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyName,CompanyDescription,OwnerName,CapitalRaised,Ask,createdOn,Sector,Verified")] Venture venture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(venture);
        }

        // GET: Ventures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venture venture = db.Ventures.Find(id);
            if (venture == null)
            {
                return HttpNotFound();
            }
            return View(venture);
        }

        // POST: Ventures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Venture venture = db.Ventures.Find(id);
            db.Ventures.Remove(venture);
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

