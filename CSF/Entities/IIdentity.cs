//
//  IIdentity.cs
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
  /// Base non-generic interface for an entity identity.
  /// </summary>
  public interface IIdentity
  {
    /// <summary>
    /// Gets a <see cref="System.Type"/> that indicates the type of entity that this instance describes.
    /// </summary>
    /// <value>The entity type.</value>
    Type EntityType { get; }

    /// <summary>
    /// Gets the underlying type of <see cref="Value"/>.
    /// </summary>
    /// <value>The identity type.</value>
    Type IdentityType { get; }

    /// <summary>
    /// Gets the identity value contained within the current instance.
    /// </summary>
    /// <value>The identity value.</value>
    object Value { get; }
  }
}

