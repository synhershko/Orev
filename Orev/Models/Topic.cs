using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orev.Models
{
	public class Topic
	{
		public string Id { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		[Required]
		[DataType(DataType.MultilineText)]
		public string Narrator { get; set; }

		[Required]
		[StringLength(5, MinimumLength = 5)]
		public string Language { get; set; } // a language identifier string, en-US for example
		
		/// <summary>
		/// Id of user submitting this topic
		/// </summary>
		[Required]
		public string UserId { get; set; }
	}
}