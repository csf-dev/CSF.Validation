//
//  AdHocGenericIdentityParser.cs
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
  /// Implementation of <see cref="IGenericIdentityParser"/> that constructs throwaway identity parser instances as
  /// required, parses the value and then discards the parser.
  /// </summary>
  public class AdHocGenericIdentityParser : IGenericIdentityParser
  {
    #region IGenericIdentityParser implementation

    /// <summary>
    /// Parses the specified input as an identity instance for the specified entity type.
    /// </summary>
    /// <param name='value'>
    /// The identity value to parse.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of of the entity, whose identity to parse.
    /// </typeparam>
    public IIdentity<TEntity> Parse<TEntity> (object value) where TEntity : IEntity
    {
      var parser = IdentityParser.Create<TEntity>();
      return parser.Parse(value);
    }

    /// <summary>
    /// Attempts to parse the specified input as an identity instance for the specified entity type.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the parsing succeeded; <c>false</c> otherwise.
    /// </returns>
    /// <param name='value'>
    /// The identity value to parse.
    /// </param>
    /// <param name='output'>
    /// The parsed identity.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of of the entity, whose identity to parse.
    /// </typeparam>
    public bool TryParse<TEntity> (object value, out IIdentity<TEntity> output) where TEntity : IEntity
    {
      var parser = IdentityParser.Create<TEntity>();
      return parser.TryParse(value, out output);
    }

    #endregion
  }
}

