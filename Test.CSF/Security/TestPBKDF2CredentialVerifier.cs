//
// TestPBKDF2CredentialVerifier.cs
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
  public class TestPBKDF2CredentialVerifier
  {
    #region constants

    private readonly byte[]
      PASSWORD_ONE                        = new byte[] { 10, 20, 30, 40, 50 },
      PASSWORD_TWO                        = new byte[] { 20, 10, 30, 40, 50 },
      SALT_ONE                            = new byte[] { 11, 21, 31, 41, 51, 61, 71, 81 },
      SALT_TWO                            = new byte[] { 12, 22, 32, 42, 52, 62, 72, 82 },
      PASSWORD_ONE_SALT_ONE_ITER_100_KEY  = new byte[] { 58, 57, 26, 90, 209, 115, 230, 94, 207, 30 };

    #endregion

    #region tests

    [Test]
    public void CreateKey_creates_same_key_with_same_password_and_salt_across_two_instances()
    {
      // Act
      var resultOne = CreateKeyInNewInstance(SALT_ONE, PASSWORD_ONE, 10, 100);
      var resultTwo = CreateKeyInNewInstance(SALT_ONE, PASSWORD_ONE, 10, 100);

      // Assert
      CollectionAssert.AreEqual(resultOne, resultTwo);
    }

    [Test]
    public void CreateKey_creates_different_key_with_different_password_and_same_salt()
    {
      // Act
      var resultOne = CreateKeyInNewInstance(SALT_ONE, PASSWORD_ONE, 10, 100);
      var resultTwo = CreateKeyInNewInstance(SALT_ONE, PASSWORD_TWO, 10, 100);

      // Assert
      Assert.IsFalse(Object.Equals(resultOne, resultTwo), "Results are not equal");
    }

    [Test]
    public void CreateKey_creates_different_key_with_same_password_and_different_salt()
    {
      // Act
      var resultOne = CreateKeyInNewInstance(SALT_ONE, PASSWORD_ONE, 10, 100);
      var resultTwo = CreateKeyInNewInstance(SALT_TWO, PASSWORD_ONE, 10, 100);

      // Assert
      Assert.IsFalse(Object.Equals(resultOne, resultTwo), "Results are not equal");
    }

    [Test]
    public void CreateKey_creates_same_key_with_same_password_and_salt_but_different_iteration_counts()
    {
      // Act
      var resultOne = CreateKeyInNewInstance(SALT_ONE, PASSWORD_ONE, 10, 100);
      var resultTwo = CreateKeyInNewInstance(SALT_ONE, PASSWORD_ONE, 10, 105);

      // Assert
      Assert.IsFalse(Object.Equals(resultOne, resultTwo), "Results are not equal");
    }

    [TestCase(10)]
    [TestCase(20)]
    public void CreateKey_creates_key_of_correct_byte_length(int keyLength)
    {
      // Act
      var result = CreateKeyInNewInstance(SALT_ONE, PASSWORD_ONE, keyLength, 100);

      // Assert
      Assert.AreEqual(keyLength, result.Length);
    }

    [TestCase(10)]
    [TestCase(20)]
    public void CreateRandomSalt_creates_salt_of_correct_byte_length(int saltLength)
    {
      // Arrange
      var sut = new StubVerifier(100);

      // Act
      var result = sut.CreateRandomSalt(saltLength);

      // Assert
      Assert.AreEqual(saltLength, result.Length);
    }

    [Test]
    public void Verify_returns_true_for_correct_password_salt_and_iteration_count()
    {
      // Arrange
      var stored = new Mock<StubStoredCredentials>();
      stored.Setup(x => x.GetKeyAsByteArray()).Returns(PASSWORD_ONE_SALT_ONE_ITER_100_KEY);
      stored.Setup(x => x.GetSaltAsByteArray()).Returns(SALT_ONE);

      var entered = new Mock<StubEnteredCredentials>();
      entered.Setup(x => x.GetPasswordAsByteArray()).Returns(PASSWORD_ONE);

      var sut = new StubVerifier(100);

      // Act
      var result = sut.Verify(entered.Object, stored.Object);

      // Assert
      Assert.IsTrue(result);
    }

    [Test]
    public void Verify_returns_false_for_correct_password_and_iteration_count_but_wrong_salt()
    {
      // Arrange
      var stored = new Mock<StubStoredCredentials>();
      stored.Setup(x => x.GetKeyAsByteArray()).Returns(PASSWORD_ONE_SALT_ONE_ITER_100_KEY);
      stored.Setup(x => x.GetSaltAsByteArray()).Returns(SALT_TWO);

      var entered = new Mock<StubEnteredCredentials>();
      entered.Setup(x => x.GetPasswordAsByteArray()).Returns(PASSWORD_ONE);

      var sut = new StubVerifier(100);

      // Act
      var result = sut.Verify(entered.Object, stored.Object);

      // Assert
      Assert.IsFalse(result);
    }

    [Test]
    public void Verify_returns_false_for_correct_salt_and_iteration_count_but_wrong_password()
    {
      // Arrange
      var stored = new Mock<StubStoredCredentials>();
      stored.Setup(x => x.GetKeyAsByteArray()).Returns(PASSWORD_ONE_SALT_ONE_ITER_100_KEY);
      stored.Setup(x => x.GetSaltAsByteArray()).Returns(SALT_ONE);

      var entered = new Mock<StubEnteredCredentials>();
      entered.Setup(x => x.GetPasswordAsByteArray()).Returns(PASSWORD_TWO);

      var sut = new StubVerifier(100);

      // Act
      var result = sut.Verify(entered.Object, stored.Object);

      // Assert
      Assert.IsFalse(result);
    }

    [Test]
    public void Verify_returns_false_for_correct_password_and_salt_but_wrong_iteration_count()
    {
      // Arrange
      var stored = new Mock<StubStoredCredentials>();
      stored.Setup(x => x.GetKeyAsByteArray()).Returns(PASSWORD_ONE_SALT_ONE_ITER_100_KEY);
      stored.Setup(x => x.GetSaltAsByteArray()).Returns(SALT_ONE);

      var entered = new Mock<StubEnteredCredentials>();
      entered.Setup(x => x.GetPasswordAsByteArray()).Returns(PASSWORD_ONE);

      var sut = new StubVerifier(105);

      // Act
      var result = sut.Verify(entered.Object, stored.Object);

      // Assert
      Assert.IsFalse(result);
    }

    #endregion

    #region methods

    private byte[] CreateKeyInNewInstance(byte[] salt, byte[] password, int length, int iterationCount)
    {
      var sut = new StubVerifier(iterationCount);
      return sut.CreateKey(password, salt, length);
    }

    private bool VerifyInNewInstance(StubEnteredCredentials entered, StubStoredCredentials stored, int iterationCount)
    {
      var sut = new StubVerifier(iterationCount);
      return sut.Verify(entered, stored);
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

    public class StubVerifier : PBKDF2CredentialVerifier<StubEnteredCredentials,StubStoredCredentials>
    {
      public StubVerifier(int iterationCount) : base(iterationCount) {}
    }

    #endregion
  }
}

