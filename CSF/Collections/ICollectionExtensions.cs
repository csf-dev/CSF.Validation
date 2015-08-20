//
// ICollectionExtensions.cs
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

namespace CSF.Collections
{
  /// <summary>
  /// Extension methods for <c>ICollection</c> instances.
  /// </summary>
  public static class ICollectionExtensions
  {
    /// <summary>
    /// Replaces the contents of the given generic <c>ICollection</c> with a given enumerable collection of the same
    /// type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use this method as an alternative to an assignment operator when an assignment would not be a legal thing to do.
    /// For example, if the source collection is a read-only property, or there is an important reason that the source
    /// collection remains the same instance.
    /// </para>
    /// <para>
    /// This method essentially just clears the source collection and then enumerates through the replacement, adding
    /// every item found.
    /// </para>
    /// </remarks>
    /// <param name='sourceCollection'>
    /// The collection to have its contents replaced.
    /// </param>
    /// <param name='replacementCollection'>
    /// A collection holding the replacement elements.
    /// </param>
    /// <typeparam name='T'>
    /// The type of item contained by the collection.
    /// </typeparam>
    public static void ReplaceContents<T>(this ICollection<T> sourceCollection, IEnumerable<T> replacementCollection)
    {
      if(sourceCollection == null)
      {
        throw new ArgumentNullException("sourceCollection");
      }
      else if(replacementCollection == null)
      {
        throw new ArgumentNullException("replacementCollection");
      }

      sourceCollection.Clear();
      foreach(T item in replacementCollection)
      {
        sourceCollection.Add(item);
      }
    }
    /// <summary>
    /// Creates a wrapped copy of the <paramref name="sourceList"/> and associates a pair of actions with it that are
    /// executed upon items before they are added/removed to/from the list.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is very heavily based on the excellent work found at:
    /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The wrapped copy of the list, with the new behaviour added.
    /// </returns>
    /// <param name='sourceList'>
    /// The source <c>IList</c>, which is unmodified in the process.
    /// </param>
    /// <param name='beforeAdd'>
    /// An action to perform upon an item before it is added to the list.
    /// </param>
    /// <param name='beforeRemove'>
    /// An action to perform upon an item before it is removed from the list.
    /// </param>
    /// <typeparam name='T'>
    /// The type of object contained within the list.
    /// </typeparam>
    public static EventBoundCollectionWrapper<T> WrapWithBeforeActions<T>(this ICollection<T> sourceList,
                                                                    Action<T> beforeAdd,
                                                                    Action<T> beforeRemove)
      where T : class
    {
      // See the remarks for IEventBoundList<T> for an important rationale discussion for the generic constraint 'class'

      EventBoundCollectionWrapper<T> output;
      output = (sourceList as EventBoundCollectionWrapper<T>)?? new EventBoundCollectionWrapper<T>(sourceList);

      output.BeforeAdd = (list, item) => {
        beforeAdd(item);
        return true;
      };
      output.BeforeRemove = (list, item) => {
        beforeRemove(item);
        return true;
      };

      return output;
    }

    /// <summary>
    /// Creates a wrapped copy of the <paramref name="sourceList"/> and associates a pair of functions with it that are
    /// executed upon items before they are added/removed to/from the list.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is very heavily based on the excellent work found at:
    /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The wrapped copy of the list, with the new behaviour added.
    /// </returns>
    /// <param name='sourceList'>
    /// The source <c>IList</c>, which is unmodified in the process.
    /// </param>
    /// <param name='beforeAdd'>
    /// A function to execute before an item is added to the list.  If the function returns false then addition is
    /// aborted.
    /// </param>
    /// <param name='beforeRemove'>
    /// A function to execute before an item  is removed from the list.  If the function returns false then addition is
    /// aborted.
    /// </param>
    /// <typeparam name='T'>
    /// The type of object contained within the list.
    /// </typeparam>
    public static EventBoundCollectionWrapper<T> WrapWithBeforeActions<T>(this ICollection<T> sourceList,
                                                                    Func<ICollection<T>, T, bool> beforeAdd,
                                                                    Func<ICollection<T>, T, bool> beforeRemove)
      where T : class
    {
      // See the remarks for IEventBoundList<T> for an important rationale discussion for the generic constraint 'class'

      EventBoundCollectionWrapper<T> output;
      output = (sourceList as EventBoundCollectionWrapper<T>)?? new EventBoundCollectionWrapper<T>(sourceList);

      output.BeforeAdd = beforeAdd;
      output.BeforeRemove = beforeRemove;

      return output;
    }
  }
}

