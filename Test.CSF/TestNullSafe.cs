//
// TestNullSafe.cs
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
using CSF;
using NUnit.Framework;

namespace Test.CSF
{
  [TestFixture]
  public class TestNullSafe
  {
    #region tests

    [Test]
    public void TestToUlong()
    {
      ulong? val = NullSafe.ConvertTo<ulong>("5");

      Assert.IsTrue(val.HasValue);
      Assert.AreEqual(5, val.Value);
    }

    [Test]
    public void TestToDate()
    {
      DateTime? val = NullSafe.ConvertTo<DateTime>("2012-10-19");

      Assert.IsTrue(val.HasValue);
      Assert.AreEqual(new DateTime(2012, 10, 19), val.Value);
    }

    [Test]
    public void TestToFailures()
    {
      int? val = NullSafe.ConvertTo<int>("foo");

      Assert.IsFalse(val.HasValue);

      val = NullSafe.ConvertTo<int>(null);

      Assert.IsFalse(val.HasValue);
    }

    [Test]
    public void TestGet()
    {
      var root = new MockOuter() { Inner = new MockInner() { Value = 5 } };
      bool result;
      int val;

      result = NullSafe.Get(root, x => x.Inner.Value, out val);

      Assert.IsTrue(result);
      Assert.AreEqual(5, val);

      root.Inner = null;

      result = NullSafe.Get(root, x => x.Inner.Value, out val);

      Assert.IsFalse(result);
    }

    [Test]
    public void TestToString()
    {
      var root = new MockOuter() { Inner = new MockInner() { Value = 5 } };
      string result;

      result = NullSafe.ToString(root, x => x.Inner.Value);

      Assert.AreEqual("5", result);

      root.Inner = null;

      result = NullSafe.ToString(root, x => x.Inner.Value);

      Assert.IsNull(result);
    }

    #endregion

    #region contained types

    class MockOuter
    {
      public MockInner Inner { get; set; }
    }

    class MockInner
    {
      public int Value { get; set; }
    }

    #endregion
  }
}

