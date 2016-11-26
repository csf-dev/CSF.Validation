//
// TestFraction.cs
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

namespace Test.CSF
{
  [TestFixture]
  public class TestFraction
  {
    #region tests
    
    [Test]
    public void TestSimplify()
    {
      Fraction fraction = new Fraction(6, 10);
      Assert.AreEqual("3/5", fraction.Simplify().ToString(), "Simplified");
    }
    
    [Test]
    public void TestEquals()
    {
      Fraction
        fractionOne = new Fraction(6, 10),
        fractionTwo = new Fraction(3, 5);
      Assert.AreEqual(fractionOne, fractionTwo, "Equal after simplification");
    }
    
    [Test]
    public void TestToDecimal()
    {
      Fraction fraction = new Fraction(8, 50);
      Assert.AreEqual(0.16m, fraction.ToDecimal(), "Correct decimal value");
    }
    
    [Test]
    public void TestParse()
    {
      Fraction parsed;
      
      parsed = Fraction.Parse("2/4");
      Assert.AreEqual(new Fraction(2, 4), parsed, "Parsed 2/4");
      
      parsed = Fraction.Parse("-7/6");
      Assert.AreEqual(new Fraction(-7, 6), parsed, "Parsed -7/6");
      
      parsed = Fraction.Parse("-37/-90");
      Assert.AreEqual(new Fraction(-37, -90), parsed, "Parsed -37/-90");
    }
    
    [Test]
    public void TestAdditionOperator()
    {
      Assert.AreEqual(Fraction.Parse("1/1"), Fraction.Parse("2/3") + Fraction.Parse("1/3"), "Correct addition");
    }
    
    [Test]
    public void TestSubtractionOperator()
    {
      Assert.AreEqual(Fraction.Parse("0/1"), Fraction.Parse("2/3") - Fraction.Parse("2/3"), "Correct addition");
    }
    
    [Test]
    public void TestMultiplicationOperator()
    {
      Assert.AreEqual(Fraction.Parse("1/3"), Fraction.Parse("1/2") * Fraction.Parse("2/3"), "Correct addition");
    }
    
    [Test]
    public void TestDivisionOperator()
    {
      Assert.AreEqual(Fraction.Parse("4/6"), Fraction.Parse("1/3") / Fraction.Parse("2/4"), "Correct addition");
    }
    
    [Test]
    public void TestSimplifiesToInteger()
    {
      Assert.IsTrue(Fraction.Parse("3/3").SimplifiesToInteger, "3/3 is an integer amount");
      Assert.IsTrue(Fraction.Parse("-3/1").SimplifiesToInteger, "-3/1 is an integer amount");
      Assert.IsTrue(Fraction.Parse("6/3").SimplifiesToInteger, "6/3 is an integer amount");
      Assert.IsFalse(Fraction.Parse("7/3").SimplifiesToInteger, "7/3 is not an integer amount");
    }
    
    [Test]
    public void TestToInteger()
    {
      Assert.AreEqual(1, Fraction.Parse("3/3").ToInteger(), "3/3");
      Assert.AreEqual(-3, Fraction.Parse("-3/1").ToInteger(), "-3/1");
      Assert.AreEqual(2, Fraction.Parse("6/3").ToInteger(), "6/3");
    }
    
    [Test]
    public void TestToCompositeString()
    {
      Assert.AreEqual("2 2/3", new Fraction(8, 3).ToCompositeString(), "8/3 is 2 2/3");
      Assert.AreEqual("1/3", new Fraction(1, 3).ToCompositeString(), "1/3 is 1/3");
      Assert.AreEqual("-1/3", new Fraction(-1, 3).ToCompositeString(), "-1/3 is -1/3");
      Assert.AreEqual("-5 1/3", new Fraction(-16, 3).ToCompositeString(), "-16/3 is -5 1/3");
      Assert.AreEqual("-5", new Fraction(-15, 3).ToCompositeString(), "-15/3 is -5");
      Assert.AreEqual("10", new Fraction(20, 2).ToCompositeString(), "20/2 is 10");
    }
    
    #endregion
  }
}

