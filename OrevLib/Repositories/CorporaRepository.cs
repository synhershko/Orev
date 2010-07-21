using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orev.Domain;

namespace Orev.Repositories
{
    public class CorporaRepository : NHibernateRepository<Corpus>
    {
        /// <summary>
        /// Pulls a corpus by language identifier, also can be used to prioritize corpora
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public virtual Corpus SelectCorpusForLanguage(string lang)
        {
            return SingleOrDefault(C => C.Language == lang);
        }
    }
}
