//
// TestNameValueCollectionExtensions.cs
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
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestNameValueCollectionExtensions
  {
    [Test]
    public void TestToDictionary()
    {
      NameValueCollection collection = new NameValueCollection();

      collection.Add("foo", "bar");
      collection.Add("foo", "baz");
      collection.Add("spong", "wibble");

      IDictionary<string,string> clone = collection.ToDictionary();

      Assert.AreEqual(2, clone.Count, "Count of elements");
      Assert.AreEqual("bar,baz", clone["foo"], "Content for 'foo'");
      Assert.AreEqual("wibble", clone["spong"], "Content for 'spong'");
    }
  }
}

