//
//  NonExistantEntityException.cs
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

namespace CSF.Entities
{
  /// <summary>
  /// Exception raised when an <c>IIdentity</c> is used to get an entity directly, but the desired entity does not
  /// exist.
  /// </summary>
  public class NonExistantEntityException<TEntity> : Exception where TEntity : IEntity
  {
    #region constants

    private const string
      DEFAULT_MESSAGE         = "The entity referenced by the specified identity must exist.",
      IDENTITY_KEY            = "Identity";

    #endregion

    #region properties

    /// <summary>
    /// Gets the identity that failed to load an entity.
    /// </summary>
    /// <value>
    /// The identity.
    /// </value>
    public IIdentity<TEntity> Identity
    {
      get {
        return (IIdentity<TEntity>) this.Data[IDENTITY_KEY];
      }
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <c>CSF.Entities.NonExistantEntityException</c> class.
    /// </summary>
    /// <param name='identity'>
    /// Identity.
    /// </param>
    public NonExistantEntityException (IIdentity<TEntity> identity) : this(DEFAULT_MESSAGE, null, identity) {}

    /// <summary>
    /// Initializes a new instance of the <c>CSF.Entities.NonExistantEntityException</c> class.
    /// </summary>
    /// <param name='message'>
    /// Message.
    /// </param>
    /// <param name='identity'>
    /// Identity.
    /// </param>
    public NonExistantEntityException (string message, IIdentity<TEntity> identity) : this(message, null, identity) {}

    /// <summary>
    /// Initializes a new instance of the <c>CSF.Entities.NonExistantEntityException</c> class.
    /// </summary>
    /// <param name='message'>
    /// Message.
    /// </param>
    /// <param name='inner'>
    /// Inner.
    /// </param>
    /// <param name='identity'>
    /// Identity.
    /// </param>
    public NonExistantEntityException (string message,
                                       Exception inner,
                                       IIdentity<TEntity> identity) : base(message, inner)
    {
      this.Data[IDENTITY_KEY] = identity;
    }

    /// <summary>
    /// Initializes a new instance of the <c>CSF.Entities.NonExistantEntityException</c> class.
    /// </summary>
    /// <param name='message'>
    /// Message.
    /// </param>
    /// <param name='inner'>
    /// Inner.
    /// </param>
    public NonExistantEntityException (string message, Exception inner) : this(message, inner, null) {}

    #endregion
  }
}

