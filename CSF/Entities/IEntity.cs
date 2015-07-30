//  
//  IEntity.cs
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
  /// <para>Base non-generic interface for domain entities.</para>
  /// </summary>
  public interface IEntity
  {
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets whether or not the current instance has an identity or not.</para>
    /// </summary>
    bool HasIdentity { get; }

    #endregion

    #region methods

    /// <summary>
    /// Gets the raw identity instance contained within the current entity, favour the extension method 'GetIdentity'
    /// instead.
    /// </summary>
    /// <returns>The identity value.</returns>
    IIdentity GetRawIdentity();

    #endregion
  }
}

