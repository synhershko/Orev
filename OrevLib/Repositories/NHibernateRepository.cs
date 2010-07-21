using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Linq;
using NHibernate.Cfg;

namespace Orev.Repositories
{
    public class NHibernateRepository<T> : IRepository<T>, IDisposable
        where T : class, Orev.Domain.IOrevEntity
    {
        #region SessionFactory

        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();
                    _sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        #endregion

        private ISession NHSession { get; set; }
        private ITransaction Transaction { get; set; }

        public bool OwnsSession { get; set; }
        public bool ManualTransactions { get; set; }

        public NHibernateRepository()
        {
            this.NHSession = OpenSession();
            OwnsSession = true;
            ManualTransactions = false;
        }

        public NHibernateRepository(ISession session)
        {
            this.NHSession = session;
            OwnsSession = false;
            ManualTransactions = false;
        }

        /// <summary>
        /// Allows sharing a session and a transaction between repositories
        /// </summary>
        public NHibernateRepository(ISession session, ITransaction transaction)
        {
            this.NHSession = session;
            this.Transaction = transaction;
            OwnsSession = false;
            ManualTransactions = true;
        }

        #region Transactions
        public void BeginTransaction()
        {
            if (Transaction != null)
                Transaction.Dispose();

            Transaction = NHSession.BeginTransaction();
        }

        public void Commit()
        {
            Transaction.Commit();
            Transaction.Dispose();
            Transaction = null;
        }

        public void Rollback()
        {
            Transaction.Rollback();
            Transaction.Dispose();
            Transaction = null;
        }
        #endregion

        #region IRepository<T> Members

        public virtual void Add(T item)
        {
            if (!ManualTransactions) BeginTransaction();
            NHSession.Save(item);
            if (!ManualTransactions) Commit();
        }

        public virtual void Update(T item)
        {
            if (!ManualTransactions) BeginTransaction();
            NHSession.Update(item);
            if (!ManualTransactions) Commit();
        }

        public virtual void Delete(T item)
        {
            if (!ManualTransactions) BeginTransaction();
            NHSession.Delete(item);
            if (!ManualTransactions) Commit();
        }

        public virtual T SingleOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            return NHSession.Linq<T>().SingleOrDefault(query);
        }

        public virtual IEnumerable<T> All(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            return NHSession.Linq<T>().Where(query).ToList();
        }

        public virtual int Count(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            return NHSession.Linq<T>().Count(query);
        }

        public virtual IEnumerable<T> Distinct(IEqualityComparer<T> comparer)
        {
            return NHSession.Linq<T>().Distinct(comparer);
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Transaction != null)
                {
                    Transaction.Rollback();
                    Transaction.Dispose();
                    Transaction = null;
                }
                if (OwnsSession && NHSession != null)
                {
                    NHSession.Dispose();
                    NHSession = null;
                }
            }
        }

        ~NHibernateRepository()
        {
            Dispose(false);
        }

        #endregion
    }
}
