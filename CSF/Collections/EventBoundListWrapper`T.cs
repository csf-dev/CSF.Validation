//
//  EventBoundListWrapper.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2012 Craig Fowler
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Collections
{
  /// <summary>
  /// Implementation of a generic <c>IEventBoundList</c> that wraps a normal generic <c>IList</c> instance.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This type is very heavily based on the excellent work found at:
  /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
  /// </para>
  /// </remarks>
  [Serializable]
  public class EventBoundListWrapper<T> : IEventBoundList<T> where T : class
  {
    // See the remarks for IEventBoundList<T> for an important rationale discussion for the generic constraint 'class'.

    #region fields

    private readonly IList<T> _wrapped;
    private Action<IList<T>> _afterAdd, _afterRemove;
    private Func<IList<T>, T, bool> _beforeAdd, _beforeRemove;

    #endregion

    #region event-binding properties

    /// <summary>
    /// Gets or sets the action to perform after an item is added to this collection. 
    /// </summary>
    /// <value>
    /// The action to perform after item addition. 
    /// </value>
    public virtual Action<IList<T>> AfterAdd
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
    public virtual Action<IList<T>> AfterRemove
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
    public virtual Func<IList<T>, T, bool> BeforeAdd
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
    public virtual Func<IList<T>, T, bool> BeforeRemove
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
      if(this.BeforeRemove(this, item))
      {
        this.AfterRemove(this);
      }
    }

    /// <summary>
    /// Detaches the item at the specified index from the list as if it had been removed but does not actually remove
    /// it from the underlying list. 
    /// </summary>
    /// <param name='index'>
    /// The numeric index of the item to detach 
    /// </param>
    public virtual void DetachAt(int index)
    {
      if(this.BeforeRemove(this, _wrapped[index]))
      {
        this.AfterRemove(this);
      }
    }

    /// <summary>
    /// Detaches all contained items from the list as if they had been removed, without actually removing them from the
    /// underlying list. 
    /// </summary>
    public virtual void DetachAll()
    {
      for(int i = _wrapped.Count - 1; i >=0; i--)
      {
        this.DetachAt(i);
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
    /// Gets or sets the item at the specified index.
    /// </summary>
    /// <param name='index'>
    /// The numeric index to get/set to/from.
    /// </param>
    public virtual T this[int index]
    {
      get {
        return _wrapped[index];
      }
      set {
        /* This setter is a problem for us because the BeforeAdd function must be executed against a copy of the wrapped
         * list that does not include an item at the desired index.  So - we make a copy of it first for that function.
         */
        var copiedList = new List<T>(_wrapped);
        copiedList.RemoveAt(index);

        if(this.BeforeAdd(copiedList, value))
        {
          _wrapped[index] = value;
          this.AfterAdd(this);
        }
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
      if(this.BeforeAdd(this, item))
      {
        _wrapped.Add(item);
        this.AfterAdd(this);
      }
    }

    /// <summary>
    /// Clears all items from the current instance.
    /// </summary>
    public virtual void Clear()
    {
      while(_wrapped.Any())
      {
        this.RemoveAt(0);
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

      if(this.BeforeRemove(this, item))
      {
        output = _wrapped.Remove(item);
        this.AfterRemove(this);
      }
      else
      {
        output = false;
      }

      return output;
    }

    /// <summary>
    /// Removes the item at the given <paramref name="index"/> from this collection.
    /// </summary>
    /// <param name='index'>
    /// The index at which to remove the item.
    /// </param>
    public virtual void RemoveAt(int index)
    {
      if(this.BeforeRemove(this, _wrapped[index]))
      {
        _wrapped.RemoveAt(index);
        this.AfterRemove(this);
      }
    }

    /// <summary>
    /// Determines the index of a specific item in the current instance.
    /// </summary>
    /// <returns>
    /// The numeric index of the item.
    /// </returns>
    /// <param name='item'>
    /// The item to search for.
    /// </param>
    public virtual int IndexOf(T item)
    {
      return _wrapped.IndexOf(item);
    }

    /// <summary>
    /// Inserts an item into the current collection at the specified index.
    /// </summary>
    /// <param name='index'>
    /// The index at which to insert the item.
    /// </param>
    /// <param name='item'>
    /// The item to insert.
    /// </param>
    public virtual void Insert(int index, T item)
    {
      if(this.BeforeAdd(this, item))
      {
        _wrapped.Insert(index, item);
        this.AfterAdd(this);
      }
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
    public virtual bool IsWrappedList(IList<T> compareTo)
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
    /// The generic <c>IList</c> that this instance will wrap.
    /// </param>
    public EventBoundListWrapper(IList<T> wrapped)
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

