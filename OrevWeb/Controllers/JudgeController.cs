using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orev.Domain;
using Orev.Common;
using Orev.Repositories;

using TopicsRepository = Orev.Repositories.NHibernateRepository<Orev.Domain.Topic>;
using JudgmentsRepository = Orev.Repositories.NHibernateRepository<Orev.Domain.Judgment>;

namespace Orev.Controllers
{
    public class JudgeController : Controller
    {
        public ActionResult Index(int? topicId, int? corpusId, string lang)
        {
            // Go to topic selection if none selected
            if (topicId == null || topicId <= 0)
            {
                if (!string.IsNullOrEmpty(lang) || corpusId > 0)
                    return RedirectToAction("SelectTopic", new { corpusId = corpusId, lang = lang });
                else
                    return RedirectToAction("SelectLanguage");
            }

            // Get the actual topic and save its language identifier
            using (var rep = new TopicsRepository())
            {
                var topic = rep.SingleOrDefault(T => T.Id == topicId.Value);
                if (topic == null)
                {
                    ViewData["Message"] = "No such topic exists";
                    return View();
                }
                
                ViewData["Topic"] = topic;
                ViewData["TopicId"] = topic.Id;
                lang = topic.Language;
            }

            using (var rep = new CorporaRepository())
            {
                Corpus corpus = null;

                // Load the requested corpus, failsafe below
                if (corpusId != null && corpusId > 0)
                {
                    corpus = rep.SingleOrDefault(C => C.Id == corpusId.Value && C.Language == lang);
                    ViewData["Corpus"] = corpus;
                    if (corpus != null)
                        corpusId = corpus.Id;
                }

                // Auto-select a corpus matching the topic's language
                if (ViewData["Corpus"] == null || corpusId <= 0)
                {
                    corpus = rep.SelectCorpusForLanguage(lang);
                    if (corpus == null)
                        ViewData["Message"] = "No corpus was found for the selected language";
                    else
                    {
                        ViewData["Corpus"] = corpus;
                        corpusId = corpus.Id;
                    }
                    // Another option: return RedirectToAction("SelectCorpus", new { lang = lang });
                }

                ViewData["CorpusUrl"] = corpus.Path;
            }
            ViewData["CorpusId"] = corpusId;


            return View();
        }

        [HttpPost]
        public JsonResult SaveJudgment(int topicId, int corpusId, string docId, bool judgment)
        {
            // TODO: input validation
            // TODO: user validation

            if (!string.IsNullOrEmpty(docId))
            {
                Judgment j = new Judgment
                {
                    CorpusId = corpusId,
                    DocumentId = docId,
                    TopicId = topicId,
                    UserId = 0, // TODO: Set to current user ID
                    UserJudgement = judgment
                };

                using (var jRep = new JudgmentsRepository())
                {
                    jRep.Add(j);
                }
            }

            using (var Corpora = new CorporaRepository())
            {
                var corpus = Corpora.SingleOrDefault(C => C.Id == corpusId);
                string nextDocId = OrevHelpers.GetNextCorpusFile(corpus);
                if (string.IsNullOrEmpty(nextDocId))
                    return Json(new { corpusDone = true });
                else
                    return Json(new { corpusDone = false, nextDoc = nextDocId });
            }
        }

        public ActionResult SelectLanguage()
        {
            using (var rep = new TopicsRepository())
            {
                IEnumerable<Topic> l = rep.Distinct(new TopicLanguageEqualityComparer());
                if (l == null || l.Count() == 0)
                    ViewData["Message"] = "No languages were found. Please add a new topic";
                else
                {
                    string[] s = new string[l.Count()];
                    for (int i = 0; i < l.Count(); i++)
                    {
                        s[i] = l.ElementAt(i).Language;
                    }
                    ViewData["LanguagesList"] = s;
                }
            }

            return View();
        }

        public ActionResult SelectTopic(int? corpusId, string lang)
        {
            if (corpusId > 0)
            {
                using (var rep = new CorporaRepository())
                {
                    var corpus = rep.SingleOrDefault(C => C.Id == corpusId);
                    if (corpus != null)
                        lang = corpus.Language;
                }
            }

            using (var rep = new TopicsRepository())
            {
                IEnumerable<Topic> topicsByLang = rep.All(T => T.Language == lang);
                return View(topicsByLang);
            }
        }


        public ActionResult ClearAllJudgments(int corpusId, int topicId, int? userId)
        {
            using (var rep = new JudgmentsRepository())
            {
                var js = rep.All(J => J.CorpusId == corpusId && J.TopicId == topicId);
                foreach (var j in js)
                {
                    rep.Delete(j);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
