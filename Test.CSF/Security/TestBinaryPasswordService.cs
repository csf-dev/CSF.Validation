//
// TestBinaryPasswordService.cs
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
using NUnit.Framework;
using CSF.Security;

namespace Test.CSF.Security
{
  [TestFixture]
  public class TestBinaryPasswordService
  {
    [Test]
    public void TestGenerateHash()
    {
      IPasswordService service = new BinaryPasswordService("SHA256");
      
      IHashAndSaltPair hashAndSalt = service.GenerateHash("foo bar baz", 6);
      
      Assert.IsInstanceOf<BinaryHashAndSaltPair>(hashAndSalt, "Output correct type");
      BinaryHashAndSaltPair typedHashAndSalt = hashAndSalt as BinaryHashAndSaltPair;
      Assert.AreEqual(6, typedHashAndSalt.Salt.Length, "Correct salt length in bytes");
      Assert.AreEqual(32, typedHashAndSalt.Hash.Length, "Correct hash length in bytes");
    }
    
    [Test]
    public void TestPasswordMatches()
    {
      IPasswordService service = new BinaryPasswordService("SHA256");
      
      IHashAndSaltPair hashAndSalt = new BinaryHashAndSaltPair("aS11CyjyKim0PgOeRp/aDKPe0+rPUzw7UttJ9rFJf14=",
                                                               "6OAcgh2S");
      Assert.IsTrue(service.PasswordMatches("foo bar baz", hashAndSalt));
    }
    
    [Test]
    public void TestGenerateRandomString()
    {
      IPasswordService service = new BinaryPasswordService("SHA256");
      string randomString = service.GenerateRandomString("abc123", 20);
      
      Assert.AreEqual(20, randomString.Length, "Correct length");
      Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(randomString, "^[abc123]{20}$"),
                    "Matches desired pattern");
    }
  }
}

