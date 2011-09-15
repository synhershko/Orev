using System;
using System.Linq;
using System.Web.Mvc;
using Orev.Helpers;
using Orev.Infrastructure;
using Orev.Models;

namespace Orev.Controllers
{
    public class JudgeController : RavenController
    {
		[HttpGet]
		[Authorize]
		public ActionResult Index(int? topicId, int? corpusId, string lang)
		{
			// Go to topic selection if none selected
			if (topicId == null || topicId <= 0)
			{
				if (!string.IsNullOrEmpty(lang) || corpusId > 0)
					return RedirectToAction("SelectTopic", new {corpusId = corpusId, lang = lang});
				return RedirectToAction("SelectLanguage");
			}

			// Get the actual topic to get its language identifier
			var topic = RavenSession.Load<Topic>(topicId.Value);
			if (topic == null || string.IsNullOrWhiteSpace(topic.Language))
			{
				ViewBag.Message = "No such topic exists";
				return RedirectToAction("SelectTopic", new { corpusId = corpusId, lang = lang });
			}
			lang = topic.Language;
			ViewBag.Topic = topic;

			// Load the requested corpus if specified, failsafe below
			if (corpusId > 0)
			{
				var corpus = RavenSession.Load<Corpus>(corpusId.Value);
				if (corpus != null && lang.Equals(corpus.Language, StringComparison.InvariantCultureIgnoreCase))
				{
					ViewBag.Corpus = corpus;
				}
			}

			// If no corpus was selected, auto-select one matching the topic's language
			if (ViewBag.Corpus == null)
			{
				var corpus = RavenSession.Query<Corpus>()
					.Where(x => x.Language == lang)
					.FirstOrDefault();

				if (corpus == null)
				{
					ViewBag.Message = "No corpus was found for the selected language";
				}
				else
				{
					ViewBag.Corpus = corpus;
				}
				// Another option: return RedirectToAction("SelectCorpus", new { lang = lang });
			}

			return View(topic);
		}

		//[HttpPost]
		[AjaxOnly]
		[Authorize]
		public JsonResult SaveJudgment(string topicId, string corpusId, string docId, Judgment.Verdict verdict)
		{
			// TODO: input validation

			var j = new Judgment
			        	{
			        		CorpusId = corpusId,
			        		TopicId = topicId,
			        		DocumentId = docId,
			        		UserJudgement = verdict,
			        		UserId = "users/" + User.Identity.Name
			        	};

			RavenSession.Store(j);
			RavenSession.SaveChanges();

			var query = RavenSession.Advanced.LuceneQuery<CorpusDocuments_ByNextUnrated.ReduceResult, CorpusDocuments_ByNextUnrated>()
				.Where("Topics:*")
				.AndAlso()
				.Not
				.WhereEquals("Topics", topicId);

			// TODO: Make this optional
			query = query.AndAlso().WhereEquals("CorpusId", corpusId);

			var nextDoc = query.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(30)).FirstOrDefault();

			return Json(nextDoc);
		}

    	[HttpGet]
		[Authorize]
		public ActionResult SelectLanguage()
		{
			var langs = RavenSession.Advanced.DatabaseCommands.GetTerms("TopicsBasicIndex", "Language", null, 50);
			if (langs.Count() == 0)
				ViewBag.Message = "No languages were found. Please add a new topic";

			return View(langs);
		}

		[HttpGet]
		[Authorize]
		public ActionResult SelectTopic(int? corpusId, string lang)
		{
			Corpus corpus = null;
			if (corpusId > 0)
			{
				corpus = RavenSession.Load<Corpus>(corpusId.Value);
				if (corpus != null) lang = corpus.Language;
			}

			var topicsByLang = RavenSession.Query<Topic>()
				.Where(x => x.Language == lang)
				.ToArray();

			ViewBag.CorpusId = corpus == null ? string.Empty : corpus.Id;

			return View(topicsByLang);
		}
    }
}
