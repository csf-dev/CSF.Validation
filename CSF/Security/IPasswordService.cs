//
// IPasswordService.cs
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

namespace CSF.Security
{
  /// <summary>
  /// Interface for a service that provides password hashing and checking.
  /// </summary>
  public interface IPasswordService
  {
    #region properties
    
    /// <summary>
    /// Gets the name of the hashing algorithm that will be used by this instance.
    /// </summary>
    /// <value>
    /// The name of the algorithm.
    /// </value>
    string AlgorithmName { get; }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Generates a new password hash and salt pair from a password, using the specified length of password salt.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The meaning of <paramref name="saltLength"/> is implementation-dependent.  It could mean (for example) the
    /// number of printable characters that comprise the salt, or it could mean the number of bytes of data.
    /// </para>
    /// <para>
    /// The password salt is randomly-generated.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A hash-and-salt pair corresponding to the <paramref name="password"/>.
    /// </returns>
    /// <param name='password'>
    /// The password to hash.
    /// </param>
    /// <param name='saltLength'>
    /// The length of salt to use, please read the remarks for more info.
    /// </param>
    IHashAndSaltPair GenerateHash(string password, int saltLength);
    
    /// <summary>
    /// Determines whether the specified password matches the specified hash and salt.
    /// </summary>
    /// <returns>
    /// Whether or not the password matches the hash/salt pair or not.
    /// </returns>
    /// <param name='password'>
    /// The password to test.
    /// </param>
    /// <param name='hashAndSalt'>
    /// The hash/salt pair to test against.
    /// </param>
    bool PasswordMatches(string password, IHashAndSaltPair hashAndSalt);
    
    /// <summary>
    /// Generates a randomised string within a specified dictionary.
    /// </summary>
    /// <returns>
    /// The random string.
    /// </returns>
    /// <param name='dictionary'>
    /// A string that indicates a 'dictionary' of characters that may be used.
    /// </param>
    /// <param name='length'>
    /// The length of randomised string to create.
    /// </param>
    string GenerateRandomString(string dictionary, int length);
    
    /// <summary>
    /// Generates a randomised string within a specified dictionary.
    /// </summary>
    /// <returns>
    /// The random string.
    /// </returns>
    /// <param name='dictionary'>
    /// A character-array that indicates a 'dictionary' of characters that may be used.
    /// </param>
    /// <param name='length'>
    /// The length of randomised string to create.
    /// </param>
    string GenerateRandomString(char[] dictionary, int length);
    
    #endregion
  }
}

