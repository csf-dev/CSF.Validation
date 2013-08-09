//
//  IIdentityParser.cs
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

namespace CSF.Entities
{
  /// <summary>
  /// Interface for a type that is capable of parsing identity instances for a given entity type.
  /// </summary>
  public interface IIdentityParser<TEntity> where TEntity : IEntity
  {
    #region methods

    /// <summary>
    /// Parse the specified input as an identity instance.
    /// </summary>
    /// <param name='input'>
    /// The identity value to parse.
    /// </param>
    IIdentity<TEntity> Parse(object input);

    /// <summary>
    /// Attempts to parse the specified input as an identity instance.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the parsing succeeded; <c>false</c> otherwise.
    /// </returns>
    /// <param name='input'>
    /// The identity value to parse.
    /// </param>
    /// <param name='output'>
    /// The parsed identity.
    /// </param>
    bool TryParse(object input, out IIdentity<TEntity> output);

    #endregion
  }
}

