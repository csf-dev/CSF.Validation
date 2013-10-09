using System;
using NUnit.Framework;
using CSF;

namespace Test.CSF
{
  [TestFixture]
  public class TestEnumExtensions
  {
    #region tests

    [Test]
    public void TestIsDefinedValueTrue()
    {
      Assert.IsTrue(SampleEnum.Two.IsDefinedValue());
    }

    [Test]
    public void TestIsDefinedValueFalse()
    {
      Assert.IsFalse(((SampleEnum) 8).IsDefinedValue());
    }

    [Test]
    public void TestWithFlags()
    {
      FlagsEnum testVal = FlagsEnum.One | FlagsEnum.Two;

      FlagsEnum result = testVal.WithFlags(FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four);
      Assert.AreEqual(7, (int) result);
    }

    [Test]
    public void TestWithoutFlags()
    {
      FlagsEnum testVal = FlagsEnum.One | FlagsEnum.Eight;

      FlagsEnum result = testVal.WithoutFlags(FlagsEnum.One, FlagsEnum.Two, FlagsEnum.Four);
      Assert.AreEqual(8, (int) result);
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void TestWithFlagsNotFlags()
    {
      SampleEnum testVal = SampleEnum.One | SampleEnum.Two;
      testVal.WithFlags(SampleEnum.One, SampleEnum.Two, SampleEnum.Three);
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void TestWithFlagsNotEnumeration()
    {
      DateTime testVal = DateTime.Today;
      testVal.WithFlags(DateTime.Now);
    }

    #endregion

    #region test enumeration
    
    enum SampleEnum : int
    {
      [global::CSF.Reflection.UIText("First")]
      One   = 1,
      
      [global::CSF.Reflection.UIText("Second")]
      Two   = 2,
      
      Three = 3
    }

    [Flags]
    enum FlagsEnum : int
    {
      One = 1,

      Two = 2,

      Four = 4,

      Eight = 8
    }
    
    #endregion
  }
}

