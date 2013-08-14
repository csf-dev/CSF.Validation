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
      var identity = entity.GetIdentity();
      Assert.AreEqual(String.Format("[{0}: {1}]", typeof(Person).FullName, 5),
                      identity.ToString(),
                      "Correct identity");
    }
    
    [Test]
    public void TestSetIdentity()
    {
      Person entity = new Person();
      
      entity.SetIdentity(5);
      var identity = entity.GetIdentity();
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
      
      Assert.IsFalse(three.Equals((object) threeProduct), "Non-matching types not equal");
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

    #region contained mocks
    
    public class Person : Entity<Person,uint> {}

    public class Product : Entity<Product,uint> {}
    
    #endregion
  }
}

