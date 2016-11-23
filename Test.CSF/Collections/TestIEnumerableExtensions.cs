//
// TestIEnumerableExtensions.cs
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

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestIEnumerableExtensions
  {
    #region testing AreContentsSameAs

    [Test]
    public void AreContentsSameAs_returns_true_for_differently_ordered_collections()
    {
      var collection1 = new string[] {
        "foo", "foo", "bar", "baz"
      };

      var collection2 = new string[] {
        "baz", "bar", "foo", "foo"
      };

      Assert.IsTrue(collection1.AreContentsSameAs(collection2), "Contents are the same");
    }

    [Test]
    public void AreContentsSameAs_returns_true_for_reference_equal_collections()
    {
      var collection1 = new string[] {
        "foo", "foo", "bar", "baz"
      };

      Assert.IsTrue(collection1.AreContentsSameAs(collection1), "Contents are the same");
    }

    [Test]
    public void AreContentsSameAs_returns_false_for_collections_with_same_items_but_different_recurrances()
    {
      var collection1 = new string[] {
        "foo", "foo", "bar", "baz"
      };

      var collection2 = new string[] {
        "baz", "bar", "foo", "bar"
      };

      Assert.IsFalse(collection1.AreContentsSameAs(collection2), "Contents are the same");
    }

    [Test]
    public void AreContentsSameAs_returns_false_for_collections_with_different_items()
    {
      var collection1 = new string[] {
        "foo", "foo", "bar", "wibble"
      };

      var collection2 = new string[] {
        "baz", "bar", "foo", "foo"
      };

      Assert.IsFalse(collection1.AreContentsSameAs(collection2), "Contents are the same");
    }

    [Test]
    public void AreContentsSameAs_returns_true_for_collections_using_a_custom_item_comparer()
    {
      var collection1 = new string[] {
        "foo", "foo", "flam", "flom"
      };

      var collection2 = new string[] {
        "floop", "fwoop", "foo", "foo"
      };

      Assert.IsTrue(collection1.AreContentsSameAs(collection2, new FirstCharacterEqualityComparer()),
                    "Contents are the same");
    }

    #endregion

    #region contained type

    class FirstCharacterEqualityComparer : IEqualityComparer<string>
    {
      #region IEqualityComparer implementation

      public bool Equals (string x, string y)
      {
        return (!String.IsNullOrEmpty(x)
                && !String.IsNullOrEmpty(y)
                && x[0] == y[0]);
      }

      public int GetHashCode (string obj)
      {
        return (obj.Length > 0)? obj[0].GetHashCode() : 0;
      }

      #endregion
    }

    #endregion
  }
}

