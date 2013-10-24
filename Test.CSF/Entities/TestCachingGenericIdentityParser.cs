using System;
using NUnit.Framework;
using CSF.Entities;

namespace Test.CSF.Entities
{
  [TestFixture]
  public class TestCachingGenericIdentityParser
  {
    #region tests

    [Test]
    public void TestParse()
    {
      IGenericIdentityParser parser = new CachingGenericIdentityParser();
      Assert.AreEqual(new Identity<DummyEntityOne,int>(3), parser.Parse<DummyEntityOne>("3"), "Parser works");
      Assert.AreEqual(new Identity<DummyEntityOne,int>(5), parser.Parse<DummyEntityOne>("5"), "Parser works second time");
    }

    #endregion

    #region contained types

    public class DummyEntityOne : Entity<DummyEntityOne,int> {}

    #endregion
  }
}

