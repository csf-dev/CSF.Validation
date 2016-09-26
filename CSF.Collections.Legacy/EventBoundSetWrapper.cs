//
// EventBoundSetWrapper.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Iesi.Collections.Generic;

namespace CSF.Collections.EventHandling.Legacy
{
  /// <summary>
  /// Implementation of a generic <c>EventBoundCollectionWrapper</c> that wraps a generic <c>ISet</c> instance.
  /// </summary>
  public class EventBoundSetWrapper<T>
    : EventBoundCollectionWrapper<Iesi.Collections.Generic.ISet<T>,T>,
      Iesi.Collections.Generic.ISet<T>,
      Iesi.Collections.ISet
    where T : class
  {
    #region ISet implementation

    /// <summary>
    /// Gets a value indicating whether this instance is empty.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is empty; otherwise, <c>false</c>.
    /// </value>
    public bool IsEmpty
    {
      get {
        return base.WrappedCollection.IsEmpty;
      }
    }

    /// <summary>
    /// Performs a union operation between the current instance and the given set.
    /// </summary>
    /// <param name='a'>
    /// The set with which to perform the operation;
    /// </param>
    public Iesi.Collections.Generic.ISet<T> Union (Iesi.Collections.Generic.ISet<T> a)
    {
      return this.WrappedCollection.Union(a);
    }

    /// <summary>
    /// Performs an intersect operation between the current instance and the given set.
    /// </summary>
    /// <param name='a'>
    /// The set with which to perform the operation;
    /// </param>
    public Iesi.Collections.Generic.ISet<T> Intersect (Iesi.Collections.Generic.ISet<T> a)
    {
      return this.WrappedCollection.Intersect(a);
    }

    /// <summary>
    /// Performs a 'minus' operation between the current instance and the given set.
    /// </summary>
    /// <param name='a'>
    /// The set with which to perform the operation;
    /// </param>
    public Iesi.Collections.Generic.ISet<T> Minus (Iesi.Collections.Generic.ISet<T> a)
    {
      return this.WrappedCollection.Minus(a);
    }

    /// <summary>
    /// Performs an exclusive or operation between the current instance and the given set.
    /// </summary>
    /// <param name='a'>
    /// The set with which to perform the operation;
    /// </param>
    public Iesi.Collections.Generic.ISet<T> ExclusiveOr (Iesi.Collections.Generic.ISet<T> a)
    {
      return this.WrappedCollection.ExclusiveOr(a);
    }

    /// <summary>
    /// Gets a value that indicates whether the current instance contains all of the given instances.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the current instance contains all of the given instances; <c>false</c> otherwise.
    /// </returns>
    /// <param name='c'>
    /// If set to <c>true</c> c.
    /// </param>
    public bool ContainsAll (ICollection<T> c)
    {
      if(c == null)
      {
        throw new ArgumentNullException("c");
      }

      return this.WrappedCollection.ContainsAll(c);
    }

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
    public bool AddAll (ICollection<T> c)
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
      if(c == null)
      {
        throw new ArgumentNullException("c");
      }

      bool output = false;

      foreach(T item in c)
      {
        output |= this.Remove(item);
      }

      return output;
    }

    /// <summary>
    /// Removes all of the items from the current collection, except those that are contained within the given
    /// collection.
    /// </summary>
    /// <returns>
    /// <c>true</c> if at least one instance was removed; <c>false</c> otherwise.
    /// </returns>
    /// <param name='c'>
    /// The collection of items not to remove from the current instance.
    /// </param>
    public bool RetainAll (ICollection<T> c)
    {
      if(c == null)
      {
        throw new ArgumentNullException("c");
      }

      Set<T> set1 = new HashedSet<T>(c), set2 = new HashedSet<T>();

      foreach(T item in this)
      {
        if(!set1.Contains(item))
        {
          set2.Add(item);
        }
      }

      return this.RemoveAll(set2);
    }

    /// <summary>
    /// Clones the current instance.
    /// </summary>
    public object Clone ()
    {
      Iesi.Collections.Generic.ISet<T> clonedWrappedSet;
      clonedWrappedSet = (Iesi.Collections.Generic.ISet<T>) base.WrappedCollection.Clone();
      EventBoundSetWrapper<T> clonedWrapper = new EventBoundSetWrapper<T>(clonedWrappedSet);

      clonedWrapper.BeforeAdd = this.BeforeAdd;
      clonedWrapper.AfterAdd = this.AfterAdd;
      clonedWrapper.BeforeRemove = this.BeforeRemove;
      clonedWrapper.AfterRemove = this.AfterRemove;

      return clonedWrapper;
    }

    #endregion

    #region ISet implementation

    Iesi.Collections.ISet Iesi.Collections.ISet.Union (Iesi.Collections.ISet a)
    {
      return (Iesi.Collections.ISet) this.Union((Iesi.Collections.Generic.ISet<T>) a);
    }

    Iesi.Collections.ISet Iesi.Collections.ISet.Intersect (Iesi.Collections.ISet a)
    {
      return (Iesi.Collections.ISet) this.Intersect((Iesi.Collections.Generic.ISet<T>) a);
    }

    Iesi.Collections.ISet Iesi.Collections.ISet.Minus (Iesi.Collections.ISet a)
    {
      return (Iesi.Collections.ISet) this.Minus((Iesi.Collections.Generic.ISet<T>) a);
    }

    Iesi.Collections.ISet Iesi.Collections.ISet.ExclusiveOr (Iesi.Collections.ISet a)
    {
      return (Iesi.Collections.ISet) this.ExclusiveOr((Iesi.Collections.Generic.ISet<T>) a);
    }

    bool Iesi.Collections.ISet.Contains (object o)
    {
      return this.Contains((T) o);
    }

    bool Iesi.Collections.ISet.ContainsAll (ICollection c)
    {
      return this.ContainsAll((ICollection<T>) c);
    }

    bool Iesi.Collections.ISet.Add (object o)
    {
      return this.Add((T) o);
    }

    bool Iesi.Collections.ISet.AddAll (ICollection c)
    {
      return this.AddAll((ICollection<T>) c);
    }

    bool Iesi.Collections.ISet.Remove (object o)
    {
      return this.Remove((T) o);
    }

    bool Iesi.Collections.ISet.RemoveAll (ICollection c)
    {
      return this.RemoveAll((ICollection<T>) c);
    }

    bool Iesi.Collections.ISet.RetainAll (ICollection c)
    {
      return this.RetainAll((ICollection<T>) c);
    }

    void Iesi.Collections.ISet.Clear ()
    {
      this.Clear();
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the event-bound-set class.
    /// </summary>
    /// <param name='wrapped'>
    /// The set instance to wrap.
    /// </param>
    public EventBoundSetWrapper (Iesi.Collections.Generic.ISet<T> wrapped) : base(wrapped) {}

    #endregion
  }
}

