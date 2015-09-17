//
// EventBoundCollectionWrapper`2.cs
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

namespace CSF.Collections
{
  /// <summary>
  /// Base type for all implementations of the generic <c>IEventBoundCollection&lt;TCollection,TItem&gt;</c> interface.
  /// </summary>
  [Serializable]
  public abstract class EventBoundCollectionWrapper<TCollection,T> : IEventBoundCollection<TCollection,T>
    where T : class
    where TCollection : ICollection<T>
  {
    #region fields

    private readonly TCollection _wrapped;
    private Action<TCollection> _afterAdd, _afterRemove;
    private Func<TCollection, T, bool> _beforeAdd, _beforeRemove;

    #endregion

    #region event-binding properties

    /// <summary>
    /// Gets or sets the action to perform after an item is added to this collection. 
    /// </summary>
    /// <value>
    /// The action to perform after item addition. 
    /// </value>
    public virtual Action<TCollection> AfterAdd
    {
      get {
        return _afterAdd ?? (_afterAdd = list => {});
      }
      set {
        _afterAdd = value;
      }
    }

    /// <summary>
    /// Gets or sets the action to perform after an item is removed from this collection. 
    /// </summary>
    /// <value>
    /// The action to perform after item removal. 
    /// </value>
    public virtual Action<TCollection> AfterRemove
    {
      get {
        return _afterRemove ?? (_afterRemove = list => {});
      }
      set {
        _afterRemove = value;
      }
    }

    /// <summary>
    /// Gets or sets a function to execute before an item is added to this collection. If the return value of that
    /// function is false then adding the item is aborted. 
    /// </summary>
    /// <value>
    /// The function to execute before item addition. 
    /// </value>
    public virtual Func<TCollection, T, bool> BeforeAdd
    {
      get {
        return _beforeAdd ?? (_beforeAdd = (list, item) => true);
      }
      set {
        _beforeAdd = value;
      }
    }

    /// <summary>
    /// Gets or sets a function to execute before an item is removed from this collection. If the return value of that
    /// function is false then removing the item is aborted. 
    /// </summary>
    /// <value>
    /// The function to execute before item removal. 
    /// </value>
    public virtual Func<TCollection, T, bool> BeforeRemove
    {
      get {
        return _beforeRemove ?? (_beforeRemove = (list, item) => true);
      }
      set {
        _beforeRemove = value;
      }
    }

    #endregion

    #region event-binding methods

    /// <summary>
    /// Detaches the specified item from the list as if it had been removed but does not actually remove it from the
    /// underlying list. 
    /// </summary>
    /// <param name='item'>
    /// The item to detach 
    /// </param>
    public virtual void Detach(T item)
    {
      if(this.BeforeRemove(this.WrappedCollection, item))
      {
        this.AfterRemove(this.WrappedCollection);
      }
    }

    /// <summary>
    /// Detaches all contained items from the list as if they had been removed, without actually removing them from the
    /// underlying list. 
    /// </summary>
    public virtual void DetachAll()
    {
      foreach(T item in _wrapped)
      {
        this.Detach(item);
      }
    }

    #endregion

    #region list properties

    /// <summary>
    /// Gets the count of elements in this collection.
    /// </summary>
    /// <value>
    /// The count.
    /// </value>
    public virtual int Count
    {
      get {
        return _wrapped.Count;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is read only.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
    /// </value>
    public virtual bool IsReadOnly
    {
      get {
        return _wrapped.IsReadOnly;
      }
    }

    /// <summary>
    /// Gets the wrapped collection.
    /// </summary>
    /// <value>
    /// The wrapped collection.
    /// </value>
    protected TCollection WrappedCollection
    {
      get {
        return _wrapped;
      }
    }

    #endregion

    #region list methods

    /// <summary>
    /// Gets the enumerator for the current instance.
    /// </summary>
    /// <returns>
    /// The enumerator.
    /// </returns>
    public virtual IEnumerator<T> GetEnumerator()
    {
      return _wrapped.GetEnumerator();
    }

    /// <summary>
    /// Adds an item to the current instance.
    /// </summary>
    /// <param name='item'>
    /// The item to add.
    /// </param>
    public virtual void Add(T item)
    {
      if(this.BeforeAdd(this.WrappedCollection, item))
      {
        _wrapped.Add(item);
        this.AfterAdd(this.WrappedCollection);
      }
    }

    /// <summary>
    /// Clears all items from the current instance.
    /// </summary>
    public virtual void Clear()
    {
      while(_wrapped.Any())
      {
        this.Remove(_wrapped.First());
      }
    }

    /// <summary>
    /// Determines whether the current collection contains a specific value.
    /// </summary>
    /// <param name='item'>
    /// The item to search for.
    /// </param>
    public virtual bool Contains(T item)
    {
      return _wrapped.Contains(item);
    }

    /// <summary>
    /// Copies the contents of the current instance to an array.
    /// </summary>
    /// <param name='array'>
    /// The array to copy to.
    /// </param>
    /// <param name='arrayIndex'>
    /// Array index.
    /// </param>
    public virtual void CopyTo(T[] array, int arrayIndex)
    {
      _wrapped.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Removes the first occurrence of an item from the current collection.
    /// </summary>
    /// <param name='item'>
    /// The item to remove from the current collection.
    /// </param>
    public virtual bool Remove(T item)
    {
      bool output;

      if(this.BeforeRemove(this.WrappedCollection, item))
      {
        output = _wrapped.Remove(item);
        this.AfterRemove(this.WrappedCollection);
      }
      else
      {
        output = false;
      }

      return output;
    }

    /// <summary>
    /// Determines the specified list is the same list that the current instance wraps.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The comparison performed here is one of reference equality.  If the given list is not precisely the same list
    /// that is wrapped by this event bound list instance then <c>false</c> will be returned.
    /// </para>
    /// <para>
    /// This functionality is required to work around the problem-scenario illustrated by issue #20 - whereby the
    /// wrapped list could be substituted without going via the wrapper.  In this scenario the wrapper can become out of
    /// sync with the list that it wraps.  This (a private member being updated without going via a wrapper) would be a
    /// rare scenario if not for frameworks such as NHibernate.  NH intentionally uses reflection to go and manipulate
    /// or replace the backing store without touching the property.
    /// </para>
    /// </remarks>
    /// <returns>
    /// <c>true</c> if the specified list is the same list that this instance wraps; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='compareTo'>
    /// The list to compare to the list that is wrapped by the current instance.
    /// </param>
    public virtual bool IsWrapping(TCollection compareTo)
    {
      return Object.ReferenceEquals(_wrapped, compareTo);
    }

    #endregion

    #region explicit interface implementations

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    void ICollection.CopyTo(Array array, int index)
    {
      T[] copy = new T[_wrapped.Count];
      this.CopyTo(copy, 0);
      Array.Copy(copy, 0, array, index, _wrapped.Count);
    }

    object ICollection.SyncRoot
    {
      get {
        return ((ICollection) _wrapped).SyncRoot;
      }
    }

    bool ICollection.IsSynchronized
    {
      get {
        return ((ICollection) _wrapped).IsSynchronized;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <c>EventBoundListWrapper</c> class.
    /// </summary>
    /// <param name='wrapped'>
    /// The generic <c>ICollection</c> that this instance will wrap.
    /// </param>
    public EventBoundCollectionWrapper(TCollection wrapped)
    {
      if(wrapped == null)
      {
        throw new ArgumentNullException("wrapped");
      }

      _wrapped = wrapped;
    }

    #endregion
  }
}

