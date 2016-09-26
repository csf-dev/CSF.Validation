//
// TestIListExtensions.cs
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
using CSF.Collections;
using System.Collections.Generic;
using CSF.Collections.EventHandling;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestIListExtensions
  {
    #region tests

    [Test]
    public void TestWrapWithBeforeActions()
    {
      string test = String.Empty;

      IList<string> source = new List<string>();
      IList<string> wrapped = source.WrapWithBeforeActions(x => { test = String.Concat(test, x); }, x => {});

      wrapped.Add("foo");
      wrapped.Add("bar");
      wrapped.Add("baz");

      Assert.AreEqual("foobarbaz", test, "Test string (modified by actions) is correct");
      Assert.AreEqual(3, wrapped.Count, "Correct count (wrapped)");
      Assert.AreEqual(3, source.Count, "Correct count (source)");
    }

    [Test]
    public void TestWrapWithBeforeActionsWithFunctions()
    {
      string test = String.Empty;

      IList<string> source = new List<string>();
      IList<string> wrapped = source.WrapWithBeforeActions((list, x) => {
        bool output = (x.Length % 2 == 0);
        if(output) { test = String.Concat(test, x); }
        return output;
      }, (list, x) => { return true; });

      wrapped.Add("fork");
      wrapped.Add("foo");
      wrapped.Add("Splatter");

      Assert.AreEqual("forkSplatter", test, "Test value (modified by actions) is correct");
      Assert.AreEqual(2, wrapped.Count, "Correct count (wrapped)");
      Assert.AreEqual(2, source.Count, "Correct count (source)");
    }

    [Test]
    public void TestToReadOnlyList()
    {
      IList<int> integers = new List<int>();

      integers.Add(2);
      integers.Add(3);
      integers.Add(4);
      integers.Add(5);

      IList<int> readonlyIntegers = integers.ToReadOnlyList();

      Assert.IsNotNull(readonlyIntegers, "Output nullability.");
      Assert.IsTrue(readonlyIntegers.IsReadOnly, "Output read-only state.");
      Assert.AreEqual(integers.Count, readonlyIntegers.Count, "Output count of elements.");
    }

    [Test]
    public void TestToReadOnlyListAlreadyReadOnly()
    {
      IList<int> integers = new int[] { 2,3,4,5 };

      IList<int> readonlyIntegers = integers.ToReadOnlyList();

      Assert.IsNotNull(readonlyIntegers, "Output nullability.");
      Assert.IsTrue(readonlyIntegers.IsReadOnly, "Output read-only state.");
      Assert.AreEqual(integers.Count, readonlyIntegers.Count, "Output count of elements.");
      Assert.AreSame(integers, readonlyIntegers, "Output and source are same object reference.");
    }

    #endregion
  }
}

