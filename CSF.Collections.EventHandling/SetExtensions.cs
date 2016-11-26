//
// ISetExtensions.cs
//
// Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
// Copyright (c) 2016 Craig Fowler
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
using System.Linq;
using System.Collections.Generic;

namespace CSF.Collections.EventHandling
{
  /// <summary>
  /// Extension methods to the generic <c>ISet</c> type.
  /// </summary>
  public static class SetExtensions
  {
    /// <summary>
    /// Creates a wrapped copy of the <paramref name="sourceCollection"/> and associates a pair of actions with it that
    /// are executed upon items before they are added/removed to/from the set.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is very heavily based on the excellent work found at:
    /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The wrapped copy of the set, with the new behaviour added.
    /// </returns>
    /// <param name='sourceCollection'>
    /// The source <c>ISet</c>, which is unmodified in the process.
    /// </param>
    /// <param name='beforeAdd'>
    /// An action to perform upon an item before it is added to the set.
    /// </param>
    /// <param name='beforeRemove'>
    /// An action to perform upon an item before it is removed from the set.
    /// </param>
    /// <typeparam name='T'>
    /// The type of object contained within the set.
    /// </typeparam>
    public static EventBoundSetWrapper<T> WrapWithBeforeActions<T>(this ISet<T> sourceCollection,
                                                                   Action<T> beforeAdd,
                                                                   Action<T> beforeRemove)
      where T : class
    {
      // See the remarks for IEventBoundList<T> for an important rationale discussion for the generic constraint 'class'

      EventBoundSetWrapper<T> output;
      output = (sourceCollection as EventBoundSetWrapper<T>)?? new EventBoundSetWrapper<T>(sourceCollection);

      output.BeforeAdd = (set, item) => {
        beforeAdd(item);
        return true;
      };
      output.BeforeRemove = (set, item) => {
        beforeRemove(item);
        return true;
      };

      return output;
    }

    /// <summary>
    /// Creates a wrapped copy of the <paramref name="sourceCollection"/> and associates a pair of functions with it
    /// that are executed upon items before they are added/removed to/from the set.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is very heavily based on the excellent work found at:
    /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The wrapped copy of the set, with the new behaviour added.
    /// </returns>
    /// <param name='sourceCollection'>
    /// The source <c>ISet</c>, which is unmodified in the process.
    /// </param>
    /// <param name='beforeAdd'>
    /// A function to execute before an item is added to the set.  If the function returns false then addition is
    /// aborted.
    /// </param>
    /// <param name='beforeRemove'>
    /// A function to execute before an item  is removed from the set.  If the function returns false then addition is
    /// aborted.
    /// </param>
    /// <typeparam name='T'>
    /// The type of object contained within the set.
    /// </typeparam>
    public static EventBoundSetWrapper<T> WrapWithBeforeActions<T>(this ISet<T> sourceCollection,
                                                                   Func<ISet<T>, T, bool> beforeAdd,
                                                                   Func<ISet<T>, T, bool> beforeRemove)
      where T : class
    {
      // See the remarks for IEventBoundList<T> for an important rationale discussion for the generic constraint 'class'

      EventBoundSetWrapper<T> output;
      output = (sourceCollection as EventBoundSetWrapper<T>)?? new EventBoundSetWrapper<T>(sourceCollection);

      output.BeforeAdd = beforeAdd;
      output.BeforeRemove = beforeRemove;

      return output;
    }
  }
}

