//
// NonExistantEntityException.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2015 CSF Software Limited
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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

