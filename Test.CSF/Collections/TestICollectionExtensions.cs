using System;
using NUnit.Framework;
using System.Collections.Generic;
using CSF.Collections;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestICollectionExtensions
  {
    [Test]
    public void TestReplaceContents()
    {
      List<string> list = new List<string>(new string[] { "A", "B", "C" });

      list.ReplaceContents(new string[] { "X", "Y", "Z" });

      Assert.AreEqual(3, list.Count, "Count of items");
      Assert.IsTrue(list.Contains("X"), "Contained item 1");
      Assert.IsTrue(list.Contains("Y"), "Contained item 2");
      Assert.IsTrue(list.Contains("Z"), "Contained item 3");
    }
  }
}

