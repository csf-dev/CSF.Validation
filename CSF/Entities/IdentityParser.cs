//
//  IdentityParser.cs
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
using System.Linq;

namespace CSF.Entities
{
  /// <summary>
  /// Static type designed to create identity parser instances.
  /// </summary>
  public class IdentityParser
  {
    #region methods

    /// <summary>
    /// Creates and returns an identity parser instance, suitable for parsing identities of the specified entity type.
    /// </summary>
    /// <typeparam name='TEntity'>
    /// The type of of the entity, for which to create a parser.
    /// </typeparam>
    public static IIdentityParser<TEntity> Create<TEntity>() where TEntity : IEntity
    {
      Type
        entityType = typeof(TEntity),
        desiredInterface = typeof(IEntity<,>);

      var interfaces = entityType.GetInterfaces().Where(x => x.IsGenericType
                                                             && x.GetGenericTypeDefinition() == desiredInterface);

      if(!interfaces.Any())
      {
        string message = String.Format("The entity type must implement IEntity<TEntity,TIdentity> precisely once in " +
                                       "its inheritance hierarchy.  The entity type '{0}' does not implement it.",
                                       entityType.FullName);
        throw new InvalidOperationException(message);
      }
      else if(interfaces.Count() > 1)
      {
        string message = String.Format("The entity type must implement IEntity<TEntity,TIdentity> precisely once in " +
                                       "its inheritance hierarchy.  The entity type '{0}' implements it more than once.",
                                       entityType.FullName);
        throw new InvalidOperationException(message);
      }

      Type
        entityInterface = interfaces.First(),
        interfaceEntityType = entityInterface.GetGenericArguments()[0],
        interfaceIdentityType = entityInterface.GetGenericArguments()[1];

      if(entityType != interfaceEntityType)
      {
        string message = String.Format("The Create<TEntity>() method must be called with the type parameter indicating " +
                                       "the root 'entity type' in an inheritance hierarchy.  The entity type '{0}' is " +
                                       "not such a root entity type.",
                                       entityType.FullName);
        throw new InvalidOperationException(message);
      }

      Type parserType = typeof(IdentityParser<,>).MakeGenericType(entityType, interfaceIdentityType);
      return (IIdentityParser<TEntity>) Activator.CreateInstance(parserType);
    }

    /// <summary>
    /// Convenience method parses the specified input as an identity instance.
    /// </summary>
    /// <param name='input'>
    /// The identity value to parse.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of of the entity, whose identity to parse.
    /// </typeparam>
    public static IIdentity<TEntity> Parse<TEntity>(object input) where TEntity : IEntity
    {
      var parser = Create<TEntity>();
      return parser.Parse(input);
    }

    /// <summary>
    /// Convenience method attempts to parse the specified input as an identity instance.
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
    /// <typeparam name='TEntity'>
    /// The type of of the entity, whose identity to parse.
    /// </typeparam>
    public static bool TryParse<TEntity>(object input, out IIdentity<TEntity> output) where TEntity : IEntity
    {
      var parser = Create<TEntity>();
      return parser.TryParse(input, out output);
    }

    #endregion
  }
}

