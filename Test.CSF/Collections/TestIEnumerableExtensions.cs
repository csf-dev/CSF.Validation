using System;
using NUnit.Framework;
using CSF.Collections;
using System.Collections.Generic;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestIEnumerableExtensions
  {
    #region testing ToSeparatedString

    [Test]
    public void TestToSeparatedString()
    {
      int[] foo = {1, 2, 3, 4, 5, 10, 20};
      Assert.AreEqual("1, 2, 3, 4, 5, 10, 20", foo.ToSeparatedString(", "), "Correct with ', ' separator");
      Assert.AreEqual("123451020", foo.ToSeparatedString(String.Empty), "Correct with empty separator");
    }

    [Test]
    public void TestToSeparatedStringGeneric()
    {
      IEnumerable<int> foo = new int[] {1, 2, 3, 4, 5, 10, 20};
      Assert.AreEqual("1, 2, 3, 4, 5, 10, 20", foo.ToSeparatedString(", "), "Correct with ', ' separator");
      Assert.AreEqual("123451020", foo.ToSeparatedString(String.Empty), "Correct with empty separator");
    }

    [Test]
    public void TestToSeparatedStringGenericSelector()
    {
      IEnumerable<DateTime> foo = new DateTime[] { new DateTime(1999,1,1), new DateTime(2000,1,1), new DateTime(2001,1,1) };
      Assert.AreEqual("1999,2000,2001",
                      foo.ToSeparatedString(",", x => x.Year),
                      "Correct with ',' separator");
    }

    #endregion

    #region testing AreContentsSameAs

    [Test]
    public void TestAreContentsSameAs()
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
    public void TestAreContentsSameAsRefEqual()
    {
      var collection1 = new string[] {
        "foo", "foo", "bar", "baz"
      };

      Assert.IsTrue(collection1.AreContentsSameAs(collection1), "Contents are the same");
    }

    [Test]
    public void TestAreContentsSameAsDifferentCounts()
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
    public void TestAreContentsSameAsDifferentItems()
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
    public void TestAreContentsSameAsCustomComparer()
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

    [Test]
    public void TestAreContentsSameAsWithoutCustomComparer()
    {
      var collection1 = new string[] {
        "foo", "foo", "flam", "flom"
      };

      var collection2 = new string[] {
        "floop", "fwoop", "foo", "foo"
      };

      Assert.IsFalse(collection1.AreContentsSameAs(collection2), "Contents are the same");
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

