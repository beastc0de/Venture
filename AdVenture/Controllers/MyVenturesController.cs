using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdVenture.Controllers
{
    public class MyVenturesController : Controller
    {
        // GET: MyVentures
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}