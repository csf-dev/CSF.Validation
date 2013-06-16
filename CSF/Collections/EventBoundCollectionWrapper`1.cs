//
//  EventBoundCollectionWrapper.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2013 Craig Fowler
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

namespace CSF.Collections
{
  /// <summary>
  /// Convenience implementation of a generic <c>EventBoundCollectionWrapper&lt;TCollection,TItem&gt;</c> that works for
  /// a plain <c>ICollection&lt;TItem&gt;</c>.
  /// </summary>
  public class EventBoundCollectionWrapper<T> : EventBoundCollectionWrapper<ICollection<T>,T>
    where T : class
  {
    #region constructor

    /// <summary>
    /// Initializes a new instance of the <c>EventBoundCollectionWrapper</c> class.
    /// </summary>
    /// <param name='wrapped'>
    /// The generic <c>ICollection</c> that this instance will wrap.
    /// </param>
    public EventBoundCollectionWrapper(ICollection<T> wrapped) : base(wrapped) {}

    #endregion
  }
}

