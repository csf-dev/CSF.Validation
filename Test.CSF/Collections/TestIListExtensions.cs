using System;
using NUnit.Framework;
using CSF.Collections;
using System.Collections.Generic;

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

