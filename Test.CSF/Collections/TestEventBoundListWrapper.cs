//
// TestEventBoundListWrapper.cs
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
  public class TestEventBoundListWrapper
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
      EventBoundListWrapper<StubClass> list;

      list = new EventBoundListWrapper<StubClass>(new List<StubClass>(OriginalList));
      list.BeforeAdd = (coll, item) => { item.AProperty = "BeforeAdd"; return true; };
      StubClass added = new StubClass();
      list.Add(added);

      Assert.AreEqual(4, list.Count, "Count");
      Assert.AreSame(added, list[3], "Same instance");
      Assert.AreEqual("BeforeAdd", added.AProperty, "Property is set");
    }

    [Test]
    public void TestRemove()
    {
      EventBoundListWrapper<StubClass> list;

      list = new EventBoundListWrapper<StubClass>(new List<StubClass>(OriginalList));
      list.BeforeRemove = (coll, item) => { item.AProperty = "BeforeRemove"; return true; };
      StubClass removed = list[2];
      list.Remove(removed);

      Assert.AreEqual(2, list.Count, "Count");
      Assert.AreEqual("BeforeRemove", removed.AProperty, "Property is set");
    }

    #endregion

    #region testing existing functionality

    [Test]
    public void TestCount()
    {
      EventBoundListWrapper<StubClass> list;

      list = new EventBoundListWrapper<StubClass>(new List<StubClass>(OriginalList));
      Assert.AreEqual(3, list.Count);
    }

    [Test]
    public void TestIsReadOnly()
    {
      EventBoundListWrapper<StubClass> list;

      list = new EventBoundListWrapper<StubClass>(new List<StubClass>(OriginalList));
      Assert.IsFalse(list.IsReadOnly, "Not read only");

      list = new EventBoundListWrapper<StubClass>(OriginalList);
      Assert.IsTrue(list.IsReadOnly, "Read only");
    }

    [Test]
    public void TestIndexerGet()
    {
      EventBoundListWrapper<StubClass> list;

      list = new EventBoundListWrapper<StubClass>(new List<StubClass>(OriginalList));
      Assert.AreSame(OriginalList[1], list[1]);
    }

    [Test]
    public void TestIndexerSet()
    {
      EventBoundListWrapper<StubClass> list;

      list = new EventBoundListWrapper<StubClass>(new List<StubClass>(OriginalList));
      list[1] = new StubClass();
      Assert.AreNotSame(OriginalList[1], list[1]);
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

