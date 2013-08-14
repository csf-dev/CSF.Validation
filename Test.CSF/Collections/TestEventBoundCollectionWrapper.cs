using System;
using NUnit.Framework;
using CSF.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestEventBoundCollectionWrapper
  {
    #region fields

    private StubClass[] OriginalList;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      OriginalList = new StubClass[] {
        new StubClass() { AProperty = "One" },
        new StubClass() { AProperty = "Two" },
        new StubClass() { AProperty = "Three" }
      };
    }

    #endregion

    #region testing new functionality

    [Test]
    public void TestAdd()
    {
      EventBoundCollectionWrapper<StubClass> list;

      list = new EventBoundCollectionWrapper<StubClass>(new List<StubClass>(OriginalList));
      list.BeforeAdd = (coll, item) => { item.AProperty = "BeforeAdd"; return true; };
      StubClass added = new StubClass();
      list.Add(added);

      Assert.AreEqual(4, list.Count, "Count");
      Assert.AreSame(added, list.Last(), "Same instance");
      Assert.AreEqual("BeforeAdd", added.AProperty, "Property is set");
    }

    [Test]
    public void TestRemove()
    {
      EventBoundCollectionWrapper<StubClass> list;

      list = new EventBoundCollectionWrapper<StubClass>(new List<StubClass>(OriginalList));
      list.BeforeRemove = (coll, item) => { item.AProperty = "BeforeRemove"; return true; };
      StubClass removed = list.Last();
      list.Remove(removed);

      Assert.AreEqual(2, list.Count, "Count");
      Assert.AreEqual("BeforeRemove", removed.AProperty, "Property is set");
    }

    #endregion

    #region testing existing functionality

    [Test]
    public void TestCount()
    {
      EventBoundCollectionWrapper<StubClass> list;

      list = new EventBoundCollectionWrapper<StubClass>(new List<StubClass>(OriginalList));
      Assert.AreEqual(3, list.Count);
    }

    [Test]
    public void TestIsReadOnly()
    {
      EventBoundCollectionWrapper<StubClass> list;

      list = new EventBoundCollectionWrapper<StubClass>(new List<StubClass>(OriginalList));
      Assert.IsFalse(list.IsReadOnly, "Not read only");

      list = new EventBoundCollectionWrapper<StubClass>(OriginalList);
      Assert.IsTrue(list.IsReadOnly, "Read only");
    }

    #endregion

    #region contained type

    class StubClass
    {
      public string AProperty { get; set; }
    }

    #endregion
  }
}

