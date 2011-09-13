using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orev.Models
{
	public class CorpusFeedingInput
	{
		[Required]
		public string CorpusId { get; set; }

		[Required]
		public Uri DocumentsZipUrl { get; set; }
	}
}