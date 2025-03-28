using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QueroSugar.Controllers
{
    public class AppController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult profile()
        {
            return View();
        }
    }
}