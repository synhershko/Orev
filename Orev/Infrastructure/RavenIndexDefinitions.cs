using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Orev.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Orev.Infrastructure
{
	public class TopicsBasicIndex : AbstractIndexCreationTask<Topic>
	{
		public TopicsBasicIndex()
		{
			Map = docs => from doc in docs
			              select new {doc.Language, doc.UserId, doc.Title, doc.Description, doc.Narrator};

			Indexes = new Dictionary<Expression<Func<Topic, object>>, FieldIndexing>
			          	{
			          		{x => x.Title, FieldIndexing.Analyzed},
			          		{x => x.Description, FieldIndexing.Analyzed},
			          		{x => x.Narrator, FieldIndexing.Analyzed},
			          	};
		}
	}

	public class CorpusBasicIndex : AbstractIndexCreationTask<Corpus>
	{
		public CorpusBasicIndex()
		{
			Map = docs => from doc in docs
			              select new {doc.Language, doc.Name, doc.Description};

			Indexes = new Dictionary<Expression<Func<Corpus, object>>, FieldIndexing>
			          	{
			          		{x => x.Name, FieldIndexing.Analyzed},
			          		{x => x.Description, FieldIndexing.Analyzed},
			          	};
		}
	}

	public class CorpusDocumentsBasicIndex : AbstractIndexCreationTask<CorpusDocument>
	{
		public CorpusDocumentsBasicIndex()
		{
			Map = docs => from doc in docs
			              select new {doc.CorpusId, doc.InternalUniqueName};
		}
	}

	public class JudgmentsBasicIndex : AbstractIndexCreationTask<Judgment>
	{
		public JudgmentsBasicIndex()
		{
			Map = docs => from doc in docs
			              select new {doc.CorpusId, doc.DocumentId, doc.TopicId, doc.UserId, doc.UserJudgement};
		}
	}

	public class CorpusDocuments_ByNextUnrated : AbstractMultiMapIndexCreationTask<CorpusDocuments_ByNextUnrated.ReduceResult>
	{
		public class ReduceResult
		{
			public string DocumentId { get; set; }
			public string CorpusId { get; set; }
			public string[] Topics { get; set; }
		}

		public CorpusDocuments_ByNextUnrated()
		{
			AddMap<CorpusDocument>(docs => from corpusDoc in docs
										   select new { DocumentId = corpusDoc.Id, CorpusId = corpusDoc.CorpusId, Topics = new string[0] }
										   );

			AddMap<Judgment>(judgments => from j in judgments
										  select new { DocumentId = j.DocumentId, CorpusId = string.Empty, Topics = new string[] { j.TopicId } });

			Reduce = results => from result in results
								group result by result.DocumentId
			                    into g
									select new
			                           	{
			                           		DocumentId = g.Key,
											CorpusId = g.Select(x=>x.CorpusId).FirstOrDefault(),
			                           		Topics = g.SelectMany(x => x.Topics).Distinct().ToArray()
			                           	};
			
			TransformResults = (db, results) => from result in results
			                                    let doc = db.Load<CorpusDocument>(result.DocumentId)
			                                    select doc;
		}
	}
}