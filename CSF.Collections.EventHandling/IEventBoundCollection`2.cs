//
// IEventBoundCollection`2.cs
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

namespace CSF.Collections.EventHandling
{
  /// <summary>
  /// Interface for a type provides an event-bound collection.
  /// </summary>
  public interface IEventBoundCollection<TCollection,TItem> : ICollection<TItem>, ICollection
    where TCollection : ICollection<TItem>
    where TItem : class
  {
    #region properties

    /// <summary>
    /// Gets or sets the action to perform after an item is added to this collection.
    /// </summary>
    /// <value>
    /// The action to perform after item addition.
    /// </value>
    Action<TCollection> AfterAdd { get; set; }

    /// <summary>
    /// Gets or sets the action to perform after an item is removed from this collection.
    /// </summary>
    /// <value>
    /// The action to perform after item removal.
    /// </value>
    Action<TCollection> AfterRemove { get; set; }

    /// <summary>
    /// Gets or sets a function to execute before an item is added to this collection.  If the return value of that
    /// function is false then adding the item is aborted.
    /// </summary>
    /// <value>
    /// The function to execute before item addition.
    /// </value>
    Func<TCollection, TItem, bool> BeforeAdd { get; set; }

    /// <summary>
    /// Gets or sets a function to execute before an item is removed from this collection.  If the return value of that
    /// function is false then removing the item is aborted.
    /// </summary>
    /// <value>
    /// The function to execute before item removal.
    /// </value>
    Func<TCollection, TItem, bool> BeforeRemove { get; set; }

    #endregion

    #region methods

    /// <summary>
    /// Detaches the specified item from the list as if it had been removed but does not actually remove it from the
    /// underlying list.
    /// </summary>
    /// <param name='item'>
    /// The item to detach
    /// </param>
    void Detach(TItem item);

    /// <summary>
    /// Detaches all contained items from the list as if they had been removed, without actually removing
    /// them from the underlying list.
    /// </summary>
    void DetachAll();

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
    bool IsWrapping(TCollection compareTo);

    #endregion
  }
}

