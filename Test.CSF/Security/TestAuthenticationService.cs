//
// TestAuthenticationService.cs
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
using CSF.Security;
using NUnit.Framework;
using Moq;

namespace Test.CSF.Security
{
  [TestFixture]
  public class TestAuthenticationService
  {
    #region fields

    private Mock<ICredentialsRepository<StubEnteredCredentials,StubStoredCredentials>> _repository;
    private Mock<ICredentialVerifier<StubEnteredCredentials,StubStoredCredentials>> _verifier;

    private IAuthenticationService<StubEnteredCredentials> _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _repository = new Mock<ICredentialsRepository<StubEnteredCredentials, StubStoredCredentials>>();
      _verifier = new Mock<ICredentialVerifier<StubEnteredCredentials, StubStoredCredentials>>();

      _sut = new AuthenticationService<StubEnteredCredentials,StubStoredCredentials>(_repository.Object,
                                                                                     _verifier.Object);
    }

    #endregion

    #region tests

    [Test]
    public void Authenticate_returns_not_found_result_when_credentials_not_found()
    {
      // Arrange
      var entered = Mock.Of<StubEnteredCredentials>();

      _repository.Setup(x => x.GetStoredCredentials(entered)).Returns((StubStoredCredentials) null);

      // Act
      var result = _sut.Authenticate(entered);

      // Assert
      Assert.IsFalse(result.CredentialsFound);
    }

    [Test]
    public void Authenticate_returns_failure_result_when_credentials_not_found()
    {
      // Arrange
      var entered = Mock.Of<StubEnteredCredentials>();

      _repository.Setup(x => x.GetStoredCredentials(entered)).Returns((StubStoredCredentials) null);

      // Act
      var result = _sut.Authenticate(entered);

      // Assert
      Assert.IsFalse(result.CredentialsVerified);
    }

    [Test]
    public void Authenticate_returns_found_result_when_credentials_are_found()
    {
      // Arrange
      var entered = Mock.Of<StubEnteredCredentials>();
      var stored = Mock.Of<StubStoredCredentials>();

      _repository.Setup(x => x.GetStoredCredentials(entered)).Returns(stored);

      // Act
      var result = _sut.Authenticate(entered);

      // Assert
      Assert.IsTrue(result.CredentialsFound);
    }

    [Test]
    public void Authenticate_returns_failure_result_when_authentication_fails()
    {
      // Arrange
      var entered = Mock.Of<StubEnteredCredentials>();
      var stored = Mock.Of<StubStoredCredentials>();

      _repository.Setup(x => x.GetStoredCredentials(entered)).Returns(stored);
      _verifier.Setup(x => x.Verify(entered, stored)).Returns(false);

      // Act
      var result = _sut.Authenticate(entered);

      // Assert
      Assert.IsFalse(result.CredentialsVerified);
    }

    [Test]
    public void Authenticate_returns_success_result_when_authentication_passes()
    {
      // Arrange
      var entered = Mock.Of<StubEnteredCredentials>();
      var stored = Mock.Of<StubStoredCredentials>();

      _repository.Setup(x => x.GetStoredCredentials(entered)).Returns(stored);
      _verifier.Setup(x => x.Verify(entered, stored)).Returns(true);

      // Act
      var result = _sut.Authenticate(entered);

      // Assert
      Assert.IsTrue(result.CredentialsVerified);
    }

    #endregion

    #region contained types

    public class StubStoredCredentials : Base64KeyAndSalt {}

    public class StubEnteredCredentials : ICredentialsWithPassword
    {
      public virtual byte[] GetPasswordAsByteArray()
      {
        throw new NotImplementedException();
      }
    }

    #endregion
  }
}

