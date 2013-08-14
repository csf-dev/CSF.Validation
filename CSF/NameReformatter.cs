//
//  NamingFormatter.cs
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

namespace CSF
{
  /// <summary>
  /// Naming formatter type, reformats a name using the naming convention rules defined by the implementation type.
  /// </summary>
  public abstract class NameReformatter
  {
    #region methods

    /// <summary>
    /// Formats the specified name using the rules defined by the current implementation.
    /// </summary>
    /// <param name='objectName'>
    /// The name to re-format.
    /// </param>
    public abstract string Format(string objectName);

    /// <summary>
    /// Concatenates and formats a number of object names, using the rules defined by the current implementation.
    /// </summary>
    /// <param name='objectNames'>
    /// A collection of the object names to re-format.
    /// </param>
    public virtual string Format(params string[] objectNames)
    {
      return this.Format(String.Concat(objectNames));
    }

    #endregion
  }
}

