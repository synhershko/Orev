using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orev.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Install()
        {
            OrevWeb.Helpers.NHibernateHelpers.GenerateSchema();
            ViewData["Message"] = "Installation completed successfully";
            return View("Index");
        }

        public ActionResult UpdateSchema()
        {
            OrevWeb.Helpers.NHibernateHelpers.UpdateSchema();
            ViewData["Message"] = "Schema updated successfully";
            return View("Index");
        }
    }
}
