//
//  EntityExtensions.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2015 Craig Fowler
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
  /// Extension methods for entity types.
  /// </summary>
  public static class EntityExtensions
  {
    #region extension methods

    /// <summary>
    /// Gets an <see cref="T:Identity{TIdentity,TEntity}"/> instance for the given entity instance.
    /// </summary>
    /// <returns>The identity, or a <c>null</c> reference if the entity does not have an identity.</returns>
    /// <param name="entity">An entity instance.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public static IIdentity<TEntity> GetIdentity<TEntity>(this TEntity entity)
      where TEntity : IEntity
    {
      if(entity == null)
      {
        throw new ArgumentNullException("entity");
      }

      return entity.HasIdentity? (IIdentity<TEntity>) entity.GetRawIdentity() : null;
    }

    #endregion
  }
}

