//
// EventBoundListWrapper`1.cs
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

namespace CSF.Collections.EventHandling
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
  public class EventBoundListWrapper<T> : EventBoundCollectionWrapper<IList<T>,T>, IList<T>
    where T : class
  {
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

