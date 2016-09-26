//
// TestEnumExtensions.cs
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
using CSF;
using System.Reflection;

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
    [ExpectedException(typeof(NotSupportedException))]
    public void TestWithFlagsNotFlags()
    {
      SampleEnum testVal = SampleEnum.One | SampleEnum.Two;
      testVal.WithFlags(SampleEnum.One, SampleEnum.Two, SampleEnum.Three);
    }

    [Test]
    [ExpectedException(typeof(NotSupportedException))]
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

    [Test]
    public void TestGetFieldInfo()
    {
      FieldInfo field = SampleEnum.Three.GetFieldInfo();
      Assert.IsNotNull(field, "Field info not null");
      Assert.AreEqual(SampleEnum.Three.ToString(), field.Name, "Field hsa correct name");
    }

    #endregion

    #region test enumeration
    
    enum SampleEnum : int
    {
      One   = 1,
      
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

