//
// IIdentity.cs
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
  /// Base non-generic interface for an entity identity.
  /// </summary>
  public interface IIdentity : IEquatable<IIdentity>
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

