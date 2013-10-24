//
//  CachingGenericIdentityParser.cs
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

namespace CSF.Entities
{
  /// <summary>
  /// An implementation of <see cref="IGenericIdentityParser"/> that caches created identity parser instances, reusing
  /// them where appropriate.
  /// </summary>
  public class CachingGenericIdentityParser : IGenericIdentityParser
  {
    #region fields

    private Dictionary<Type, object> _parsers;
    private object _syncRoot;

    #endregion

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
      var parser = this.GetParser<TEntity>();
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
      var parser = this.GetParser<TEntity>();
      return parser.TryParse(value, out output);
    }

    /// <summary>
    /// Gets an identity parser instance appropriate for parsing identities of the given type.
    /// </summary>
    /// <returns>
    /// The identity parser.
    /// </returns>
    /// <typeparam name='TEntity'>
    /// The type of of the entity, whose identity to parse.
    /// </typeparam>
    public IIdentityParser<TEntity> GetParser<TEntity>() where TEntity : IEntity
    {
      Type entityType = typeof(TEntity);

      lock(_syncRoot)
      {
        if(!_parsers.ContainsKey(entityType))
        {
          _parsers.Add(entityType, IdentityParser.Create<TEntity>());
        }
      }

      return (IIdentityParser<TEntity>) _parsers[entityType];
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Entities.CachingGenericIdentityParser"/> class.
    /// </summary>
    public CachingGenericIdentityParser()
    {
      _parsers = new Dictionary<Type, object>();
      _syncRoot = new object();
    }

    #endregion
  }
}

