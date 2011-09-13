using System.IO;
using Ionic.Zip;

namespace Orev.CorpusReaders.Standard
{
	public class ZippedCorpusReader : ICorpusReader
	{
		public CorpusDocument.ContentFormat DefaultFormat { get; set; }

		private readonly string _filePath;

		public ZippedCorpusReader(string zipFilePath)
		{
			AbortReading = false;
			_filePath = zipFilePath;
			DefaultFormat = CorpusDocument.ContentFormat.Html;
		}

		public event ReportProgressDelegate OnProgress;
		public event HitDocumentDelegate OnDocument;
		public bool AbortReading { get; set; }

		public void Read()
		{
			if (OnDocument == null) return;

			using (var zip = ZipFile.Read(_filePath))
			{
				foreach (var e in zip)
				{
					string contents;
					using (var stream = new MemoryStream())
					{
						e.Extract(stream);

						var sr = new StreamReader(stream);
						contents = sr.ReadToEnd();
					}
					OnDocument(new CorpusDocument { Id = e.FileName }.SetContent(contents, DefaultFormat));
				}
			}
		}
	}
}
