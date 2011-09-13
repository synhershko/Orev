using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orev.Models
{
	public class Corpus
	{
		public string Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		[StringLength(5, MinimumLength = 5)]
		public string Language { get; set; } // a language identifier string, en-US for example
	}
}