using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdVenture.Models;
using System.Net;



namespace AdVenture.Controllers
{

    public class DiscoverController : Controller
    {
        private VentureCapitalDbContext db = new VentureCapitalDbContext();
        // GET: Discover
        public ActionResult Index()
        {
            return View(db.Ventures.ToList());
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