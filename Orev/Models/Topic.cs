using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orev.Models
{
	public class Topic
	{
		public string Id { get; set; }
		
		public string Title { get; set; }
		public string Description { get; set; }
		public string Narrator { get; set; }

		public string Language { get; set; } // a language identifier string, en-US for example
		
		/// <summary>
		/// Id of user submitting this topic
		/// </summary>
		public string UserId { get; set; }
	}
}