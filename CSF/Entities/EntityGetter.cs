//  
//  EntityReader.cs
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
  /// A non-generic delegate that represents a method that is capable of reading a single <see cref="IEntity"/> using
  /// its unique <paramref name="identity"/>.
  /// </para>
  /// </summary>
  [Obsolete("This delegate is deemed to be obsolete and will be removed in v3.x.")]
  public delegate IEntity EntityReader(IIdentity identity);

  /// <summary>
  /// <para>
  /// A generic delegate that represents a method that is capable of reading a single generic <see cref="IEntity"/>
  /// using its unique <paramref name="identity"/>.
  /// </para>
  /// </summary>
  [Obsolete("This delegate is deemed to be obsolete and will be removed in v3.x.")]
  public delegate TEntity EntityReader<TEntity,TIdentity>(IIdentity<TEntity,TIdentity> identity)
                                                          where TEntity : IEntity<TEntity,TIdentity>;

#pragma warning disable 1587
#pragma warning disable 1591

  /// <summary>
  /// Generic delegate represents a method that gets an entity instance using its identity.
  /// </summary>
  public delegate TEntity EntityGetter<TEntity,TIdentity>(Identity<TEntity,TIdentity> identity) where TEntity : IEntity;

  /// <summary>
  /// Generic delegate represents a method that gets an entity instance using its identity.
  /// </summary>
  public delegate TEntity EntityGetter<TEntity>(IIdentity<TEntity> identity) where TEntity : IEntity;

#pragma warning restore 1591
#pragma warning restore 1587
}

