//
// PBKDF2CredentialVerifier.cs
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
using System.Security.Cryptography;
using System.Linq;

namespace CSF.Security
{
  /// <summary>
  /// Abstract base type for an <see cref="ICredentialVerifier"/> which uses the PBKDF2 mechanism.
  /// </summary>
  public abstract class PBKDF2CredentialVerifier<TEnteredCredentials,TStoredCredentials>
    : ICredentialVerifier<TEnteredCredentials,TStoredCredentials>, ICredentialVerifier, IBinaryKeyCreator
    where TEnteredCredentials : ICredentialsWithPassword
    where TStoredCredentials : IStoredCredentialsWithKeyAndSalt
  {
    #region constants

    private static RNGCryptoServiceProvider _randomNumberGenerator;

    #endregion

    #region fields

    private int _iterationCount;

    #endregion

    #region properties

    /// <summary>
    /// Gets the iteration count for the PBKDF2 operations.
    /// </summary>
    /// <value>The iteration count.</value>
    protected int IterationCount
    {
      get {
        return _iterationCount;
      }
    }

    /// <summary>
    /// Gets the secure random number generator.
    /// </summary>
    /// <value>The random number generator.</value>
    protected RNGCryptoServiceProvider RandomNumberGenerator
    {
      get {
        return _randomNumberGenerator;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Verifies that the entered credentials match the stored credentials.
    /// </summary>
    /// <param name="enteredCredentials">Entered credentials.</param>
    /// <param name="storedCredentials">Stored credentials.</param>
    public virtual bool Verify(TEnteredCredentials enteredCredentials, TStoredCredentials storedCredentials)
    {
      if(enteredCredentials == null)
      {
        throw new ArgumentNullException(nameof(enteredCredentials));
      }
      if(storedCredentials == null)
      {
        throw new ArgumentNullException(nameof(storedCredentials));
      }

      var storedSalt = storedCredentials.GetSaltAsByteArray();
      var storedKey = storedCredentials.GetKeyAsByteArray();
      var enteredPassword = enteredCredentials.GetPasswordAsByteArray();

      var generatedKey = CreateKey(enteredPassword, storedSalt, storedKey.Length);
      return Enumerable.SequenceEqual(generatedKey, storedKey);
    }

    /// <summary>
    /// Creates a random salt, as a byte array.
    /// </summary>
    /// <returns>The random salt.</returns>
    /// <param name="length">Desired salt length in bytes.</param>
    public virtual byte[] CreateRandomSalt(int length)
    {
      if(length < 1)
      {
        throw new ArgumentOutOfRangeException(nameof(length), "Salt length must be more than zero.");
      }

      var output = new byte[length];

      RandomNumberGenerator.GetBytes(output);

      return output;
    }

    /// <summary>
    /// Creates a key for a given password and salt, of a desired key length.
    /// </summary>
    /// <returns>The generated key.</returns>
    /// <param name="password">The password.</param>
    /// <param name="salt">The salt.</param>
    /// <param name="length">The key length.</param>
    public virtual byte[] CreateKey(byte[] password, byte[] salt, int length)
    {
      if(password == null)
      {
        throw new ArgumentNullException(nameof(password));
      }
      if(salt == null)
      {
        throw new ArgumentNullException(nameof(salt));
      }
      if(length < 1)
      {
        throw new ArgumentOutOfRangeException(nameof(length), "Key length must be more than zero.");
      }

      var utility = GetHashingUtility(password, salt);
      return utility.GetBytes(length);
    }

    /// <summary>
    /// Gets the instance of <c>Rfc2898DeriveBytes</c>.
    /// </summary>
    /// <returns>The hashing utility.</returns>
    /// <param name="password">Password.</param>
    /// <param name="salt">Salt.</param>
    protected virtual Rfc2898DeriveBytes GetHashingUtility(byte[] password, byte[] salt)
    {
      return new Rfc2898DeriveBytes(password, salt, IterationCount);
    }

    #endregion

    #region interface implementations

    bool ICredentialVerifier.Verify(object enteredCredentials, object storedCredentials)
    {
      return Verify((TEnteredCredentials) enteredCredentials, (TStoredCredentials) storedCredentials);
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Security.PBKDF2CredentialVerifier`2"/> class.
    /// </summary>
    /// <param name="iterationCount">Iteration count.</param>
    public PBKDF2CredentialVerifier(int iterationCount = 1000)
    {
      if(iterationCount < 1)
      {
        throw new ArgumentOutOfRangeException(nameof(iterationCount), "Iteration count must be more than zero.");
      }

      _iterationCount = iterationCount;
    }

    /// <summary>
    /// Initializes the <see cref="T:CSF.Security.PBKDF2CredentialVerifier`2"/> class.
    /// </summary>
    static PBKDF2CredentialVerifier()
    {
      _randomNumberGenerator = new RNGCryptoServiceProvider();
    }

    #endregion
  }
}

