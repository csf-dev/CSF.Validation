//  
//  Identity.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2012 CSF Software Limited
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
  /// <para>
  /// A static/helper class that creates <see cref="IIdentity"/> instances from <see cref="IEntity"/> types.
  /// </para>
  /// </summary>
  [Obsolete("There is nothing in this type that is not obsolete.")]
  public static class Identity
  {
    #region methods
    
    /// <summary>
    /// <para>Static convenience method creates a new <see cref="IIdentity"/> for a specified type.</para>
    /// </summary>
    /// <param name="identifier">
    /// An identifier value.
    /// </param>
    /// <typeparam name='TIdentifier'>
    /// The type of the identifier.
    /// </typeparam>
    /// <typeparam name='TEntity'>
    /// The type of the entity that the generated identity relates to.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IIdentity"/>
    /// </returns>
    [Obsolete("There is no reason to use this method, a call to the constructor will do just fine.")]
    public static Identity<TEntity,TIdentifier> Create<TEntity,TIdentifier>(TIdentifier identifier)
      where TEntity : IEntity
    {
      return new Identity<TEntity,TIdentifier>(identifier);
    }

    /// <summary>
    /// <para>Static convenience method creates a new <see cref="IIdentity"/> for a given entity type.</para>
    /// </summary>
    /// <param name='identifier'>
    /// An identifier value.
    /// </param>
    /// <param name='entityType'>
    /// The type of the entity that the generated identity relates to.
    /// </param>
    /// <typeparam name='TIdentifier'>
    /// The type of the identifier.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IIdentity"/>
    /// </returns>
    [Obsolete("This method is obsolete & will be removed in v3.x.  Use the generic overload that takes 2 type params.")]
    public static IIdentity Create<TIdentifier>(TIdentifier identifier, Type entityType)
    {
      return (IIdentity) typeof(Identity<,>)
                           .MakeGenericType(entityType, typeof(TIdentifier))
                           .GetConstructor(new Type[] { typeof(TIdentifier) })
                           .Invoke(new object[] { identifier });
    }

    /// <summary>
    /// Attempts to parse the specified input as an <see cref="IIdentity"/> instance.
    /// </summary>
    /// <returns>
    /// A value that indicates whether the parsing was successful or not.
    /// </returns>
    /// <param name='identifier'>
    /// The identifier to parse.
    /// </param>
    /// <param name='identity'>
    /// If the output of this method is <c>true</c> then this parameter exposes the parsed identity.  Otherwise, its
    /// value is undefined.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of entity that the identity instance is to be for.
    /// </typeparam>
    /// <typeparam name='TIdentifier'>
    /// The target type for the identifier.
    /// </typeparam>
    [Obsolete("This method is obsolete & will be removed in v3.x.  Use the other overload of TryParse instead.")]
    public static bool TryParse<TEntity,TIdentifier>(object identifier, out IIdentity<TEntity,TIdentifier> identity)
      where TEntity : IEntity
    {
      IIdentity<TEntity> output;
      bool success;

      success = IdentityParser.TryParse<TEntity>(identifier, out output);
      identity = (IIdentity<TEntity,TIdentifier>) output;

      return success;
    }

    /// <summary>
    /// Parse the specified input as an <see cref="IIdentity"/> instance.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier to parse.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of entity that the identity instance is to be for.
    /// </typeparam>
    /// <typeparam name='TIdentifier'>
    /// The target type for the identifier.
    /// </typeparam>
    [Obsolete("Use an implementation of the new IIdentityParser instead.")]
    public static Identity<TEntity,TIdentifier> Parse<TEntity,TIdentifier>(object identifier)
      where TEntity : IEntity
    {
      return (Identity<TEntity,TIdentifier>) IdentityParser.Parse<TEntity>(identifier);
    }

    /// <summary>
    /// Attempts to parse the specified input as an <see cref="IIdentity"/> instance.
    /// </summary>
    /// <returns>
    /// A value that indicates whether the parsing was successful or not.
    /// </returns>
    /// <param name='identifier'>
    /// The identifier to parse.
    /// </param>
    /// <param name='identity'>
    /// If the output of this method is <c>true</c> then this parameter exposes the parsed identity.  Otherwise, its
    /// value is undefined.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of entity that the identity instance is to be for.
    /// </typeparam>
    /// <typeparam name='TIdentifier'>
    /// The target type for the identifier.
    /// </typeparam>
    [Obsolete("Use an implementation of the new IIdentityParser instead.")]
    public static bool TryParse<TEntity,TIdentifier>(object identifier, out Identity<TEntity,TIdentifier> identity)
      where TEntity : IEntity
    {
      IIdentity<TEntity> output;
      bool success;

      success = IdentityParser.TryParse<TEntity>(identifier, out output);
      identity = (Identity<TEntity,TIdentifier>) output;

      return success;
    }

    /// <summary>
    /// Attempts to parse the specified input as an <see cref="IIdentity"/> instance.
    /// </summary>
    /// <returns>
    /// A value that indicates whether the parsing was successful or not.
    /// </returns>
    /// <param name='identifier'>
    /// The identifier to parse.
    /// </param>
    /// <param name='identity'>
    /// If the output of this method is <c>true</c> then this parameter exposes the parsed identity.  Otherwise, it is a
    /// null reference.
    /// </param>
    /// <typeparam name='TEntity'>
    /// The type of entity that the identity instance is to be for.
    /// </typeparam>
    /// <typeparam name='TIdentifier'>
    /// The target type for the identifier.
    /// </typeparam>
    [Obsolete("Use an implementation of the new IIdentityParser instead.")]
    public static bool TryParse<TEntity,TIdentifier>(object identifier, out IIdentity<TEntity> identity)
      where TEntity : IEntity
    {
      return IdentityParser.TryParse<TEntity>(identifier, out identity);
    }

    #endregion
  }
}

