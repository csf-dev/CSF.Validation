using System;
using NUnit.Framework;
using CSF.Collections;
using System.Collections.Generic;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestIEnumerableExtensions
  {
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
  }
}

