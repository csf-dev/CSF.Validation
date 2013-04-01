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
using System.Linq;
using System.Text;
using CSF.Collections;

namespace CSF
{
  /// <summary>
  /// Static helper class containing extension methods for <see cref="IEnumerable"/> types.
  /// </summary>
  public static class IEnumerableExtensions
  {
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
    [Obsolete("This type and its associated extension methods are being moved to the 'CSF.Collections' namespace.")]
    public static string ToSeparatedString(this IEnumerable collection, string separator)
    {
      return CSF.Collections.IEnumerableExtensions.CreateSeparatedString(collection.Cast<object>(),
                                                                         separator,
                                                                         x => x);
    }
  }
}

