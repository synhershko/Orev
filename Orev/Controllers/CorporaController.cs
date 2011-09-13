using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Orev.Helpers;
using Orev.Models;

namespace Orev.Controllers
{
    public class CorporaController : RavenController
    {
        //
        // GET: /Corpora/

        public ActionResult Index()
        {
        	var corpora = RavenSession.Query<Corpus>().Customize(x => x.WaitForNonStaleResultsAsOfLastWrite()).ToArray();
            return View(corpora);
        }

		[HttpGet]
		[Authorize]
		public ActionResult Add(string lang)
		{
			return View("Edit", new Corpus { Language = (string.IsNullOrWhiteSpace(lang) ? "en-US" : lang) });
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

			var corpus = string.IsNullOrWhiteSpace(input.Id) ? null : RavenSession.Load<Corpus>(input.Id);
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

		[HttpGet]
		public ActionResult Details(int id)
		{
			var corpus = RavenSession.Load<Corpus>(id);
			if (corpus == null)
				return HttpNotFound();

			ViewBag.IntId = id;

			return View(corpus);
		}

		[HttpGet]
		[Authorize]
		public ActionResult FeedDocuments(int corpusId)
		{
			var corpus = RavenSession.Load<Corpus>(corpusId);
			if (corpus == null)
				return HttpNotFound();

			return View(new CorpusFeedingInput {CorpusId = corpus.Id});
		}

		[HttpPost]
		[Authorize]
		public ActionResult FeedDocuments(CorpusFeedingInput input)
		{
			var user = RavenSession.GetUser(User.Identity.Name);
			if (user == null || user.Role != Models.User.OperationRoles.Admin)
				return HttpForbidden();

			if (!ModelState.IsValid)
				return View("FeedDocuments", input);

			var corpus = RavenSession.Load<Corpus>(input.CorpusId);
			if (corpus == null)
				return HttpNotFound("The requested corpus does not exist.");
			return View("Index");
		}
    }
}
