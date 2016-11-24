//
// IBinaryKeyCreator.cs
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
  /// Service which creates binary (byte array) random salts and hashed keys (given a salt).
  /// </summary>
  public interface IBinaryKeyCreator
  {
    /// <summary>
    /// Creates a random salt, as a byte array.
    /// </summary>
    /// <returns>The random salt.</returns>
    /// <param name="length">Desired salt length in bytes.</param>
    byte[] CreateRandomSalt(int length);

    /// <summary>
    /// Creates a key for a given password and salt, of a desired key length.
    /// </summary>
    /// <returns>The generated key.</returns>
    /// <param name="password">The password.</param>
    /// <param name="salt">The salt.</param>
    /// <param name="length">The key length.</param>
    byte[] CreateKey(byte[] password, byte[] salt, int length);
  }
}

