using System.Linq;
using System.Web.Mvc;
using Orev.Models;

namespace Orev.Controllers
{
    public class CorporaController : RavenController
    {
        //
        // GET: /Corpora/

        public ActionResult Index()
        {
        	var corpora = RavenSession.Query<Corpus>().ToArray();
            return View(corpora);
        }

		[HttpGet]
		[Authorize]
		public ActionResult Add(string lang)
		{
			return View("Edit", new Corpus {Language = lang});
		}

		[HttpGet]
		[Authorize]
		public ActionResult Edit(int id)
		{
			var corpus = RavenSession.Load<Corpus>(id);
			if (corpus == null)
				return HttpNotFound("The requested corpus does not exist.");
			return View(corpus);
		}

		[HttpPost]
		[Authorize]
		public ActionResult Update(Corpus input)
		{
			if (!ModelState.IsValid)
				return View("Edit", input);

			var corpus = RavenSession.Load<Corpus>(input.Id);
			if (corpus != null)
			{
				corpus.Description = input.Description;
				corpus.Language = input.Language;
				corpus.Name = input.Name;
			}
			else
			{
				RavenSession.Store(input);
			}

			return RedirectToAction("Index");
		}
    }
}
