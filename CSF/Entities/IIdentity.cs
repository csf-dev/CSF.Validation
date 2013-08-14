//  
//  IIdentity.cs
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
  /// Interface to describe the identity that every <see cref="IEntity"/> exposes in its
  /// <see cref="IEntity.GetIdentity"/> method.
  /// </para>
  /// </summary>
  [Obsolete("This interface is deemed to be obsolete and will be removed in v3.x.")]
  public interface IIdentity
  {
    /// <summary>
    /// <para>
    /// Read-only.  A <see cref="System.Type"/> that indicates the type of <see cref="IEntity"/> that this identifier
    /// represents.
    /// </para>
    /// </summary>
    Type EntityType { get; }

    /// <summary>
    /// <para>Read-only.  Gets the type of the identifier <see cref="Value"/>.</para>
    /// </summary>
    Type IdentifierType { get; }
    
    /// <summary>
    /// <para>
    /// Read-only.  An <see cref="System.Object"/> that uniquely identifies this entity amongst all other entities of
    /// the same <see cref="Type"/>.
    /// </para>
    /// </summary>
    object Value { get; }
  }
}

