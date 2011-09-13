using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

		[Required]
		public string CorpusId { get; set; }

		[Required]
		public string DocumentId { get; set; }

		[Required]
		public string TopicId { get; set; }

		[Required]
		public string UserId { get; set; }

		[Required]
		public Verdict UserJudgement { get; set; }
	}
}