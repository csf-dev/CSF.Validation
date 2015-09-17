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
  /// Determines whether two enumerations containing a given type are equal.  The order of items in the collections
  /// does not matter for this comparison.
  /// </summary>
  public class OrderNeutralEqualityComparer<TCollection> : IEqualityComparer
  {
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
      bool output;

      if(Object.ReferenceEquals(obj1, obj2))
      {
        output = true;
      }
      else if(obj1 == null)
      {
        output = false;
      }
      else
      {
        try
        {
          IEnumerable<TCollection>
            enumerable1 = (IEnumerable<TCollection>) obj1,
            enumerable2 = (IEnumerable<TCollection>) obj2;

          output = (enumerable1.Count() == enumerable2.Count()
                    && enumerable1.Except(enumerable2).Count() == 0);
        }
        catch(InvalidCastException)
        {
          output = obj1.Equals(obj2);
        }
      }

      return output;
    }

    /// <summary>
    /// Unsupported method would get the hash code for an object.
    /// </summary>
    /// <param name='obj1'>
    /// Obj1.
    /// </param>
    public int GetHashCode(object obj1)
    {
      throw new NotSupportedException();
    }

    #endregion

    #region IEqualityComparer implementation

    bool IEqualityComparer.Equals(object obj1, object obj2)
    {
      return this.AreEqual(obj1, obj2);
    }

    #endregion
  }
}

