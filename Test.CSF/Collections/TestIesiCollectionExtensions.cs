using System;
using NUnit.Framework;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using CSF.Collections;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestIesiCollectionExtensions
  {
    [Test]
    public void TestToSet()
    {
      ICollection<int> integers = new int[] { 1, 3, 5, 7 };
      ISet<int> setIntegers = integers.ToSet();

      Assert.IsNotNull(setIntegers, "Nullability");
      Assert.AreEqual(4, setIntegers.Count, "Count");
      Assert.IsTrue(setIntegers.Contains(1), "Contains 1");
      Assert.IsTrue(setIntegers.Contains(3), "Contains 3");
      Assert.IsTrue(setIntegers.Contains(5), "Contains 5");
      Assert.IsTrue(setIntegers.Contains(7), "Contains 7");
    }

    [Test]
    public void TestToSetDuplicates()
    {
      ICollection<int> integers = new int[] { 1, 5, 5, 7 };
      ISet<int> setIntegers = integers.ToSet();

      Assert.IsNotNull(setIntegers, "Nullability");
      Assert.AreEqual(3, setIntegers.Count, "Count");
      Assert.IsTrue(setIntegers.Contains(1), "Contains 1");
      Assert.IsTrue(setIntegers.Contains(5), "Contains 5");
      Assert.IsTrue(setIntegers.Contains(7), "Contains 7");
    }
  }
}

