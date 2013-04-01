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
    public void TestHasFlagTrue()
    {
      FlagsEnum val = (FlagsEnum.One | FlagsEnum.Eight);
      Assert.IsTrue(val.HasFlag(FlagsEnum.Eight));
    }

    [Test]
    public void TestHasFlagFalse()
    {
      FlagsEnum val = (FlagsEnum.One | FlagsEnum.Eight);
      Assert.IsFalse(val.HasFlag(FlagsEnum.Four));
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void TestHasFlagNotFlags()
    {
      SampleEnum val = SampleEnum.One;
      val.HasFlag(SampleEnum.One);
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

