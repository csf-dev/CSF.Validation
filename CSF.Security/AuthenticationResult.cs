//
// AuthenticationResult.cs
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
  /// Immutable type represents the result of an authentication attempt.
  /// </summary>
  public class AuthenticationResult
  {
    /// <summary>
    /// Gets a value indicating whether the credentials were found (usually meaning that a matching user was found in
    /// the db).
    /// </summary>
    /// <value><c>true</c> if the credentials were found; otherwise, <c>false</c>.</value>
    public bool CredentialsFound { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the credentials were verified.
    /// </summary>
    /// <value><c>true</c> if the credentials were verified; otherwise, <c>false</c>.</value>
    public bool CredentialsVerified { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Security.AuthenticationResult"/> class.
    /// </summary>
    /// <param name="found">Whether or not the credentials were found.</param>
    /// <param name="verified">Whether or not the credentials were verified.</param>
    public AuthenticationResult(bool found, bool verified)
    {
      CredentialsFound = found;
      CredentialsVerified = verified;
    }
  }
}

