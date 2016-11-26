//
// IEnumerableExtensions.cs
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
using System.Text;

namespace CSF.Collections
{
  /// <summary>
  /// Static helper class containing extension methods for <see cref="IEnumerable"/> types.
  /// </summary>
  public static class IEnumerableExtensions
  {
    #region extension methods

    /// <summary>
    /// Determines whether the contents of the <paramref name="source"/> collection are the same as the contents of the
    /// collection to <paramref name="compareWith"/>.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the two collections contain the same items; <c>false</c> otherwise.
    /// </returns>
    /// <param name='source'>
    /// The source collection to analyse.
    /// </param>
    /// <param name='compareWith'>
    /// The collection to compare with.
    /// </param>
    /// <typeparam name='T'>
    /// The type of item contained within the collections.
    /// </typeparam>
    public static bool AreContentsSameAs<T>(this IEnumerable<T> source, IEnumerable<T> compareWith)
    {
      return source.AreContentsSameAs<T>(compareWith, null);
    }

    /// <summary>
    /// Determines whether the contents of the <paramref name="source"/> collection are the same as the contents of the
    /// collection to <paramref name="compareWith"/>.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the two collections contain the same items; <c>false</c> otherwise.
    /// </returns>
    /// <param name='source'>
    /// The source collection to analyse.
    /// </param>
    /// <param name='compareWith'>
    /// The collection to compare with.
    /// </param>
    /// <param name='equalityComparer'>
    /// The equality comparer to be used to determine equality between items.
    /// </param>
    /// <typeparam name='T'>
    /// The type of item contained within the collections.
    /// </typeparam>
    public static bool AreContentsSameAs<T>(this IEnumerable<T> source,
                                            IEnumerable<T> compareWith,
                                            IEqualityComparer<T> equalityComparer)
    {
      var comparer = new OrderNeutralEqualityComparer<T>(equalityComparer);
      return comparer.AreEqual(source, compareWith);
    }

    #endregion
  }
}

