//
//  ICollectionExtensions.cs
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
  }
}

