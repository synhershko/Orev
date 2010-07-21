using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using Orev.Domain;
using JudgmentsRepository = Orev.Repositories.NHibernateRepository<Orev.Domain.Judgment>;

namespace Orev.Common
{
    public class TopicLanguageEqualityComparer : IEqualityComparer<Topic>
    {
        public bool Equals(Topic t1, Topic t2)
        {
            if (t1.Language == null)
                return false;
            if (t1.Language.Equals(t2))
                return true;
            return false;
        }

        public int GetHashCode(Topic t)
        {
            return t.GetHashCode();
        }
    }

    public static class OrevHelpers
    {
        private static Dictionary<int, LinkedList<string>> CorporaFiles = new Dictionary<int, LinkedList<string>>();

        public static string GetNextCorpusFile(Corpus c)
        {
            string ret = null;

            lock (CorporaFiles)
            {
                LinkedList<string> files = null;
                if (!CorporaFiles.TryGetValue(c.Id, out files) || files == null || files.Count == 0)
                {
                    files = LoadCorpusFiles(c);
                    if (files != null && files.Count > 0)
                    {
                        CorporaFiles.Remove(c.Id);
                        CorporaFiles[c.Id] = files;
                    }
                }

                if (files != null && files.Count > 0)
                {
                    ret = files.First.Value;
                    files.RemoveFirst();
                    return ret;
                }
            }

            return null;
        }

        private static LinkedList<string> LoadCorpusFiles(Corpus c) // synced by caller
        {
            // TODO: If no files left that are not judged, set the corpus's status to "complete"

            string fullPath = HttpContext.Current.Request.PhysicalApplicationPath + c.Path;

            DirectoryInfo di = new DirectoryInfo(fullPath);
            if (di.Exists)
            {
                FileInfo[] rgFiles = di.GetFiles("*.htm*");
                LinkedList<string> filesList = new LinkedList<string>();

                using (var rep = new JudgmentsRepository())
                {
                    foreach (FileInfo fi in rgFiles)
                    {
                        // Only add documents that are not judged yet
                        if (rep.All(
                            J =>
                                J.CorpusId == c.Id
                                && J.DocumentId
                                .Equals(fi.Name)
                                ).Count() == 0
                            )
                        filesList.AddLast(fi.Name);
                    }
                }

                return filesList;
            }
            return null;
        }
    }
}
