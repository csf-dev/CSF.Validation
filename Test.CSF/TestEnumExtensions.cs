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

