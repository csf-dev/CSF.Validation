//
// Base64PasswordAndSalt.cs
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
  /// Base type for stored credentials which includes a key and salt stored as Base64-encoded strings.
  /// </summary>
  public class Base64KeyAndSalt : IStoredCredentialsWithKeyAndSalt
  {
    #region properties

    /// <summary>
    /// Gets or sets the Base64-encoded representation of the stored key hash.
    /// </summary>
    /// <value>The key data.</value>
    public virtual string Key { get; set; }

    /// <summary>
    /// Gets or sets the Base64-encoded representation of the stored salt.
    /// </summary>
    /// <value>The salt data.</value>
    public virtual string Salt { get; set; }

    #endregion

    #region methods

    /// <summary>
    /// Gets the key as a byte array.
    /// </summary>
    /// <returns>The key as a byte array.</returns>
    public virtual byte[] GetKeyAsByteArray()
    {
      if(Key == null)
      {
        return null;
      }

      return Convert.FromBase64String(Key);
    }

    /// <summary>
    /// Gets the salt as a byte array.
    /// </summary>
    /// <returns>The salt as a byte array.</returns>
    public virtual byte[] GetSaltAsByteArray()
    {
      if(Salt == null)
      {
        return null;
      }

      return Convert.FromBase64String(Salt);
    }

    #endregion
  }
}

