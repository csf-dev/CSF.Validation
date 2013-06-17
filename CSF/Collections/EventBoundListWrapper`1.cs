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
  /// Implementation of a generic <c>EventBoundCollectionWrapper</c> that wraps a normal generic <c>IList</c> instance.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This type is very heavily based on the excellent work found at:
  /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
  /// </para>
  /// </remarks>
  [Serializable]
#pragma warning disable 618
  public class EventBoundListWrapper<T> : EventBoundCollectionWrapper<IList<T>,T>, IEventBoundList<T>, IList<T>
#pragma warning restore 618
    where T : class
  {
    // See the remarks for IEventBoundList<T> for an important rationale discussion for the generic constraint 'class'.

    #region event-binding properties

    /// <summary>
    /// Detaches the item at the specified index from the list as if it had been removed but does not actually remove
    /// it from the underlying list. 
    /// </summary>
    /// <param name='index'>
    /// The numeric index of the item to detach 
    /// </param>
    public virtual void DetachAt(int index)
    {
      if(this.BeforeRemove(this, base.WrappedCollection[index]))
      {
        this.AfterRemove(this);
      }
    }

    #endregion

    #region list properties

    /// <summary>
    /// Gets or sets the item at the specified index.
    /// </summary>
    /// <param name='index'>
    /// The numeric index to get/set to/from.
    /// </param>
    public virtual T this[int index]
    {
      get {
        return base.WrappedCollection[index];
      }
      set {
        /* This setter is a problem for us because the BeforeAdd function must be executed against a copy of the wrapped
         * list that does not include an item at the desired index.  So - we make a copy of it first for that function.
         */
        var copiedList = new List<T>(base.WrappedCollection);
        copiedList.RemoveAt(index);

        if(this.BeforeAdd(copiedList, value))
        {
          base.WrappedCollection[index] = value;
          this.AfterAdd(this);
        }
      }
    }

    #endregion

    #region list methods

    /// <summary>
    /// Removes the item at the given <paramref name="index"/> from this collection.
    /// </summary>
    /// <param name='index'>
    /// The index at which to remove the item.
    /// </param>
    public virtual void RemoveAt(int index)
    {
      if(this.BeforeRemove(this, base.WrappedCollection[index]))
      {
        base.WrappedCollection.RemoveAt(index);
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
      return base.WrappedCollection.IndexOf(item);
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
        base.WrappedCollection.Insert(index, item);
        this.AfterAdd(this);
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
    public EventBoundListWrapper(IList<T> wrapped) : base(wrapped) {}

    #endregion
  }
}

