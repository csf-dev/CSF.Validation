using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using CSF.Entities;

namespace CSF.Data.NHibernate
{
  /// <summary>
  /// Default implementation of the <see cref="ISessionWrapper"/>, redirecting calls directly to the NHibernate
  /// extension methods.
  /// </summary>
  public sealed class SessionWrapper : ISessionWrapper
  {
    #region fields

    private ISession _session;

    #endregion

    #region properties

    /// <summary>
    /// Gets the wrapped NHibernate ISession.
    /// </summary>
    /// <value>
    ///  The ISession. 
    /// </value>
    public ISession Session
    {
      get {
        return _session;
      }
    }

    #endregion

    #region equivalents to NHibernate LINQ extension methods

    /// <summary>
    /// Returns a cacheable implementation of the given <paramref name="query"/>.
    /// </summary>
    /// <returns>
    /// A queryable instance, with the desired cache-modification.
    /// </returns>
    /// <param name='query'>
    /// The query to manipulate (note that the instance is not directly manipulated, use the return value).
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    public IQueryable<T> Cacheable<T>(IQueryable<T> query)
    {
      return query.Cacheable();
    }

    /// <summary>
    /// Controls how a query interacts with the second level cache.
    /// </summary>
    /// <returns>
    /// A queryable instance, with the desired cache-modification.
    /// </returns>
    /// <param name='query'>
    /// The query to manipulate (note that the instance is not directly manipulated, use the return value).
    /// </param>
    /// <param name='cacheMode'>
    /// The cache mode to set into the query.
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    public IQueryable<T> CacheMode<T>(IQueryable<T> query, CacheMode cacheMode)
    {
      return query.CacheMode(cacheMode);
    }

    /// <summary>
    /// Configures a query cache region.
    /// </summary>
    /// <returns>
    /// A queryable instance, with the desired cache-modification.
    /// </returns>
    /// <param name='query'>
    /// The query to manipulate (note that the instance is not directly manipulated, use the return value).
    /// </param>
    /// <param name='region'>
    /// The name of the cache region to create.
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    public IQueryable<T> CacheRegion<T>(IQueryable<T> query, string region)
    {
      return query.CacheRegion(region);
    }

    /// <summary>
    /// Creates a new Linq query
    /// </summary>
    /// <returns>
    /// A generic <c>IQueryable</c> instance.
    /// </returns>
    /// <param name='session'>
    /// An NHibernate ISession.
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    public IQueryable<T> Query<T>(IStatelessSession session)
    {
      return session.Query<T>();
    }

    /// <summary>
    /// Creates a new Linq query
    /// </summary>
    /// <returns>
    /// A generic <c>IQueryable</c> instance.
    /// </returns>
    /// <param name='session'>
    /// An NHibernate ISession.
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    public IQueryable<T> Query<T>(ISession session)
    {
      return session.Query<T>();
    }

    /// <summary>
    /// Creates a new Linq query using the <see cref="Session"/> referenced by the current instance.
    /// </summary>
    /// <returns>
    /// A generic <c>IQueryable</c> instance.
    /// </returns>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    public IQueryable<T> Query<T>()
    {
      return this.Query<T>(this.Session);
    }

    /// <summary>
    /// Returns a copy of the given <paramref name="query"/> as a future-query.
    /// </summary>
    /// <returns>
    /// The future-query results.
    /// </returns>
    /// <param name='query'>
    /// The query to manipulate.
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    public IEnumerable<T> ToFuture<T>(IQueryable<T> query)
    {
      return query.ToFuture();
    }

    /// <summary>
    /// Returns a copy of the given <paramref name="query"/> as a future-value.
    /// </summary>
    /// <returns>
    /// The future-value result.
    /// </returns>
    /// <param name='query'>
    /// The query to manipulate.
    /// </param>
    /// <typeparam name='T'>
    /// The target type for the query.
    /// </typeparam>
    public IFutureValue<T> ToFutureValue<T>(IQueryable<T> query)
    {
      return query.ToFutureValue();
    }

    #endregion

    #region getting and loading

    /// <summary>
    /// Gets an entity of the specified identity.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is a parallel to <c>ISession.Get</c> except that it takes a generic <c>IIdentity</c> instance,
    /// removing the need to pass in the type of entity desired.
    /// </para>
    /// </remarks>
    /// <param name='identity'>
    /// The identity of the desired entity.
    /// </param>
    /// <param name='session'>
    /// An NHibernate ISession
    /// </param>
    /// <typeparam name='T'>
    /// The type of entity desired, which may be inferred via the <paramref name="identity"/> parameter.
    /// </typeparam>
    public T Get<T>(IIdentity<T> identity, ISession session) where T : IEntity
    {
      if(session == null)
      {
        throw new ArgumentNullException("session");
      }

      return (identity != null)? session.Get<T>(identity.Value) : default(T);
    }

    /// <summary>
    /// Loads an entity of the specified identity.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is a parallel to <c>ISession.Load</c> except that it takes a generic <c>IIdentity</c> instance,
    /// removing the need to pass in the type of entity desired.
    /// </para>
    /// <para>
    /// Additionally, this method will return a null/default result for a null identity.
    /// </para>
    /// </remarks>
    /// <param name='identity'>
    /// The identity of the desired entity.
    /// </param>
    /// <param name='session'>
    /// An NHibernate ISession
    /// </param>
    /// <typeparam name='T'>
    /// The type of entity desired, which may be inferred via the <paramref name="identity"/> parameter.
    /// </typeparam>
    public T Load<T>(IIdentity<T> identity, ISession session) where T : IEntity
    {
      if(session == null)
      {
        throw new ArgumentNullException("session");
      }

      return (identity != null)? session.Load<T>(identity.Value) : default(T);
    }

    /// <summary>
    /// Gets an entity of the specified identity using the <see cref="Session"/> referenced by this instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is a parallel to <c>ISession.Get</c> except that it takes a generic <c>IIdentity</c> instance,
    /// removing the need to pass in the type of entity desired.
    /// </para>
    /// </remarks>
    /// <param name='identity'>
    /// The identity of the desired entity.
    /// </param>
    /// <typeparam name='T'>
    /// The type of entity desired, which may be inferred via the <paramref name="identity"/> parameter.
    /// </typeparam>
    public T Get<T>(IIdentity<T> identity) where T : IEntity
    {
      return this.Get(identity, this.Session);
    }

    /// <summary>
    /// Loads an entity of the specified identity using the <see cref="Session"/> referenced by this instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is a parallel to <c>ISession.Load</c> except that it takes a generic <c>IIdentity</c> instance,
    /// removing the need to pass in the type of entity desired.
    /// </para>
    /// <para>
    /// Additionally, this method will return a null/default result for a null identity.
    /// </para>
    /// </remarks>
    /// <param name='identity'>
    /// The identity of the desired entity.
    /// </param>
    /// <typeparam name='T'>
    /// The type of entity desired, which may be inferred via the <paramref name="identity"/> parameter.
    /// </typeparam>
    public T Load<T>(IIdentity<T> identity) where T : IEntity
    {
      return this.Load(identity, this.Session);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Data.NHibernate.SessionWrapper"/> class.
    /// </summary>
    /// <param name='session'>
    /// An NHibernate ISession.
    /// </param>
    public SessionWrapper(ISession session)
    {
      if(session == null)
      {
        throw new ArgumentNullException("session");
      }

      _session = session;
    }

    #endregion
  }
}

