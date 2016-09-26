//
// IListExtensions.cs
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
using System.Collections.Generic;
using System.Linq;

namespace CSF.Collections
{
  /// <summary>
  /// Extension methods to the generic <c>IList</c> type.
  /// </summary>
  public static class IListExtensions
  {
    /// <summary>
    /// Returns the <paramref name="source"/> list as a readonly collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the <paramref name="source"/> collection is already a read-only collection then the source is
    /// returned by this method, with no further action.
    /// </para>
    /// <para>
    /// If the source list is not read-only then an element-by-element copy is made and returned, leaving the original
    /// list intact and unmodified.  Subsequent alterations to the source list will have no affect upon the copy.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A read-only 'snapshot' of the <paramref name="source"/> collection.
    /// </returns>
    /// <param name='source'>
    /// The collection to make read-only.
    /// </param>
    /// <typeparam name='T'>
    /// The type of the list contents.
    /// </typeparam>
    public static IList<T> ToReadOnlyList<T>(this IList<T> source)
    {
      if(source == null)
      {
        throw new ArgumentNullException("source");
      }

      IList<T> output;

      if(source.IsReadOnly)
      {
        output = source;
      }
      else
      {
        T[] clone = new T[source.Count];
        source.CopyTo(clone, 0);
        output = clone;
      }

      return output;
    }
  }
}

