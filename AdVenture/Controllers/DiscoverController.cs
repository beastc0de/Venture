using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdVenture.Models;
using System.Net;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;



namespace AdVenture.Controllers
{

    public class DiscoverController : Controller
    {
        private VentureCapitalDbContext db = new VentureCapitalDbContext();
        
        // GET: Discover
        public ActionResult Index()
        {
            ApplicationUser currentUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
            var ventures = from v in db.Ventures where v.investorID != currentUser.Id select v;
            return View(ventures.ToList());
        }

        public ActionResult GridIndex()
        {
            ApplicationUser currentUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
            var ventures = from v in db.Ventures where v.investorID != currentUser.Id select v;
            return View(ventures.ToList());
        }
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
    }
}