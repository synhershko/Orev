using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orev.Domain;
using Orev.Repositories;

namespace Orev.Controllers
{
    public class CorporaController : Controller
    {
        //
        // GET: /Corpora/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(string lang, Corpus c)
        {
            if (c == null)
                c = new Corpus();

            if (lang != null && lang.Length == 5)
                c.Language = lang;

            return View(c);
        } 

        [HttpPost]
        public ActionResult Create(string lang, FormCollection collection)
        {
            Corpus c = new Corpus();
            try
            {
                string[] allowedProperties = new[] { "Name", "Description", "Language", "Path" };
                UpdateModel<Corpus>(c, allowedProperties);

                using (var rep = new CorporaRepository())
                {
                    rep.Add(c);
                }

                return RedirectToAction("Index", "Judge");
            }
            catch
            {
                return View(c);
            }
        }

        /*
        //
        // GET: /Corpora/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Corpora/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Corpora/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Corpora/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Corpora/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }*/
    }
}
