using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace Orev.CorpusReaders.Standard
{
	public class ZippedCorpusReader : ICorpusReader
	{
		public CorpusDocument.ContentFormat DefaultFormat { get; set; }
		public Encoding FileEncoding { get; set; }

		private readonly string _filePath;
		private readonly string _tempPath = Path.GetTempFileName();

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
					if (AbortReading) break;

					if (e.IsDirectory)
						continue;

					if (e.FileName.EndsWith("\\index.html"))
						continue;

					string contents;
					using (var stream = new MemoryStream())
					{
						e.Extract(stream);

						var sr = FileEncoding != null ? new StreamReader(stream, FileEncoding) : new StreamReader(stream);
						stream.Position = 0;
						contents = sr.ReadToEnd();
					}

					string title = null;
					CorpusDocument.ContentFormat format = DefaultFormat;

					// brute force at parsing out <title> tags, if HTML
					var m = Regex.Match(contents, @"<title>\s*(.+?)\s*</title>");
					if (m.Success)
					{
						title = m.Groups[1].Value;
						format = CorpusDocument.ContentFormat.Html;
					}

					OnDocument(new CorpusDocument { Id = e.FileName, Title = title }.SetContent(contents, format));
				}
			}
		}
	}
}
