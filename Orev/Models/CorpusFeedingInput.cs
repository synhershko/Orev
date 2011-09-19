using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orev.Models
{
	public class CorpusFeedingInput
	{
		public CorpusFeedingInput()
		{
			DocumentEncoding = Encoding.GetEncoding("windows-1255"); // TODO
		}

		[Required]
		public int CorpusId { get; set; }

		[Required]
		public string DocumentsZipUrl { get; set; }

		public Encoding DocumentEncoding { get; set; }
	}
}