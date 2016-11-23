//
// TestOrderNeutralEqualityComparer.cs
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
using System.Collections;
using System.Collections.Generic;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestOrderNeutralEqualityComparer
  {
    [Test]
    public void AreEqual_returns_true_for_collections_with_same_elements()
    {
      IEqualityComparer comparer = new OrderNeutralEqualityComparer<string>();

      IList<string> listOne = new string[] { "foo", "bar", "baz" };
      IList<string> listTwo = new string[] { "bar", "foo", "baz" };

      Assert.IsTrue(comparer.Equals(listOne, listTwo), "Lists are equal");
    }

    [Test]
    public void AreEqual_returns_false_for_collections_with_different_elements()
    {
      IEqualityComparer comparer = new OrderNeutralEqualityComparer<string>();

      IList<string> listOne = new string[] { "foo", "bar", "baz" };
      IList<string> listThree = new string[] { "bar", "foo", "quux" };

      Assert.IsFalse(comparer.Equals(listOne, listThree), "Lists are not equal");
    }
  }
}

