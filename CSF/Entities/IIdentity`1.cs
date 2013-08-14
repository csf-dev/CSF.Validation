//  
//  IIdentity`T.cs
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
  /// Interface describes an identity instance that uniquely identifies an <see cref="IEntity"/> instance, by its type
  /// and an identifier value.
  /// </summary>
  public interface IIdentity<TEntity> : IEquatable<IIdentity<TEntity>> where TEntity : IEntity
  {
    /// <summary>
    /// Gets a <see cref="System.Type"/> that indicates the type of entity that this instance represents.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This will always be the same as the type <c>TEntity</c> that this type is constructed from.
    /// </para>
    /// </remarks>
    /// <value>
    /// The type of the entity.
    /// </value>
    Type EntityType { get; }

    /// <summary>
    /// Gets a <see cref="System.Type"/> that indicates the type of the identity value that this instance contains.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This essentially exposes the underlying type of the <see cref="Value"/> property.
    /// </para>
    /// </remarks>
    /// <value>
    /// The type of the contained identity value.
    /// </value>
    Type IdentifierType { get; }

    /// <summary>
    /// Gets the identity value contained within the current instance.
    /// </summary>
    /// <value>
    /// The identity value.
    /// </value>
    object Value { get; }
  }
}

