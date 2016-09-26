//
// TestReferenceList.cs
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
using System.Collections.Generic;
using CSF.Collections;
using System.Linq;
using CSF.Collections.EventHandling;

namespace Test.CSF.Collections
{
  [TestFixture]
  public class TestReferenceList
  {
    #region tests

    [Test]
    public void TestGetOrInitAdd()
    {
      Order order = new Order();
      LineItem item1 = new LineItem();

      order.LineItems.Add(item1);

      Assert.IsNotNull(item1.Order, "Nullability");
      Assert.AreSame(order, item1.Order, "Correct order");
      Assert.AreEqual(1, order.LineItems.Count, "Correct count of items");

      Assert.AreEqual(1, order.SourceList.Count, "Correct count of items in source list");
      Assert.AreSame(item1, order.SourceList.First(), "Correct item in source list");
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestGetOrInitAddNull()
    {
      Order order = new Order();
      order.LineItems.Add(null);
    }

    [Test]
    [Description("This test highlights an issue whereby the collections could come out of sync if the source list is " +
                 "replaced directly after the wrapper has been initialised.  This is an important scenario because " +
                 "an ORM such as NHibernate could modify the source list directly behind the scenes.")]
    public void TestGetOrInitAfterSourceListReplaced()
    {
      Order order = new Order();

      LineItem
        item1 = new LineItem(),
        item2 = new LineItem();

      // This forces the wrapper to initialise by touching the collection property.
      var wrappedList = order.LineItems;
      Assert.IsNotNull(wrappedList, "Wrapped list not null");

      // Now we switch the source list directly without going through the wrapper.
      order.SourceList = new List<LineItem>(new LineItem[] { item1, item2 });

      Assert.AreEqual(2, order.LineItems.Count, "Correct count of items");
      Assert.IsTrue(order.LineItems.Contains(item1), "Wrapped list contains appropriate item");
    }

    [Test]
    public void TestGetOrInitRemove()
    {
      Order order = new Order();
      LineItem item1 = new LineItem();

      order.LineItems.Add(item1);
      order.LineItems.Remove(item1);

      Assert.IsNull(item1.Order, "Nullability after removal");
      Assert.AreEqual(0, order.LineItems.Count, "Correct count of items");

      Assert.AreEqual(0, order.SourceList.Count, "Correct count of items in source list");
    }

    [Test]
    public void TestReplace()
    {
      Order order = new Order();
      LineItem item1 = new LineItem();
      LineItem item2 = new LineItem();
      LineItem item3 = new LineItem();
      LineItem item4 = new LineItem();

      order.LineItems.Add(item1);
      order.LineItems.Add(item2);

      order.LineItems = new List<LineItem>(new LineItem[] { item3, item4 });

      Assert.IsNull(item1.Order, "Item 1 order nullability");
      Assert.IsNull(item2.Order, "Item 2 order nullability");
      Assert.IsNotNull(item3.Order, "Item 3 order nullability");
      Assert.IsNotNull(item4.Order, "Item 4 order nullability");
      Assert.AreSame(order, item3.Order, "Item 3 order");
      Assert.AreSame(order, item4.Order, "Item 4 order");
      Assert.AreEqual(2, order.LineItems.Count, "Correct count of items");
    }

    [Test]
    public void TestReplaceWithEmptyCollection()
    {
      Order order = new Order();
      LineItem item1 = new LineItem();
      LineItem item2 = new LineItem();

      order.LineItems.Add(item1);
      order.LineItems.Add(item2);

      order.LineItems = new List<LineItem>();

      Assert.IsNull(item1.Order, "Item 1 order nullability");
      Assert.IsNull(item2.Order, "Item 2 order nullability");
      Assert.AreEqual(0, order.LineItems.Count, "Correct count of items");
    }

    [Test]
    public void TestReplaceWithEmptyCollectionThenRemoveNonexistentItem()
    {
      Order order = new Order();
      LineItem item1 = new LineItem();
      LineItem item2 = new LineItem();

      order.LineItems.Add(item1);
      order.LineItems.Add(item2);

      order.LineItems = new List<LineItem>();

      Assert.IsNull(item1.Order, "Item 1 order nullability");
      Assert.IsNull(item2.Order, "Item 2 order nullability");
      Assert.AreEqual(0, order.LineItems.Count, "Correct count of items");

      Assert.IsFalse(order.LineItems.Remove(item1), "Result of removing non-existent item");
    }

//    [Test]
//    public void TestAddToDifferentCollection()
//    {
//      Order
//        order = new Order(),
//        orderTwo = new Order();
//      LineItem item1 = new LineItem();
//
//      order.LineItems.Add(item1);
//      orderTwo.LineItems.Add(item1);
//
//      Assert.IsNotNull(item1.Order, "Item 1 order nullability");
//      Assert.AreSame(orderTwo, item1.Order, "Correct item 1 order.");
//      Assert.AreEqual(1, orderTwo.LineItems.Count, "Correct count of items in order 2");
//      Assert.AreEqual(0, order.LineItems.Count, "Correct count of items in order 1");
//    }

    #endregion

    #region contained types

    public class Order
    {
      private IList<LineItem> _lineItems, _wrappedLineItems;

      public IList<LineItem> LineItems
      {
        get {
          return ReferenceList.GetOrInit(ref _wrappedLineItems, ref _lineItems, x => x.Order, this);
        }
        set {
          ReferenceList.Replace(ref _wrappedLineItems, value, x => x.Order, this);
          _lineItems = value;
        }
      }

      public IList<LineItem> SourceList
      {
        get {
          return _lineItems;
        }
        set {
          _lineItems = value;
        }
      }

      public Order ()
      {
        _lineItems = new List<LineItem>();
      }
    }

    public class LineItem
    {
      public Order Order { get; set; }
    }

    #endregion
  }
}

