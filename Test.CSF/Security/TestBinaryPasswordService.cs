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

