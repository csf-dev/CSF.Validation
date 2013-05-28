//  
//  IEntity`T.cs
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
  /// Generic interface to describe an entity that is serialisable to a persistent data source such as a database.
  /// </para>
  /// </summary>
  /// <remarks>
  /// <para>This interface supports access to the reference ID within that repository.</para>
  /// </remarks>
  /// <typeparam name="TEntity">
  /// The entity type.
  /// </typeparam>
  /// <typeparam name="TIdentity">
  /// Describes the underlying type used to create unique references to this entity - usually a primitive type.
  /// </typeparam>
  public interface IEntity<TEntity,TIdentity>
    : IEntity, IEquatable<IEntity<TEntity,TIdentity>>
    where TEntity : IEntity
  {
    #region properties
    
    /// <summary>
    /// <para>Gets and sets the unique identifier component of this entity's generic identity.</para>
    /// <seealso cref="GetIdentity"/>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Ideally, avoid using this property unless absolutely neccesary.  It exists primarily for providing access to
    /// database-mapping layers that need a property with which to reference the unique identifier.
    /// </para>
    /// </remarks>
    new TIdentity Id { get; set; }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Gets the identity of the current instance.</para>
    /// </summary>
    /// <returns>
    /// An <see cref="IIdentity"/> instance.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// If the current instance does not yet have an identity.  <see cref="HasIdentity"/>.
    /// </exception>
    new Identity<TEntity,TIdentity> GetIdentity();

    /// <summary>
    /// <para>Overloaded.  Sets the reference stored within the current object instance.</para>
    /// </summary>
    /// <param name="identityValue">
    /// An instance of the underlying reference type for the current object.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If <paramref name="identityValue"/> is not a valid reference.
    /// </exception>
    void SetIdentity(TIdentity identityValue);
    
    /// <summary>
    /// <para>
    /// Overloaded.  Determines whether or not a given <paramref name="identityValue"/> is valid for storing
    /// within the current object instance.
    /// </para>
    /// </summary>
    /// <param name="identityValue">
    /// An instance of the underlying identity type for the current object.
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    bool ValidateIdentity(TIdentity identityValue);
    
    #endregion
  }
}

