using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orev.Domain
{
    public class Judgment : IOrevEntity
    {
        // Yes, we could use NHibernate's session.Load to load object proxies instead of somewhat
        // breaking the ORM/UoW logic of using object references, as we do below.
        // However, since a Judgment object is merely a small data unit which will hardly be loaded
        // back from storage, doing proper persistence is not our main concern.

        public virtual int Id { get; set; }
        public virtual int CorpusId { get; set; }
        public virtual String DocumentId { get; set; }
        public virtual int TopicId { get; set; }
        public virtual int UserId { get; set; }
        public virtual bool UserJudgement { get; set; }
    }
}
