//
// OrderNeutralEqualityComparer.cs
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
  /// Equality comparer determines order-neutral equality between collections of items.  That is, that they contain
  /// the same items, irrespective of order.
  /// </summary>
  public class OrderNeutralEqualityComparer<TCollection>
    : IEqualityComparer, IEqualityComparer<IEnumerable<TCollection>>
  {
    #region fields

    private IEqualityComparer<TCollection> _itemComparer;

    #endregion

    #region methods

    /// <summary>
    /// Determines whether two objects are equal, assuming that they are enumerable collections, ignoring the order of
    /// elements.
    /// </summary>
    /// <returns>
    /// Whether or not the objects are equal.
    /// </returns>
    /// <param name='obj1'>
    /// The first object to compare.
    /// </param>
    /// <param name='obj2'>
    /// The second object to compare.
    /// </param>
    public bool AreEqual(object obj1, object obj2)
    {
      if(Object.ReferenceEquals(obj1, obj2))
      {
        return true;
      }

      try
      {
        IEnumerable<TCollection>
          enumerable1 = (IEnumerable<TCollection>) obj1,
          enumerable2 = (IEnumerable<TCollection>) obj2;

        return AreEqual(enumerable1, enumerable2);
      }
      catch(InvalidCastException)
      {
        return false;
      }
    }

    /// <summary>
    /// Determines whether two objects are equal, assuming that they are enumerable collections, ignoring the order of
    /// elements.
    /// </summary>
    /// <returns>
    /// Whether or not the objects are equal.
    /// </returns>
    /// <param name='enumerable1'>
    /// The first object to compare.
    /// </param>
    /// <param name='enumerable2'>
    /// The second object to compare.
    /// </param>
    public bool AreEqual(IEnumerable<TCollection> enumerable1, IEnumerable<TCollection> enumerable2)
    {
      if(Object.ReferenceEquals(enumerable1, enumerable2))
      {
        return true;
      }

      if((object) enumerable1 == null || (object) enumerable2 == null)
      {
        return false;
      }

      return (enumerable1.Count() == enumerable2.Count()
              && enumerable1.OrderBy(x => x).SequenceEqual(enumerable2.OrderBy(x => x), _itemComparer));
    }

    /// <summary>
    /// Unsupported method would get the hash code for an object.
    /// </summary>
    /// <param name='obj1'>
    /// The object for which to get a hash code.
    /// </param>
    public int GetHashCode(object obj1)
    {
      var enumerable = obj1 as IEnumerable<TCollection>;

      if(enumerable != null)
      {
        return GetHashCode(enumerable);
      }

      return (obj1 != null)? obj1.GetHashCode() : 0;
    }

    /// <summary>
    /// Unsupported method would get the hash code for an object.
    /// </summary>
    /// <param name='enumerable'>
    /// The object for which to get a hash code.
    /// </param>
    public int GetHashCode(IEnumerable<TCollection> enumerable)
    {
      return (enumerable != null)? enumerable.OrderBy(x => x).GetHashCode() : 0;
    }

    #endregion

    #region IEqualityComparer implementation

    bool IEqualityComparer.Equals(object obj1, object obj2)
    {
      return AreEqual(obj1, obj2);
    }

    bool IEqualityComparer<IEnumerable<TCollection>>.Equals(IEnumerable<TCollection> enumerable1,
                                                            IEnumerable<TCollection> enumerable2)
    {
      return AreEqual(enumerable1, enumerable2);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Collections.OrderNeutralEqualityComparer`1"/> class.
    /// </summary>
    public OrderNeutralEqualityComparer() : this(null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Collections.OrderNeutralEqualityComparer`1"/> class.
    /// </summary>
    /// <param name="itemComparer">Item comparer.</param>
    public OrderNeutralEqualityComparer(IEqualityComparer<TCollection> itemComparer)
    {
      _itemComparer = itemComparer?? EqualityComparer<TCollection>.Default;
    }

    #endregion
  }
}

