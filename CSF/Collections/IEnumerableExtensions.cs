//  
//  IListInt32Extensions.cs
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
    /// Returns the collection of items as a <see cref="System.String"/>, separated by the given separator.
    /// </summary>
    /// <returns>
    /// A string representation of all of the items within the <paramref name="collection"/>.
    /// </returns>
    /// <param name='collection'>
    /// The collection for which to generate the string representation.
    /// </param>
    /// <param name='separator'>
    /// A separator sequence to appear between every item.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public static string ToSeparatedString(this IEnumerable collection, string separator)
    {
      return CreateSeparatedString(collection.Cast<object>(), separator, x => x);
    }

    /// <summary>
    /// Returns the collection of items as a <see cref="System.String"/>, separated by the given separator.
    /// </summary>
    /// <returns>
    /// A string representation of all of the items within the <paramref name="collection"/>.
    /// </returns>
    /// <param name='collection'>
    /// The collection for which to generate the string representation.
    /// </param>
    /// <param name='separator'>
    /// A separator sequence to appear between every item.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <typeparam name='T'>
    /// The type of object contained within the <paramref name="collection"/>.
    /// </typeparam>
    public static string ToSeparatedString<T>(this IEnumerable<T> collection, string separator)
    {
      return CreateSeparatedString<T>(collection, separator, x => x);
    }

    /// <summary>
    /// Returns the collection of items as a <see cref="System.String"/>, separated by the given separator.
    /// </summary>
    /// <returns>
    /// A string representation of all of the items within the <paramref name="collection"/>.
    /// </returns>
    /// <param name='collection'>
    /// The collection for which to generate the string representation.
    /// </param>
    /// <param name='separator'>
    /// A separator sequence to appear between every item.
    /// </param>
    /// <param name='selector'>
    /// A selector function to convert each item into a string representation.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <typeparam name='T'>
    /// The type of object contained within the <paramref name="collection"/>.
    /// </typeparam>
    public static string ToSeparatedString<T>(this IEnumerable<T> collection, string separator, Func<T,object> selector)
    {
      return CreateSeparatedString<T>(collection, separator, selector);
    }

    #endregion

    #region static methods

    /// <summary>
    /// Returns the collection of items as a <see cref="System.String"/>, separated by the given separator.
    /// </summary>
    /// <returns>
    /// A string representation of all of the items within the <paramref name="collection"/>.
    /// </returns>
    /// <param name='collection'>
    /// The collection for which to generate the string representation.
    /// </param>
    /// <param name='separator'>
    /// A separator sequence to appear between every item.
    /// </param>
    /// <param name='selector'>
    /// A selector function to convert each item into a string representation.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    internal static string CreateSeparatedString<T>(IEnumerable<T> collection,
                                                    string separator,
                                                    Func<T,object> selector)
    {
      StringBuilder output = new StringBuilder();
      
      if(collection == null)
      {
        throw new ArgumentNullException ("collection");
      }
      else if(separator == null)
      {
        throw new ArgumentNullException ("separator");
      }
      else if(selector == null)
      {
        throw new ArgumentNullException("selector");
      }
      
      foreach(T item in collection)
      {
        output.Append(selector(item).ToString());
        output.Append(separator);
      }
      
      if(separator.Length > 0)
      {
        output.Remove(output.Length - separator.Length, separator.Length);
      }
      
      return output.ToString();
    }

    #endregion
  }
}

