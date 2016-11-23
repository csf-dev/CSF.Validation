//
// ICredentialVerifier.cs
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
  /// Credentials verifier interface.  Verifies that a set of entered credentials (such as those provided by a user)
  /// matches a set of stored credentials (such as those retrieved from a database).
  /// </summary>
  public interface ICredentialVerifier
  {
    /// <summary>
    /// Verifies that the entered credentials match the stored credentials.
    /// </summary>
    /// <param name="enteredCredentials">Entered credentials.</param>
    /// <param name="storedCredentials">Stored credentials.</param>
    bool Verify(object enteredCredentials, object storedCredentials);
  }

  /// <summary>
  /// Credentials verifier interface.  Verifies that a set of entered credentials (such as those provided by a user)
  /// matches a set of stored credentials (such as those retrieved from a database).
  /// </summary>
  public interface ICredentialVerifier<TEnteredCredentials,TStoredCredentials> : ICredentialVerifier
  {
    /// <summary>
    /// Verifies that the entered credentials match the stored credentials.
    /// </summary>
    /// <param name="enteredCredentials">Entered credentials.</param>
    /// <param name="storedCredentials">Stored credentials.</param>
    bool Verify(TEnteredCredentials enteredCredentials, TStoredCredentials storedCredentials);
  }
}

