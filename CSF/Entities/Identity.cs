//
//  IdentityCreator.cs
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
  /// Static functionality supporting the <see cref="T:Identity{TIdentity}"/> type.
  /// </summary>
  public static class Identity
  {
    #region constants

    private static readonly Type OpenGenericIdentity = typeof(Identity<,>);

    #endregion

    /// <summary>
    /// Creates an identity instance generically.
    /// </summary>
    /// <param name="identityValue">Identity value.</param>
    /// <typeparam name="TIdentity">The 1st type parameter.</typeparam>
    public static IIdentity Create<TIdentity>(TIdentity identityValue, Type entityType)
    {
      if(entityType == null)
      {
        throw new ArgumentNullException("entityType");
      }

      var closedGenericType = OpenGenericIdentity.MakeGenericType(typeof(TIdentity), entityType);
      return (IIdentity) Activator.CreateInstance(closedGenericType, new Object[] { identityValue });
    }
  }
}

