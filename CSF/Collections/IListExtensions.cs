//
//  IListExtensions.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2012 Craig Fowler
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
    public static IEventBoundList<T> WrapWithBeforeActions<T>(this IList<T> sourceList,
                                                              Action<T> beforeAdd,
                                                              Action<T> beforeRemove) where T : class
    {
      // See the remarks for IEventBoundList<T> for an important rationale discussion for the generic constraint 'class'

      IEventBoundList<T> output = (sourceList as IEventBoundList<T>)?? new EventBoundListWrapper<T>(sourceList);

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
    public static IEventBoundList<T> WrapWithBeforeActions<T>(this IList<T> sourceList,
                                                              Func<IList<T>, T, bool> beforeAdd,
                                                              Func<IList<T>, T, bool> beforeRemove) where T : class
    {
      // See the remarks for IEventBoundList<T> for an important rationale discussion for the generic constraint 'class'

      IEventBoundList<T> output = (sourceList as IEventBoundList<T>)?? new EventBoundListWrapper<T>(sourceList);

      output.BeforeAdd = beforeAdd;
      output.BeforeRemove = beforeRemove;

      return output;
    }

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

