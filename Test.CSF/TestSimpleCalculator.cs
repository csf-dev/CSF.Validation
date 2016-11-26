//
// TestSimpleCalculator.cs
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
using System.Collections.Generic;
using CSF;

namespace Test.CSF
{
  [TestFixture]
  public class TestSimpleCalculator
  {
    [Test]
    public void TestCalculateDecimal()
    {
      ICalculator calculator = new SimpleCalculator();
      Dictionary<string, decimal> expectedValues = new Dictionary<string, decimal>();
      
      expectedValues.Add("=21.5+8.5"    , 30);
      expectedValues.Add("=6.10-0.25"   ,  5.85m);
      expectedValues.Add("=10*2.4"      , 24);
      expectedValues.Add("=6.33/3"      ,  2.11m);
      expectedValues.Add("=21.5+8.5+2.6", 32.6m);
      
      foreach(string calculation in expectedValues.Keys)
      {
        try
        {
          Assert.AreEqual(expectedValues[calculation],
                          calculator.CalculateDecimal(calculation),
                          String.Format("Calculation '{0}' is correct", calculation));
        }
        catch(Exception)
        {
          Console.Error.WriteLine("Error occured whilst calculating '{0}'", calculation);
          throw;
        }
      }
    }
    
    [Test]
    public void TestTryCalculateDecimal()
    {
      ICalculator calculator = new SimpleCalculator();
      Dictionary<string, decimal> expectedValues = new Dictionary<string, decimal>();
      decimal result;
      
      expectedValues.Add("=21.5+8.5"    , 30);
      expectedValues.Add("=6.10-0.25"   ,  5.85m);
      expectedValues.Add("=10*2.4"      , 24);
      expectedValues.Add("=6.33/3"      ,  2.11m);
      expectedValues.Add("2+2"          ,  4);
      expectedValues.Add("=21.5+8.5+2.6", 32.6m);
      
      foreach(string calculation in expectedValues.Keys)
      {
        bool success;
        
        success = calculator.TryCalculateDecimal(calculation, out result);
        
        Assert.IsTrue(success, String.Format("Success calculating '{0}'", calculation));
        Assert.AreEqual(expectedValues[calculation], result, String.Format("Calculation '{0}' is correct", calculation));
      }
      
      Assert.IsFalse(calculator.TryCalculateDecimal("invalid", out result), "Invalid calculation string");
      Assert.IsFalse(calculator.TryCalculateDecimal(null, out result), "Null calculation string");
      Assert.IsFalse(calculator.TryCalculateDecimal("=2+-2", out result), "Consecutive operators");
    }
    
    [Test]
    public void TestCalculateInteger()
    {
      ICalculator calculator = new SimpleCalculator();
      Dictionary<string, int> expectedValues = new Dictionary<string, int>();
      
      expectedValues.Add("=21+8"        , 29);
      expectedValues.Add("=6-1"         ,  5);
      expectedValues.Add("=10*2"        , 20);
      expectedValues.Add("=6/3"         ,  2);
      expectedValues.Add("=21+8+2"      , 31);
      
      foreach(string calculation in expectedValues.Keys)
      {
        Assert.AreEqual(expectedValues[calculation],
                        calculator.CalculateInteger(calculation),
                        String.Format("Calculation '{0}' is correct", calculation));
      }
    }
    
    [Test]
    public void TestTryCalculateInteger()
    {
      ICalculator calculator = new SimpleCalculator();
      Dictionary<string, int> expectedValues = new Dictionary<string, int>();
      int result;
      
      expectedValues.Add("=21+8"        , 29);
      expectedValues.Add("=6-1"         ,  5);
      expectedValues.Add("=10*2"        , 20);
      expectedValues.Add("=6/3"         ,  2);
      expectedValues.Add("2+2"          ,  4);
      expectedValues.Add("=21+8+2"      , 31);
      
      foreach(string calculation in expectedValues.Keys)
      {
        bool success;
        
        success = calculator.TryCalculateInteger(calculation, out result);
        
        Assert.IsTrue(success, String.Format("Success calculating '{0}'", calculation));
        Assert.AreEqual(expectedValues[calculation], result, String.Format("Calculation '{0}' is correct", calculation));
      }
      
      Assert.IsFalse(calculator.TryCalculateInteger("invalid", out result), "Invalid calculation string");
      Assert.IsFalse(calculator.TryCalculateInteger(null, out result), "Null calculation string");
      Assert.IsFalse(calculator.TryCalculateInteger("=2+-2", out result), "Consecutive operators");
    }
  }
}

