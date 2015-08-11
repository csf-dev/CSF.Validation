using System;
using NUnit.Framework;
using CSF.Collections;
using Iesi.Collections.Generic;
using Iesi.Collections;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestEventBoundSetWrapper
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
      EventBoundSetWrapper<StubClass> list;

      list = new EventBoundSetWrapper<StubClass>(new HashedSet<StubClass>(OriginalList));
      list.BeforeAdd = (coll, item) => { item.AProperty = "BeforeAdd"; return true; };
      StubClass added = new StubClass();
      list.Add(added);

      Assert.AreEqual(4, list.Count, "Count");
      Assert.IsTrue(list.Contains(added), "Same instance");
      Assert.AreEqual("BeforeAdd", added.AProperty, "Property is set");
    }

    [Test]
    public void TestRemove()
    {
      EventBoundSetWrapper<StubClass> list;

      list = new EventBoundSetWrapper<StubClass>(new HashedSet<StubClass>(OriginalList));
      list.BeforeRemove = (coll, item) => { item.AProperty = "BeforeRemove"; return true; };
      StubClass removed = OriginalList[2];
      list.Remove(removed);

      Assert.AreEqual(2, list.Count, "Count");
      Assert.AreEqual("BeforeRemove", removed.AProperty, "Property is set");
    }

    [Test]
    [Description("This tests that an event bound set wrapper implements both the generic and non-generic ISet" +
                 "interfaces")]
    public void TestImplementsCorrectInterface()
    {
      EventBoundSetWrapper<StubClass> list;

      list = new EventBoundSetWrapper<StubClass>(new HashedSet<StubClass>(OriginalList));

      Assert.IsInstanceOf<ISet<StubClass>>(list, "Generic ISet");
      Assert.IsInstanceOf<ISet>(list, "Non-generic ISet");
    }

    #endregion

    #region testing existing functionality

    [Test]
    public void TestCount()
    {
      EventBoundSetWrapper<StubClass> list;

      list = new EventBoundSetWrapper<StubClass>(new HashedSet<StubClass>(OriginalList));
      Assert.AreEqual(3, list.Count);
    }

    [Test]
    public void TestIsReadOnly()
    {
      EventBoundSetWrapper<StubClass> list;

      list = new EventBoundSetWrapper<StubClass>(new HashedSet<StubClass>(OriginalList));
      Assert.IsFalse(list.IsReadOnly, "Not read only");

      list = new EventBoundSetWrapper<StubClass>(new ImmutableSet<StubClass>(OriginalList.ToSet()));
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

