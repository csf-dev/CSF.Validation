//
// TestDictionaryEqualityComparer.cs
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
using System.Collections.Generic;
using System.Collections;

namespace Test.CSF
{
  [TestFixture]
  public class TestDictionaryEqualityComparer
  {
    [Test]
    public void TestAreEqualSuccess()
    {
      Dictionary<int, string>
        dictionary1 = new Dictionary<int, string>(),
        dictionary2 = new Dictionary<int, string>();
      IEqualityComparer comparer = new DictionaryEqualityComparer<int, string>();

      dictionary1.Add(1, "One");
      dictionary1.Add(2, "Two");
      dictionary1.Add(3, "Three");
      dictionary1.Add(4, "Four");

      dictionary2.Add(3, "Three");
      dictionary2.Add(1, "One");
      dictionary2.Add(4, "Four");
      dictionary2.Add(2, "Two");

      Assert.IsTrue(comparer.Equals(dictionary1, dictionary1), "Reference equality");
      Assert.IsTrue(comparer.Equals(dictionary1, dictionary2), "Dictionaries are equal");
    }

    [Test]
    public void TestAreEqualFailure()
    {
      Dictionary<int, string>
        dictionary1 = new Dictionary<int, string>(),
        dictionary2 = new Dictionary<int, string>();
      IEqualityComparer comparer = new DictionaryEqualityComparer<int, string>();

      dictionary1.Add(1, "One");
      dictionary1.Add(2, "Two");
      dictionary1.Add(3, "Three");
      dictionary1.Add(4, "Four");

      dictionary2.Add(3, "Three");
      dictionary2.Add(1, "One");
      dictionary2.Add(5, "Five");
      dictionary2.Add(2, "Two");

      Assert.IsFalse(comparer.Equals(dictionary1, dictionary2), "Dictionaries are not equal");
    }
  }
}

