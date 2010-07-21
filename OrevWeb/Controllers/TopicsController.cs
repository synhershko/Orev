using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Orev.Domain;
using Orev.Helpers;
using Orev.Common;
using OrevWeb.Helpers;

using TopicsRepository = Orev.Repositories.NHibernateRepository<Orev.Domain.Topic>;

namespace Orev.Controllers
{
    public class TopicsController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List(string lang)
        {
            if (lang == null)
                return RedirectToAction("Languages");

            IEnumerable<Topic> l;
            using (var rep = new TopicsRepository())
            {
                l = rep.All(T => T.Language.Equals(lang));
            }

            if (l == null || l.Count() ==0)
                return RedirectToAction("Create", new { lang = lang });

            return View(l);
        }

        public ActionResult Languages()
        {
            using (var rep = new TopicsRepository())
            {
                IEnumerable<Topic> l = rep.Distinct(new TopicLanguageEqualityComparer());
                if (l == null || l.Count() == 0)
                    ViewData["Message"] = "No languages were found. Please add a new topic";
                else
                {
                    string[] s = new string[l.Count()];
                    for (int i=0; i<l.Count(); i++)
                    {
                        s[i] = l.ElementAt(i).Language;
                    }
                    ViewData["LanguagesList"] = s;
                }
            }

            return View();
        }

        //
        // GET: /Topics/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create(string lang, Topic t)
        {
            if (t == null)
                t = new Topic();

            if (lang != null && lang.Length == 5)
                t.Language = lang;

            return View(t);
        }

        [HttpPost]
        public ActionResult Create(string lang, FormCollection collection)
        {
            Topic t = new Topic();
            try
            {
                string[] allowedProperties = new[] { "Title", "Description", "Narrator", "Language" };
                UpdateModel<Topic>(t, allowedProperties);

                using (var rep = new TopicsRepository())
                {
                    rep.Add(t);
                }
                return RedirectToAction("List", new { lang = collection["Language"] });
            }
            catch
            {
                return View(t);
            }
        }
        
        public ActionResult Edit(int id)
        {
            Topic t;
            using (var rep = new TopicsRepository())
            {
                t = rep.SingleOrDefault(T => T.Id == id);
            }

            if (t == null)
                ViewData["Message"] = "The requested topic was not found";
            else
                ViewData["EditMode"] = true;

            return View("Create", t);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Topic t;
                using (var rep = new TopicsRepository())
                {
                    t = rep.SingleOrDefault(T => T.Id == id);
                }

                return RedirectToAction("List", new { lang = t.Language });
            }
            catch
            {
                return View("List");
            }
        }

        //
        // GET: /Topics/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Topics/Delete/5

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
        }
    }
}
