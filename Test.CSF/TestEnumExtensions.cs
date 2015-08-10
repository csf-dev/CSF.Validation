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

    [Test]
    public void TestGetIndividualValues()
    {
      // Arrange
      SecondFlagsEnum
        testOne = (SecondFlagsEnum) 6,
        testTwo = (SecondFlagsEnum) 31,
        testThree = (SecondFlagsEnum) 2,
        testFour = (SecondFlagsEnum) 0;
      SecondFlagsEnum[]
        expectedOne = new SecondFlagsEnum[] { 
          SecondFlagsEnum.Two, 
          SecondFlagsEnum.Four
        },
        expectedTwo = new SecondFlagsEnum[] { 
          SecondFlagsEnum.One,
          SecondFlagsEnum.Two,
          SecondFlagsEnum.Four,
          SecondFlagsEnum.Eight,
          SecondFlagsEnum.Sixteen
        },
        expectedThree = new SecondFlagsEnum[] {
          SecondFlagsEnum.Two
        },
        expectedFour = new SecondFlagsEnum[0];

      // Act
      var resultOne = testOne.GetIndividualValues();
      var resultTwo = testTwo.GetIndividualValues();
      var resultThree = testThree.GetIndividualValues();
      var resultFour = testFour.GetIndividualValues();

      // Assert
      Assert.AreEqual(expectedOne, resultOne, "Result 1");
      Assert.AreEqual(expectedTwo, resultTwo, "Result 2");
      Assert.AreEqual(expectedThree, resultThree, "Result 3");
      Assert.AreEqual(expectedFour, resultFour, "Result 4");
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

    [Flags]
    enum SecondFlagsEnum : ulong
    {
      One     = 1 << 0,

      Two     = 1 << 1,

      Four    = 1 << 2,

      Eight   = 1 << 3,

      Sixteen = 1 << 4,
    }
    
    #endregion
  }
}

