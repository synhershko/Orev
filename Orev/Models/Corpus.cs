using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orev.Models
{
	public class Corpus
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Language { get; set; } // a language identifier string, en-US for example
	}
}