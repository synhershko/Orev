using System.Linq;
using System.Web.Mvc;
using Orev.Models;

namespace Orev.Controllers
{
    public class TopicsController : RavenController
    {
        //
        // GET: /Topics/

        public ActionResult Index()
        {
			var topics = RavenSession.Query<Topic>().Customize(x => x.WaitForNonStaleResultsAsOfLastWrite()).ToArray();
            return View(topics);
        }

		[HttpGet]
		[Authorize]
		public ActionResult Add(string lang)
		{
			return View("Edit", new Topic { Language = (string.IsNullOrWhiteSpace(lang) ? "en-US" : lang) });
		}

		[HttpGet]
		[Authorize]
		public ActionResult Edit(int id)
		{
			var topic = RavenSession.Load<Topic>(id);
			if (topic == null)
				return HttpNotFound("The requested corpus does not exist.");
			return View(topic);
		}

		[HttpPost]
		[Authorize]
		public ActionResult Update(Topic input)
		{
			if (!ModelState.IsValid)
				return View("Edit", input);

			var topic = string.IsNullOrWhiteSpace(input.Id) ? null : RavenSession.Load<Topic>(input.Id);
			if (topic != null)
			{
				topic.Description = input.Description;
				topic.Language = input.Language;
				topic.Narrator = input.Narrator;
				topic.Title = input.Title;
			}
			else
			{
				input.UserId = "users/" + User.Identity.Name;
				RavenSession.Store(input);
			}

			return RedirectToAction("Index");
		}

    }
}
