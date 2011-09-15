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

	public class CorpusDocuments_ByCorpusJudgmentsCount : AbstractIndexCreationTask<Judgment, CorpusDocument>
	{
		public CorpusDocuments_ByCorpusJudgmentsCount()
		{
			Map = docs => from doc in docs
						  select new { doc.CorpusId, doc.DocumentId, doc.TopicId, doc.UserId, doc.UserJudgement };


		}
	}
}