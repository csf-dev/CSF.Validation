//
// BinaryPasswordAndSalt.cs
//
// Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
// Copyright (c) 2016 Craig Fowler
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

namespace CSF.Security
{
  /// <summary>
  /// Base type for stored credentials which includes a key and salt stored as binary data.
  /// </summary>
  public class BinaryKeyAndSalt : IStoredCredentialsWithKeyAndSalt
  {
    #region properties

    /// <summary>
    /// Gets or sets the key bytes.
    /// </summary>
    /// <value>The key bytes.</value>
    public virtual byte[] Key { get; set; }

    /// <summary>
    /// Gets or sets the salt bytes.
    /// </summary>
    /// <value>The salt bytes.</value>
    public virtual byte[] Salt { get; set; }

    #endregion

    #region methods

    /// <summary>
    /// Gets the key as a byte array.
    /// </summary>
    /// <returns>The key as a byte array.</returns>
    public virtual byte[] GetKeyAsByteArray()
    {
      return Key;
    }

    /// <summary>
    /// Gets the salt as a byte array.
    /// </summary>
    /// <returns>The salt as a byte array.</returns>
    public virtual byte[] GetSaltAsByteArray()
    {
      return Salt;
    }

    #endregion
  }
}

