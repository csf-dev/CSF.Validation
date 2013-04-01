//  
//  TestEntity.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2012 CSF Software Limited
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using CSF.Entities;
using NUnit.Framework;
using System.Collections.Generic;

namespace Test.CSF.Entities
{
  [TestFixture]
  public class TestEntity
  {
    #region general tests
    
    [Test]
    public void TestHasIdentity()
    {
      Entity<Person,uint> entity = new Entity<Person,uint>();
      
      Assert.IsFalse(entity.HasIdentity, "Entity has no identity");
      
      entity.SetIdentity(5);
      Assert.IsTrue(entity.HasIdentity, "Entity has an identity");
    }
    
    [Test]
    public void TestGetIdentity()
    {
      Person entity = new Person() { Id = 5 };
      IIdentity identity = entity.GetIdentity();
      Assert.AreEqual(String.Format("[{0}: {1}]", typeof(Person).FullName, 5),
                      identity.ToString(),
                      "Correct identity");
    }
    
    [Test]
    public void TestSetIdentity()
    {
      Person entity = new Person();
      IIdentity identity;
      
      entity.SetIdentity(5);
      identity = entity.GetIdentity();
      Assert.AreEqual(String.Format("[{0}: {1}]", typeof(Person).FullName, 5),
                      identity.ToString(),
                      "Correct identity");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(ArgumentException), ExpectedMessage = "Invalid identity value")]
    public void TestSetIdentityInvalid()
    {
      Entity<Person,uint> entity = new Entity<Person,uint>();
      entity.SetIdentity(0);
      Assert.Fail("Test should never reach this point");
    }
    
    [Test]
    public void TestClearIdentity()
    {
      Person entity = new Person() { Id = 5 };
      
      Assert.IsTrue(entity.HasIdentity, "Has identity");
      entity.ClearIdentity();
      Assert.IsFalse(entity.HasIdentity, "Identity removed");
    }
    
    [Test]
    public void TestValidateIdentity()
    {
      Person entity = new Person();
      Assert.IsTrue(entity.ValidateIdentity(5), "Valid identity");
      Assert.IsFalse(entity.ValidateIdentity(0), "Invalid identity");
    }
    
    [Test]
    public void TestEquals()
    {
      string stringTest = "foo bar";
      uint numericTest = 3; 
      Person three = new Person() { Id = 3 };
      Person four = new Person() { Id = 4 };
      Person threeAgain = new Person() { Id = 3 };
      Product threeProduct = new Product() { Id = 3 };
      
      Assert.IsFalse(three.Equals(stringTest), "Entity does not equal a string");
      Assert.IsFalse(three.Equals(numericTest), "Entity does not equal a uint");
      Assert.IsFalse(three.Equals(four), "Non-matching identities not equal");
      
      Assert.IsTrue(three.Equals(three), "Copies of the same object are equal");
      Assert.IsTrue(three.Equals(threeAgain), "Identical identities are equal");
      
      Assert.IsFalse(three.Equals(threeProduct), "Non-matching types not equal");
    }
    
    [Test]
    public void TestToString()
    {
      Person entity = new Person() { Id = 5 };
      
      Assert.AreEqual(String.Format("[{0}: {1}]", typeof(Person).FullName, 5),
                      entity.ToString(),
                      "Correct identity");
      
      entity.ClearIdentity();
      Assert.AreEqual(String.Format("[{0}: {1}]", typeof(Person).FullName, "no identity"),
                      entity.ToString(),
                      "Correct identity");
    }
    
    [Test]
    public void TestOperatorEquality()
    {
      Person three = new Person() { Id = 3 };
      Person four = new Person() { Id = 4 };
      Person threeAgain = new Person() { Id = 3 };
      Product threeProduct = new Product() { Id = 3 };
      
      Assert.IsFalse(three == four, "Non-matching identities not equal");
#pragma warning disable 1718
      // Disabling CS1718 - the point of this test is to compare the object to itself
      Assert.IsTrue(three == three, "Copies of the same object are equal");
#pragma warning restore 1718
      Assert.IsTrue(three == threeAgain, "Identical instances are equal");
      Assert.IsFalse(three == threeProduct, "Non-matching types not equal");
    }
    
