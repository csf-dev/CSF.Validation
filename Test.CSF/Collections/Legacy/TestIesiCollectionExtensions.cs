//
// TestIesiCollectionExtensions.cs
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
using Iesi.Collections.Generic;
using CSF.Collections.Legacy;

namespace Test.CSF.Collections.Legacy
{
  [TestFixture]
  public class TestIesiCollectionExtensions
  {
    [Test]
    public void TestToSet()
    {
      ICollection<int> integers = new int[] { 1, 3, 5, 7 };
      Iesi.Collections.Generic.ISet<int> setIntegers = integers.ToSet();

      Assert.IsNotNull(setIntegers, "Nullability");
      Assert.AreEqual(4, setIntegers.Count, "Count");
      Assert.IsTrue(setIntegers.Contains(1), "Contains 1");
      Assert.IsTrue(setIntegers.Contains(3), "Contains 3");
      Assert.IsTrue(setIntegers.Contains(5), "Contains 5");
      Assert.IsTrue(setIntegers.Contains(7), "Contains 7");
    }

    [Test]
    public void TestToSetDuplicates()
    {
      ICollection<int> integers = new int[] { 1, 5, 5, 7 };
      Iesi.Collections.Generic.ISet<int> setIntegers = integers.ToSet();

      Assert.IsNotNull(setIntegers, "Nullability");
      Assert.AreEqual(3, setIntegers.Count, "Count");
      Assert.IsTrue(setIntegers.Contains(1), "Contains 1");
      Assert.IsTrue(setIntegers.Contains(5), "Contains 5");
      Assert.IsTrue(setIntegers.Contains(7), "Contains 7");
    }
  }
}

