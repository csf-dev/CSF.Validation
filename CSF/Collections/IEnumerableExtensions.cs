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
    /// <remarks>
    /// <para>
    /// This comparison returns true if the two collections both contain equal items, in the same quantity.  The order
    /// of those items is irrelevant.
    /// </para>
    /// <example>
    /// <para>
    /// If <c>source</c> contains the items <c>A, A, B, C, D</c> and <c>compareWith</c> contains the items
    /// <c>D, A, B, C, A</c> then this method will return <c>true</c> because (ignoring order) the collections contain
    /// the same items.
    /// </para>
    /// <para>
    /// If <c>source</c> contains the items <c>A, A, B, C, D</c> and <c>compareWith</c> contains the items
    /// <c>D, A, B, C, B</c> then this method will return <c>false</c>.  This is because the item <c>B</c> appears
    /// twice in <c>compareWith</c> and only once in <c>source</c>, also the item <c>A</c> appears only once in
    /// <c>compareWith</c> but twice in <c>source</c>.
    /// </para>
    /// <para>
    /// If <c>source</c> contains the items <c>A, B, C</c> and <c>compareWith</c> contains the items
    /// <c>D, A, B</c> then this method will return <c>false</c>.  This is because the two collections to not carry the
    /// same items.
    /// </para>
    /// </example>
    /// <para>
    /// This overload uses the default equality comparer to determine item-equality.
    /// </para>
    /// <para>
    /// This method is very heavily-based on the excellent work found at this StackOverflow answer:
    /// http://stackoverflow.com/a/3670089
    /// </para>
    /// </remarks>
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
      return AreContentsSame<T>(source, compareWith, new Dictionary<T,int>());
    }

    /// <summary>
    /// Determines whether the contents of the <paramref name="source"/> collection are the same as the contents of the
    /// collection to <paramref name="compareWith"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This comparison returns true if the two collections both contain equal items, in the same quantity.  The order
    /// of those items is irrelevant.
    /// </para>
    /// <example>
    /// <para>
    /// If <c>source</c> contains the items <c>A, A, B, C, D</c> and <c>compareWith</c> contains the items
    /// <c>D, A, B, C, A</c> then this method will return <c>true</c> because (ignoring order) the collections contain
    /// the same items.
    /// </para>
    /// <para>
    /// If <c>source</c> contains the items <c>A, A, B, C, D</c> and <c>compareWith</c> contains the items
    /// <c>D, A, B, C, B</c> then this method will return <c>false</c>.  This is because the item <c>B</c> appears
    /// twice in <c>compareWith</c> and only once in <c>source</c>, also the item <c>A</c> appears only once in
    /// <c>compareWith</c> but twice in <c>source</c>.
    /// </para>
    /// <para>
    /// If <c>source</c> contains the items <c>A, B, C</c> and <c>compareWith</c> contains the items
    /// <c>D, A, B</c> then this method will return <c>false</c>.  This is because the two collections to not carry the
    /// same items.
    /// </para>
    /// </example>
    /// <para>
    /// This overload uses a specified equality comparer to determine equality between items.
    /// </para>
    /// <para>
    /// This method is very heavily-based on the excellent work found at this StackOverflow answer:
    /// http://stackoverflow.com/a/3670089
    /// </para>
    /// </remarks>
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
      return AreContentsSame<T>(source, compareWith, new Dictionary<T,int>(equalityComparer));
    }

    #endregion

    #region static methods

    /// <summary>
    /// Determines whether the contents of the <paramref name="firstCollection"/> collection are the same as the
    /// contents of the collection to <paramref name="secondCollection"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This comparison returns true if the two collections both contain equal items, in the same quantity.  The order
    /// of those items is irrelevant.
    /// </para>
    /// <example>
    /// <para>
    /// If <c>firstCollection</c> contains the items <c>A, A, B, C, D</c> and <c>secondCollection</c> contains the items
    /// <c>D, A, B, C, A</c> then this method will return <c>true</c> because (ignoring order) the collections contain
    /// the same items.
    /// </para>
    /// <para>
    /// If <c>firstCollection</c> contains the items <c>A, A, B, C, D</c> and <c>secondCollection</c> contains the items
    /// <c>D, A, B, C, B</c> then this method will return <c>false</c>.  This is because the item <c>B</c> appears
    /// twice in <c>secondCollection</c> and only once in <c>firstCollection</c>, also the item <c>A</c> appears only
    /// once in <c>secondCollection</c> but twice in <c>firstCollection</c>.
    /// </para>
    /// <para>
    /// If <c>firstCollection</c> contains the items <c>A, B, C</c> and <c>secondCollection</c> contains the items
    /// <c>D, A, B</c> then this method will return <c>false</c>.  This is because the two collections to not carry the
    /// same items.
    /// </para>
    /// </example>
    /// <para>
    /// This method is very heavily-based on the excellent work found at this StackOverflow answer:
    /// http://stackoverflow.com/a/3670089
    /// </para>
    /// </remarks>
    /// <returns>
    /// <c>true</c> if the two collections contain the same items; <c>false</c> otherwise.
    /// </returns>
    /// <param name='firstCollection'>
    /// The first collection to analyse.
    /// </param>
    /// <param name='secondCollection'>
    /// The second collection to analyse.
    /// </param>
    /// <param name='itemCounter'>
    /// An <c>IDictionary</c> implementation (using an appropriate equality comparer) used to count the instances of
    /// each item found in the collections.
    /// </param>
    /// <typeparam name='T'>
    /// The type of item contained within the collections.
    /// </typeparam>
    internal static bool AreContentsSame<T>(IEnumerable<T> firstCollection,
                                            IEnumerable<T> secondCollection,
                                            IDictionary<T,int> itemCounter)
    {
      if(firstCollection == null)
      {
        throw new ArgumentNullException("firstCollection");
      }
      else if(secondCollection == null)
      {
        throw new ArgumentNullException("secondCollection");
      }
      else if(itemCounter == null)
      {
        throw new ArgumentNullException("instanceCounter");
      }

      bool? output = null;

      if(Object.ReferenceEquals(firstCollection, secondCollection))
      {
        output = true;
      }
      else
      {
        foreach(T item in firstCollection)
        {
          if(itemCounter.ContainsKey(item))
          {
            itemCounter[item] ++;
          }
          else
          {
            itemCounter.Add(item, 1);
          }
        }

        foreach(T item in secondCollection)
        {
          if(itemCounter.ContainsKey(item))
          {
            itemCounter[item] --;
          }
          else
          {
            output = false;
            break;
          }
        }

        if(!output.HasValue)
        {
          output = itemCounter.Values.All(count => count == 0);
        }
      }

      return output.Value;
    }

    #endregion
  }
}

