using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orev.Models
{
	public class Judgment
	{
		public enum Verdict
		{
			Relevant,
			NotRelevant,
			Skip,
		};

		public string CorpusId { get; set; }
		public string DocumentId { get; set; }
		public string TopicId { get; set; }
		public string UserId { get; set; }
		public Verdict UserJudgement { get; set; }
	}
}