//
// EventBoundSetWrapper.cs
//
// Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
// Copyright (c) 2016 Craig Fowler
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Collections
{
  /// <summary>
  /// Implementation of a generic <c>EventBoundCollectionWrapper</c> that wraps a generic <c>ISet</c> instance.
  /// </summary>
  public class EventBoundSetWrapper<T> : EventBoundCollectionWrapper<ISet<T>,T>, ISet<T> where T : class
  {
    #region ISet implementation

    /// <summary>
    /// Adds an item to the current instance.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the instance was added (IE: it was not already contained); <c>false</c> otherwise.
    /// </returns>
    /// <param name='o'>
    /// The item to add.
    /// </param>
    public new bool Add (T o)
    {
      bool output = false;

      if(this.BeforeAdd(this.WrappedCollection, o))
      {
        output = this.WrappedCollection.Add(o);
        this.AfterAdd(this.WrappedCollection);
      }

      return output;
    }

    /// <summary>
    /// Adds all of the given items to the current collection.
    /// </summary>
    /// <returns>
    /// <c>true</c> if at least one of the instances was added (IE: it was not already contained); <c>false</c>
    /// otherwise.
    /// </returns>
    /// <param name='c'>
    /// The items to add.
    /// </param>
    public bool AddAll (IEnumerable<T> c)
    {
      if(c == null)
      {
        throw new ArgumentNullException("c");
      }

      bool output = false;

      foreach(T item in c)
      {
        output |= this.Add(item);
      }

      return output;
    }

    /// <summary>
    /// Removes all of the given items from the current collection.
    /// </summary>
    /// <returns>
    /// <c>true</c> if at least one of the instances was removed (IE: it was previously contained); <c>false</c>
    /// otherwise.
    /// </returns>
    /// <param name='c'>
    /// The items to remove.
    /// </param>
    public bool RemoveAll (ICollection<T> c)
    {
      bool output = false;

      foreach(T item in c)
      {
        output |= this.Remove(item);
      }

      return output;
    }

    /// <summary>
    /// Removes all of the given items from the current collection.
    /// </summary>
    /// <param name='other'>The items to remove.</param>
    public void ExceptWith(IEnumerable<T> other)
    {
      if(other == null)
      {
        throw new ArgumentNullException(nameof(other));
      }

      this.RemoveAll(other.ToArray());
    }

    /// <summary>
    /// Removes items from the current collection, except those which exist in the given collection.
    /// </summary>
    /// <param name='other'>The items to keep.</param>
    public void IntersectWith(IEnumerable<T> other)
    {
      if(other == null)
      {
        throw new ArgumentNullException(nameof(other));
      }

      this.ExceptWith(this.WrappedCollection.Except(other));
    }

    /// <summary>
    /// Determines whether the current set is a proper (strict) subset of a specified collection.
    /// </summary>
    /// <param name='other'>The other collection.</param>
    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
      return this.WrappedCollection.IsProperSubsetOf(other);
    }

    /// <summary>
    /// Determines whether the current set is a proper (strict) superset of a specified collection.
    /// </summary>
    /// <param name='other'>The other collection.</param>
    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
      return this.WrappedCollection.IsProperSupersetOf(other);

    }

    /// <summary>
    /// Determines whether a set is a subset of a specified collection.
    /// </summary>
    /// <param name='other'>The other collection.</param>
    public bool IsSubsetOf(IEnumerable<T> other)
    {
      return this.WrappedCollection.IsSubsetOf(other);
    }

    /// <summary>
    /// Determines whether the current set is a superset of a specified collection.
    /// </summary>
    /// <param name='other'>The other collection.</param>
    public bool IsSupersetOf(IEnumerable<T> other)
    {
      return this.WrappedCollection.IsSupersetOf(other);
    }

    /// <summary>
    /// Determines whether the current set overlaps with the specified collection.
    /// </summary>
    /// <param name='other'>The other collection.</param>
    public bool Overlaps(IEnumerable<T> other)
    {
      return this.WrappedCollection.Overlaps(other);
    }

    /// <summary>
    /// Determines whether the current set and the specified collection contain the same elements.
    /// </summary>
    /// <param name='other'>The other collection.</param>
    public bool SetEquals(IEnumerable<T> other)
    {
      return this.WrappedCollection.SetEquals(other);
    }

    /// <summary>
    /// Modifies the current set so that it contains only elements that are present either in the current set or in 
    /// the specified collection, but not both.
    /// </summary>
    /// <param name='other'>The other collection.</param>
    public void SymmetricExceptWith(IEnumerable<T> other)
    {
      if(other == null)
      {
        throw new ArgumentNullException(nameof(other));
      }

      var toRemove = this.WrappedCollection.Intersect(other);
      var toAdd = other.Except(this.WrappedCollection);

      this.ExceptWith(toRemove);
      this.AddAll(toAdd);
    }

    /// <summary>
    /// Modifies the current set so that it contains all elements that are present in the current set, in the
    /// specified collection, or in both.
    /// </summary>
    /// <param name='other'>The other collection.</param>
    public void UnionWith(IEnumerable<T> other)
    {
      if(other == null)
      {
        throw new ArgumentNullException(nameof(other));
      }

      var elementsToAdd = this.WrappedCollection
        .Union(other)
        .Distinct()
        .Except(this.WrappedCollection);

      this.AddAll(elementsToAdd);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the event-bound-set class.
    /// </summary>
    /// <param name='wrapped'>
    /// The set instance to wrap.
    /// </param>
    public EventBoundSetWrapper (ISet<T> wrapped) : base(wrapped) {}

    #endregion
  }
}

