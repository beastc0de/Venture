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
    public class OffersController : Controller
    {
        private VentureCapitalDbContext db = new VentureCapitalDbContext();

        // GET: Offers
        public ActionResult Index()
        {
            
            ApplicationUser currentUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
            var userVentures = from v in db.Ventures where v.investorID == currentUser.Id select v;
            var query = from b in db.Bids where b.status == "pending" join v in userVentures on b.ventureID equals v.Id  select b;
            
            return View(query.ToList());
        }

        // GET: Offers/Details/5
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

        
        public ActionResult Accept(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bids bid = db.Bids.Find(id);
            db.Bids.Find(id).status = "accepted";
            db.SaveChanges();
           
            if (bid == null)
            {
                return HttpNotFound();
            }
            
            return View();
        }

        public ActionResult Deny(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bids bid = db.Bids.Find(id);
            db.Bids.Find(id).status = "denied";
            db.SaveChanges();
            if (bid == null)
            {
                return HttpNotFound();
            }
            
            return View(bid);
        }

        // GET: Offers/Delete/5
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

        // POST: Offers/Delete/5
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
