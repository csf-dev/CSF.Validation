//
// TestPrimeFactoriser.cs
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
using System.Linq;
using System.Collections.Generic;

namespace Test.CSF
{
  [TestFixture]
  public class TestPrimeFactoriser
  {
    #region tests
    
    [Test]
    public void TestGenerate()
    {
      PrimeFactoriser generator = PrimeFactoriser.Default;
      IList<int> results  = generator.GeneratePrimes(15).ToList();
      
      Assert.AreEqual(6, results.Count, "Correct count");
      Assert.IsTrue(results.Contains(2), "Contains 2");
      Assert.IsTrue(results.Contains(3), "Contains 3");
      Assert.IsTrue(results.Contains(5), "Contains 5");
      Assert.IsTrue(results.Contains(7), "Contains 7");
      Assert.IsTrue(results.Contains(11), "Contains 11");
      Assert.IsTrue(results.Contains(13), "Contains 13");
    }
    
    [Test]
    public void TestGenerateRepeatedly()
    {
      PrimeFactoriser generator = PrimeFactoriser.Default;
      IList<int> results  = generator.GeneratePrimes(15).ToList();
      
      Assert.IsTrue(results.Contains(2), "Contains 2");
      Assert.IsTrue(results.Contains(3), "Contains 3");
      Assert.IsTrue(results.Contains(5), "Contains 5");
      Assert.IsTrue(results.Contains(7), "Contains 7");
      Assert.IsTrue(results.Contains(11), "Contains 11");
      Assert.IsTrue(results.Contains(13), "Contains 13");
      Assert.AreEqual(6, results.Count, "Correct count");
      
      results  = generator.GeneratePrimes(15).ToList();
      
      Assert.IsTrue(results.Contains(2), "Still contains 2");
      Assert.IsTrue(results.Contains(3), "Still contains 3");
      Assert.IsTrue(results.Contains(5), "Still contains 5");
      Assert.IsTrue(results.Contains(7), "Still contains 7");
      Assert.IsTrue(results.Contains(11), "Still contains 11");
      Assert.IsTrue(results.Contains(13), "Still contains 13");
      Assert.AreEqual(6, results.Count, "Still correct count");
    }
    
    [Test]
    [Explicit("This test takes a long time to run, depending on your CPU.  An Intel i5 can run this in about 40 seconds")]
    public void TestGenerateLots()
    {
      PrimeFactoriser generator = PrimeFactoriser.Default;
      IList<int> results  = generator.GeneratePrimes(1299709).ToList();
      Assert.AreEqual(100000, results.Count, "Correct count");
    }
    
    [Test]
    public void TestGetPrimeFactors()
    {
      PrimeFactoriser generator = PrimeFactoriser.Default;
      IList<int> results = generator.GetPrimeFactors(5775).ToList();
      
      Assert.IsTrue(results.Contains(5), "Contains 5");
      Assert.IsTrue(results.Contains(3), "Contains 3");
      Assert.IsTrue(results.Contains(11), "Contains 11");
      Assert.IsTrue(results.Contains(7), "Contains 7");
      Assert.AreEqual(5, results.Count, "Correct count");
    }
    
    [Test]
    public void TestGetCommonPrimeFactors()
    {
      PrimeFactoriser generator = PrimeFactoriser.Default;
      IList<int> results = generator.GetCommonPrimeFactors(5775, 23100).ToList();
      
      Assert.IsTrue(results.Contains(5), "Contains 5");
      Assert.IsTrue(results.Contains(3), "Contains 3");
      Assert.IsTrue(results.Contains(11), "Contains 11");
      Assert.IsTrue(results.Contains(7), "Contains 7");
      Assert.AreEqual(5, results.Count, "Correct count");
    }
    
    #endregion
  }
}

