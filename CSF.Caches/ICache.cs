//
// ICache.cs
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

namespace CSF.Caches
{
  /// <summary>
  /// Interface for a cache implementation, which caches instances of <typeparamref name="TValue" /> indexed by a
  /// <typeparamref name="TKey" />.
  /// </summary>
  public interface ICache<TKey,TValue>
  {
    #region methods

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
    bool Add(TKey key, TValue value);

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
    bool Remove(TKey key);

    /// <summary>
    /// Gets a value indicating whether the cache contains an item identified by the given key.
    /// </summary>
    /// <param name="key">The key at which to search for an item.</param>
    bool Contains(TKey key);

    /// <summary>
    /// Attempts to get a value from the cache.
    /// </summary>
    /// <returns><c>true</c>, if a value was found in the cache, <c>false</c> otherwise.</returns>
    /// <param name="key">The key for which to retrieve a value.</param>
    /// <param name="value">Exposes the value retrieved from the cache.  The value of this parameter is undefined if no value was found.</param>
    bool TryGet(TKey key, out TValue value);

    /// <summary>
    /// Gets a value from the cache, raising an exception if no value is available.
    /// </summary>
    /// <exception cref="NotAvailableInCacheException">If the cache does not contain a value for the given key.</exception>
    /// <param name="key">The key at which to get a value.</param>
    TValue Get(TKey key);

    /// <summary>
    /// Attempts to get a value from the cache, executing a function to create a value (and add that to the cache) if
    /// one is not already contained.
    /// </summary>
    /// <returns>The cached value, or the newly-created/added one.</returns>
    /// <param name="key">The key for the value.</param>
    /// <param name="valueFactory">A delegate with which to create an instance of the value if it is required.</param>
    TValue GetOrAdd(TKey key, Func<TValue> valueFactory);

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
    TValue GetOrAdd(TKey key, Func<TValue> valueFactory, out bool cacheHit);

    #endregion
  }
}

