//
// ThreadSafeCache.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2015 CSF Software Limited
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Threading;
using System.Collections.Generic;

namespace CSF.Caches
{
  /// <summary>
  /// Implementation of <see cref="ICache{TKey,TValue}"/> that is thread-safe and well-optimised for frequent reading.
  /// </summary>
  public class ThreadSafeCache<TKey,TValue> : ICache<TKey,TValue>
  {
    #region fields

    private Dictionary<TKey,TValue> _cache;
    private ReaderWriterLockSlim _syncRoot;

    #endregion

    #region properties

    /// <summary>
    /// Gets the underlying cache storage.
    /// </summary>
    /// <value>The cache.</value>
    protected virtual IDictionary<TKey,TValue> Cache
    {
      get {
        return _cache;
      }
    }

    /// <summary>
    /// Gets an object used for performing locking and concurrency control within this type.
    /// </summary>
    /// <value>The synchronisation root.</value>
    protected virtual ReaderWriterLockSlim SyncRoot
    {
      get {
        return _syncRoot;
      }
    }

    #endregion

    #region ICache implementation

    /// <summary>
    /// Adds an item to the cache.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If an item already exists within the cache, identified by the given key, then the value is not added to the
    /// cache, and <c>false</c> is returned.
    /// </para>
    /// </remarks>
    /// <returns><c>true</c> if a new item was added to the cache; <c>false</c> if not.</returns>
    /// <param name="key">The key at which to store the value.</param>
    /// <param name="value">The value to store.</param>
    public virtual bool Add(TKey key, TValue value)
    {
      if(key == null)
      {
        throw new ArgumentNullException("key");
      }
      bool output;

      try
      {
        this.SyncRoot.EnterWriteLock();

        output = !this.Cache.ContainsKey(key);
        if(output)
        {
          this.Cache.Add(key, value);
        }
      }
      finally
      {
        if(this.SyncRoot.IsWriteLockHeld)
        {
          this.SyncRoot.ExitWriteLock();
        }
      }

      return output;
    }

    /// <summary>
    /// Removes an item from the cache.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If an item does not exist within the cache, identified by the given key, then nothing is removed and
    /// <c>false</c> is returned.
    /// </para>
    /// </remarks>
    /// <returns><c>true</c> if a an item was removed; <c>false</c> if not.</returns>
    /// <param name="key">The key at which to remove an item.</param>
    public virtual bool Remove(TKey key)
    {
      if(key == null)
      {
        throw new ArgumentNullException("key");
      }

      bool output;

      try
      {
        this.SyncRoot.EnterWriteLock();

        output = this.Cache.Remove(key);
      }
      finally
      {
        if(this.SyncRoot.IsWriteLockHeld)
        {
          this.SyncRoot.ExitWriteLock();
        }
      }

      return output;
    }

    /// <summary>
    /// Gets a value indicating whether the cache contains an item identified by the given key.
    /// </summary>
    /// <param name="key">The key at which to search for an item.</param>
    public virtual bool Contains(TKey key)
    {
      if(key == null)
      {
        throw new ArgumentNullException("key");
      }

      bool output;

      try
      {
        this.SyncRoot.EnterReadLock();

        output = this.Cache.ContainsKey(key);
      }
      finally
      {
        if(this.SyncRoot.IsReadLockHeld)
        {
          this.SyncRoot.ExitReadLock();
        }
      }

      return output;
    }

    /// <summary>
    /// Attempts to get a value from the cache.
    /// </summary>
    /// <returns><c>true</c>, if a value was found in the cache, <c>false</c> otherwise.</returns>
    /// <param name="key">The key for which to retrieve a value.</param>
    /// <param name="value">Exposes the value retrieved from the cache.  The value of this parameter is undefined if no value was found.</param>
    public virtual bool TryGet(TKey key, out TValue value)
    {
      if(key == null)
      {
        throw new ArgumentNullException("key");
      }

      bool output;

      try
      {
        this.SyncRoot.EnterReadLock();
        output = this.Cache.TryGetValue(key, out value);
      }
      finally
      {
        if(this.SyncRoot.IsReadLockHeld)
        {
          this.SyncRoot.ExitReadLock();
        }
      }

      return output;
    }

    /// <summary>
    /// Gets a value from the cache, raising an exception if no value is available.
    /// </summary>
    /// <exception cref="NotAvailableInCacheException">If the cache does not contain a value for the given key.</exception>
    /// <param name="key">The key at which to get a value.</param>
    public virtual TValue Get(TKey key)
    {
      if(key == null)
      {
        throw new ArgumentNullException("key");
      }

      TValue output;

      try
      {
        this.SyncRoot.EnterReadLock();
        output = this.Cache[key];
      }
      catch(KeyNotFoundException ex)
      {
        throw new NotAvailableInCacheException("The item must exist in the cache, consider using: bool TryGet(TKey,out TValue).",
                                               ex);
      }
      finally
      {
        if(this.SyncRoot.IsReadLockHeld)
        {
          this.SyncRoot.ExitReadLock();
        }
      }

      return output;
    }

    /// <summary>
    /// Attempts to get a value from the cache, executing a function to create a value (and add that to the cache) if
    /// one is not already contained.
    /// </summary>
    /// <returns>The cached value, or the newly-created/added one.</returns>
    /// <param name="key">The key for the value.</param>
    /// <param name="valueFactory">A delegate with which to create an instance of the value if it is required.</param>
    public virtual TValue GetOrAdd(TKey key, Func<TValue> valueFactory)
    {
      bool discarded;
      return this.GetOrAdd(key, valueFactory, out discarded);
    }

    /// <summary>
    /// Attempts to get a value from the cache, executing a function to create a value (and add that to the cache) if
    /// one is not already contained.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <paramref name="cacheHit"/> parameter will be set to <c>true</c> if the request was fulfilled from the
    /// cache, or <c>false</c> if the <paramref name="valueFactory"/> was invoked in order to create a value for the
    /// cache.
    /// </para>
    /// </remarks>
    /// <returns>The cached value, or the newly-created/added one.</returns>
    /// <param name="key">The key for the value.</param>
    /// <param name="valueFactory">A delegate with which to create an instance of the value if it is required.</param>
    /// <param name="cacheHit">Exposes a value indicating whether or not the result was a cache 'hit'.</param>
    public virtual TValue GetOrAdd(TKey key, Func<TValue> valueFactory, out bool cacheHit)
    {
      if(key == null)
      {
        throw new ArgumentNullException("key");
      }

      TValue output;

      try
      {
        this.SyncRoot.EnterUpgradeableReadLock();

        cacheHit = this.Cache.TryGetValue(key, out output);

        if(!cacheHit)
        {
          this.SyncRoot.EnterWriteLock();

          output = valueFactory();
          this.Cache.Add(key, output);
        }
      }
      finally
      {
        if(this.SyncRoot.IsWriteLockHeld)
        {
          this.SyncRoot.ExitWriteLock();
        }
        if(this.SyncRoot.IsUpgradeableReadLockHeld)
        {
          this.SyncRoot.ExitUpgradeableReadLock();
        }
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ThreadSafeCache{TKey,TValue}"/> class.
    /// </summary>
    public ThreadSafeCache()
    {
      _syncRoot = new ReaderWriterLockSlim();
      _cache = new Dictionary<TKey, TValue>();
    }

    #endregion
  }
}