    [Test]
    public void TestOperatorInequality()
    {
      Person three = new Person() { Id = 3 };
      Person four = new Person() { Id = 4 };
      Person threeAgain = new Person() { Id = 3 };
      Product threeProduct = new Product() { Id = 3 };
      
      Assert.IsTrue(three != four, "Non-matching identities not equal");
#pragma warning disable 1718
      // Disabling CS1718 - the point of this test is to compare the object to itself
      Assert.IsFalse(three != three, "Copies of the same object are equal");
#pragma warning restore 1718
      Assert.IsFalse(three != threeAgain, "Identical instances are equal");
      Assert.IsTrue(three != threeProduct, "Non-matching types not equal");
    }

    #endregion

    #region testing event-bound reference lists

    [Test]
    [Description("Tests using the 'Add' method to add items to an existing list.")]
    public void TestGetOneToManyReferenceListAdd()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order(),
        orderThree = new Order();

      person.Orders.Add(orderOne);
      person.Orders.Add(orderTwo);

      Assert.AreEqual(2, person.Orders.Count, "Order count");
      Assert.AreEqual(2, person.SourceList.Count, "Order count (source list)");
      Assert.AreSame(person, orderOne.Owner, "Owner (order 1)");
      Assert.AreSame(person, orderTwo.Owner, "Owner (order 2)");
      Assert.IsNull(orderThree.Owner, "Owner (order 3)");
    }

    [Test]
    [Description("Tests using the 'Remove' method to remove items from an existing list.")]
    public void TestGetOneToManyReferenceListRemove()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order();

      person.Orders = new List<Order>(new Order[] { orderOne, orderTwo });

      Assert.IsTrue(person.Orders.Remove(orderTwo), "Return value of 'Remove' method");
      Assert.IsNull(orderTwo.Owner, "Owner after removal (order 2)");
      Assert.AreEqual(1, person.Orders.Count, "Order count");
      Assert.AreEqual(1, person.SourceList.Count, "Order count (source list)");
    }

    [Test]
    [Description("Tests replacing a list with a new one.")]
    public void TestGetOneToManyReferenceListReplaceList()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order(),
        orderThree = new Order();

      person.Orders = new Order[] { orderOne, orderTwo };
      person.Orders = new Order[] { orderThree };

      Assert.IsNull(orderOne.Owner, "Owner after replacement (order 1)");
      Assert.IsNull(orderTwo.Owner, "Owner after replacement (order 2)");
      Assert.AreSame(person, orderThree.Owner, "Owner after replacement (order 3)");
      Assert.AreEqual(1, person.Orders.Count, "Order count");
      Assert.AreEqual(1, person.SourceList.Count, "Order count (source list)");
    }

    [Test]
    [Description("Tests replacing a list with an empty one.")]
    public void TestGetOneToManyReferenceListReplaceWithEmptyList()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order();

      person.Orders = new Order[] { orderOne, orderTwo };
      person.Orders = new List<Order>();

      Assert.IsNull(orderOne.Owner, "Owner after replacement (order 1)");
      Assert.IsNull(orderTwo.Owner, "Owner after replacement (order 2)");
      Assert.AreEqual(0, person.Orders.Count, "Order count");
      Assert.AreEqual(0, person.SourceList.Count, "Order count (source list)");
    }

    [Test]
    [Description("Tests replacing a list with an empty one and then immediately trying to remove an item.")]
    public void TestGetOneToManyReferenceListReplaceWithEmptyListThenRemove()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order();

      person.Orders = new Order[] { orderOne, orderTwo };
      person.Orders = new List<Order>();

      Assert.IsFalse(person.Orders.Remove(orderOne), "Return value of 'Remove' method call.");
    }

    [Test]
    [Description("Tests replacing a list with an empty one and then immediately trying to remove an item.")]
    public void TestGetOneToManyReferenceListReplaceWithEmptyListThenAddToDifferentCollection()
    {
      Person
        person = new Person() { Id = 1 },
        personTwo = new Person() { Id = 2 };

      Order
        orderOne = new Order(),
        orderTwo = new Order();

      person.Orders = new Order[] { orderOne, orderTwo };
      person.Orders = new List<Order>();

      personTwo.Orders.Add(orderOne);
      Assert.AreSame(personTwo, orderOne.Owner, "Owner (order 1)");
    }

    [Test]
    public void TestGetOneToManyReferenceListCheckingSourceList()
    {
      Person person = new Person() { Id = 1 };
      Order order = new Order() { Id = 2 };

      person.Orders = new Order[] { order };

      Assert.AreEqual(1, person.SourceList.Count, "Count of orders in source list.");
      Assert.IsNotNull(order.Owner, "Order owner nullability");
      Assert.AreSame(person, person.SourceList[0].Owner, "Order owner in source list item");
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestGetOneToManyReferenceListReplaceWithNull()
    {
      Person person = new Person() { Id = 1 };
      person.Orders = null;
    }

    [Test]
    [Description("This test highlights an issue whereby, if the event-bound list is initialised and then " +
                 "subsequently the source list is replaced, the list exposed by the property becomes out of sync " +
                 "with the wrapped list/backing store.")]
    public void TestGetOneToManyReferenceListReplaceSourceList()
    {
      Person person = new Person();

      Order order = new Order();

      // Touch the property in order to initialise the collection.
      IList<Order> propertyList = person.Orders;

      Assert.IsNotNull(propertyList, "Event bound orders nullability (pre-switch)");
      Assert.AreEqual(0, propertyList.Count, "Event bound orders count (pre-switch)");

      // Switch the source list behind the scenes!  The event-bound list doesn't know that anything's happened though!
      person.SourceList = new List<Order>(new Order[] { order });

      // Get the list from the property again.
      propertyList = person.Orders;

      Assert.IsNotNull(propertyList, "Event bound orders nullability (post-switch)");
      Assert.AreEqual(1, propertyList.Count, "Event bound orders count (post-switch)");
      Assert.IsTrue(propertyList.Contains(order), "Event bound orders expected contained item (post-switch)");
    }

    #endregion

    #region testing event-bound reference lists via old API

    [Test]
    [Description("Tests using the 'Add' method to add items to an existing list.")]
    public void TestGetOneToManyReferenceListAddOldApi()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order(),
        orderThree = new Order();

      person.OrdersViaObsoleteApi.Add(orderOne);
      person.OrdersViaObsoleteApi.Add(orderTwo);

      Assert.AreEqual(2, person.OrdersViaObsoleteApi.Count, "Order count");
      Assert.AreEqual(2, person.SourceList.Count, "Order count (source list)");
      Assert.AreSame(person, orderOne.Owner, "Owner (order 1)");
      Assert.AreSame(person, orderTwo.Owner, "Owner (order 2)");
      Assert.IsNull(orderThree.Owner, "Owner (order 3)");
    }

    [Test]
    [Description("Tests using the 'Remove' method to remove items from an existing list.")]
    public void TestGetOneToManyReferenceListRemoveOldApi()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order();

      person.OrdersViaObsoleteApi = new List<Order>(new Order[] { orderOne, orderTwo });

      Assert.IsTrue(person.OrdersViaObsoleteApi.Remove(orderTwo), "Return value of 'Remove' method");
      Assert.IsNull(orderTwo.Owner, "Owner after removal (order 2)");
      Assert.AreEqual(1, person.OrdersViaObsoleteApi.Count, "Order count");
      Assert.AreEqual(1, person.SourceList.Count, "Order count (source list)");
    }

    [Test]
    [Description("Tests replacing a list with a new one.")]
    public void TestGetOneToManyReferenceListReplaceListOldApi()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order(),
        orderThree = new Order();

      person.OrdersViaObsoleteApi = new Order[] { orderOne, orderTwo };
      person.OrdersViaObsoleteApi = new Order[] { orderThree };

      Assert.IsNull(orderOne.Owner, "Owner after replacement (order 1)");
      Assert.IsNull(orderTwo.Owner, "Owner after replacement (order 2)");
      Assert.AreSame(person, orderThree.Owner, "Owner after replacement (order 3)");
      Assert.AreEqual(1, person.OrdersViaObsoleteApi.Count, "Order count");
      Assert.AreEqual(1, person.SourceList.Count, "Order count (source list)");
    }

    [Test]
    [Description("Tests replacing a list with an empty one.")]
    public void TestGetOneToManyReferenceListReplaceWithEmptyListOldApi()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order();

      person.OrdersViaObsoleteApi = new Order[] { orderOne, orderTwo };
      person.OrdersViaObsoleteApi = new List<Order>();

      Assert.IsNull(orderOne.Owner, "Owner after replacement (order 1)");
      Assert.IsNull(orderTwo.Owner, "Owner after replacement (order 2)");
      Assert.AreEqual(0, person.OrdersViaObsoleteApi.Count, "Order count");
      Assert.AreEqual(0, person.SourceList.Count, "Order count (source list)");
    }

    [Test]
    [Description("Tests replacing a list with an empty one and then immediately trying to remove an item.")]
    public void TestGetOneToManyReferenceListReplaceWithEmptyListThenRemoveOldApi()
    {
      Person person = new Person() { Id = 1 };

      Order
        orderOne = new Order(),
        orderTwo = new Order();

      person.OrdersViaObsoleteApi = new Order[] { orderOne, orderTwo };
      person.OrdersViaObsoleteApi = new List<Order>();

      Assert.IsFalse(person.OrdersViaObsoleteApi.Remove(orderOne), "Return value of 'Remove' method call.");
    }

    [Test]
    [Description("Tests replacing a list with an empty one and then immediately trying to remove an item.")]
    public void TestGetOneToManyReferenceListReplaceWithEmptyListThenAddToDifferentCollectionOldApi()
    {
      Person
        person = new Person() { Id = 1 },
        personTwo = new Person() { Id = 2 };

      Order
        orderOne = new Order(),
        orderTwo = new Order();

      person.OrdersViaObsoleteApi = new Order[] { orderOne, orderTwo };
      person.OrdersViaObsoleteApi = new List<Order>();

      personTwo.OrdersViaObsoleteApi.Add(orderOne);
      Assert.AreSame(personTwo, orderOne.Owner, "Owner (order 1)");
    }

    [Test]
    public void TestGetOneToManyReferenceListCheckingSourceListOldApi()
    {
      Person person = new Person() { Id = 1 };
      Order order = new Order() { Id = 2 };

      person.OrdersViaObsoleteApi = new Order[] { order };

      Assert.AreEqual(1, person.SourceList.Count, "Count of orders in source list.");
      Assert.IsNotNull(order.Owner, "Order owner nullability");
      Assert.AreSame(person, person.SourceList[0].Owner, "Order owner in source list item");
    }

    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestGetOneToManyReferenceListReplaceWithNullOldApi()
    {
      Person person = new Person() { Id = 1 };
      person.OrdersViaObsoleteApi = null;
    }

    [Test]
    [Description("This test highlights an issue whereby, if the event-bound list is initialised and then " +
                 "subsequently the source list is replaced, the list exposed by the property becomes out of sync " +
                 "with the wrapped list/backing store.")]
    public void TestGetOneToManyReferenceListReplaceSourceListOldApi()
    {
      Person person = new Person();

      Order order = new Order();

      // Touch the property in order to initialise the collection.
      IList<Order> propertyList = person.OrdersViaObsoleteApi;

      Assert.IsNotNull(propertyList, "Event bound orders nullability (pre-switch)");
      Assert.AreEqual(0, propertyList.Count, "Event bound orders count (pre-switch)");

      // Switch the source list behind the scenes!  The event-bound list doesn't know that anything's happened though!
      person.SourceList = new List<Order>(new Order[] { order });

      // Get the list from the property again.
      propertyList = person.OrdersViaObsoleteApi;

      Assert.IsNotNull(propertyList, "Event bound orders nullability (post-switch)");
      Assert.AreEqual(1, propertyList.Count, "Event bound orders count (post-switch)");
      Assert.IsTrue(propertyList.Contains(order), "Event bound orders expected contained item (post-switch)");
    }

    #endregion
    
    #region contained mocks
    
    public class Person : Entity<Person,uint>
    {
      private IList<Order> _orders, _wrappedOrders;

      public virtual IList<Order> Orders
      {
        get {
          return this.GetOrInitReferenceList(ref _wrappedOrders, ref _orders, x => x.Owner);
        }
        set {
          this.ReplaceReferenceList(ref _wrappedOrders, value, x => x.Owner);
          _orders = value;
        }
      }

      public virtual IList<Order> OrdersViaObsoleteApi
      {
        get {
#pragma warning disable 618
          return this.GetOneToManyReferenceList(ref _wrappedOrders, ref _orders, x => x.Owner);
#pragma warning restore 618
        }
        set {
#pragma warning disable 618
          _wrappedOrders = this.ReplaceOneToManyReferenceList(_wrappedOrders, value, x => x.Owner);
#pragma warning restore 618
          _orders = value;
        }
      }

      public virtual IList<Order> SourceList
      {
        get {
          return _orders;
        }
        set {
          _orders = value;
        }
      }
    }
    
    public class Order : Entity<Order,uint>
    {
      public virtual Person Owner
      {
        get;
        set;
      }
    }
    
    public class Product : Entity<Product,uint>
    {}
    
    #endregion
  }
}

