using Orev.CorpusReaders;
using System.Linq;
using Raven.Client;
using Raven.Client.Linq;

namespace Orev.Infrastructure
{
	public class RavenCorpusReader : CorpusReaders.ICorpusReader
	{
		public RavenCorpusReader(ICorpusReader reader)
		{
			_reader = reader;
		}

		private ICorpusReader _reader { get; set; }
		private IDocumentSession _session;
		private int _docCount;

		public string CorpusId { get; set; }
		public bool InitialDeployment { get; set; }

		public event ReportProgressDelegate OnProgress;
		public event HitDocumentDelegate OnDocument;

		public bool AbortReading
		{
			get { return _reader.AbortReading; }
			set { _reader.AbortReading = value; }
		}

		public void Read()
		{
			_reader.OnDocument += _reader_OnDocument;
			_session = MvcApplication.DocumentStore.OpenSession();
			_reader.Read();

			_session.SaveChanges();
			_session.Dispose();
		}

		void _reader_OnDocument(CorpusDocument doc)
		{
			if (_docCount == 10) AbortReading = true;

			if (!InitialDeployment)
			{
				var d = _session.Query<Orev.Models.CorpusDocument>()
					.Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
					.Where(x => x.InternalUniqueName == doc.Id)
					.FirstOrDefault();

				if (d != null)
				{
					d.Title = doc.Title;
					d.Content = doc.Content;
					_session.SaveChanges();
					return;
				}
			}

			_session.Store(new Orev.Models.CorpusDocument
			               	{
			               		CorpusId = CorpusId,
								Title = doc.Title,
								Content = doc.Content,
								InternalUniqueName = doc.Id,
			               	});

			// This is here to boost the process and enable batching to the RavenDB server
			if (++_docCount % 20 == 0)
			{
				_session.SaveChanges();
				_session.Dispose();
				_session = MvcApplication.DocumentStore.OpenSession();
			}
		}
	}
}