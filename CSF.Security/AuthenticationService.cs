//
// AuthenticationService.cs
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
  /// Abstract base type for an authentication service.
  /// </summary>
  public class AuthenticationService<TEnteredCredentials,TStoredCredentials>
    : IAuthenticationService<TEnteredCredentials>, IAuthenticationService
  {
    #region fields

    protected ICredentialsRepository<TEnteredCredentials,TStoredCredentials> CredentialsRepository { get; private set; }
    protected ICredentialVerifier<TEnteredCredentials,TStoredCredentials> CredentialsVerifier { get; private set; }

    #endregion

    #region methods

    /// <summary>
    /// Attempts authentication using the given credentials.
    /// </summary>
    /// <param name="enteredCredentials">Entered credentials.</param>
    public virtual AuthenticationResult Authenticate(TEnteredCredentials enteredCredentials)
    {
      if(enteredCredentials == null)
      {
        throw new ArgumentNullException(nameof(enteredCredentials));
      }

      var storedCredentials = CredentialsRepository.GetStoredCredentials(enteredCredentials);
      if(storedCredentials == null)
      {
        return new AuthenticationResult(false, false);
      }

      var verified = CredentialsVerifier.Verify(enteredCredentials, storedCredentials);

      return new AuthenticationResult(true, verified);
    }

    #endregion

    #region interface implementations

    AuthenticationResult IAuthenticationService.Authenticate(object enteredCredentials)
    {
      return Authenticate((TEnteredCredentials) enteredCredentials);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Security.AuthenticationService`2"/> class.
    /// </summary>
    /// <param name="repository">Credentials repository.</param>
    /// <param name="verifier">Credentials verifier.</param>
    public AuthenticationService(ICredentialsRepository<TEnteredCredentials,TStoredCredentials> repository,
                                 ICredentialVerifier<TEnteredCredentials,TStoredCredentials> verifier)
    {
      if(repository == null)
      {
        throw new ArgumentNullException(nameof(repository));
      }
      if(verifier == null)
      {
        throw new ArgumentNullException(nameof(verifier));
      }

      CredentialsRepository = repository;
      CredentialsVerifier = verifier;
    }

    #endregion
  }
}

