//
// TestEntity.cs
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
using CSF.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using Moq;

namespace Test.CSF.Entities
{
  [TestFixture]
  public class TestEntity
  {
    #region general tests
    
    [Test]
    public void TestHasIdentity()
    {
      Entity<uint> entity = new Entity<uint>();
      
      Assert.IsFalse(entity.HasIdentity, "Entity has no identity");
      
      entity.Identity = 5;
      Assert.IsTrue(entity.HasIdentity, "Entity has an identity");
    }
    
    [Test]
    public void TestGetIdentity()
    {
      Person entity = new Person() { Identity = 5 };
      var identity = entity.GetIdentity();

      Assert.AreEqual(String.Format("[{0}#{1}]", typeof(Person).FullName, 5),
                      identity.ToString(),
                      "Correct identity");
    }

    [TestCase(0u, 0u, false)]
    [TestCase(1u, 0u, false)]
    [TestCase(0u, 1u, false)]
    [TestCase(1u, 2u, false)]
    [TestCase(1u, 1u, true)]
    public void TestEquals(uint identityOne, uint identityTwo, bool expectEqual)
    {
      // Arrange
      Person
        one = new Person() { Identity = identityOne },
        two = new Person() { Identity = identityTwo };

      // Act and assert
      Assert.AreEqual(expectEqual, one.Equals(two));
    }

    [Test]
    public void TestEqualsWrongTypeString()
    {
      // Arrange
      Person sut = new Person() { Identity = 1 };

      // Act and assert
      Assert.IsFalse(sut.Equals("Foo"));
    }

    [Test]
    public void TestEqualsWrongTypeNumber()
    {
      // Arrange
      Person sut = new Person() { Identity = 1 };

      // Act and assert
      Assert.IsFalse(sut.Equals((uint) 1));
    }

    [Test]
    public void TestEqualsWrongTypeDifferentEntity()
    {
      // Arrange
      Person one = new Person() { Identity = 1 };
      Product two = new Product() { Identity = 2 };

      // Act and assert
      Assert.IsFalse(one.Equals(two));
    }

    [Test]
    public void TestEqualsReferenceEqual()
    {
      // Arrange
      Person
        one = new Person() { Identity = 0 },
        two = one;

      // Act and assert
      Assert.IsTrue(one.Equals(two));
    }
    
    [TestCase(0u, "[Test.CSF.Entities.TestEntity+Person#(no identity)]")]
    [TestCase(1u, "[Test.CSF.Entities.TestEntity+Person#1]")]
    [TestCase(555u, "[Test.CSF.Entities.TestEntity+Person#555]")]
    public void TestToString(uint identity, string expected)
    {
      // Arrange
      Person sut = new Person() { Identity = identity };

      // Act and assert
      Assert.AreEqual(expected, sut.ToString());
    }

    [TestCase(0u, 0u, false)]
    [TestCase(1u, 0u, false)]
    [TestCase(0u, 1u, false)]
    [TestCase(1u, 2u, false)]
    [TestCase(1u, 1u, true)]
    public void TestOpEquality(uint identityOne, uint identityTwo, bool expectEqual)
    {
      // Arrange
      Person
        one = new Person() { Identity = identityOne },
        two = new Person() { Identity = identityTwo };

      // Act and assert
      Assert.AreEqual(expectEqual, one == two);
    }

    [TestCase(0u, 0u, true)]
    [TestCase(1u, 0u, true)]
    [TestCase(0u, 1u, true)]
    [TestCase(1u, 2u, true)]
    [TestCase(1u, 1u, false)]
    public void TestOpInequality(uint identityOne, uint identityTwo, bool expectNotEqual)
    {
      // Arrange
      Person
      one = new Person() { Identity = identityOne },
      two = new Person() { Identity = identityTwo };

      // Act and assert
      Assert.AreEqual(expectNotEqual, one != two);
    }

    #endregion

    #region Operator interaction with Equals

    [Test]
    public void TestOperatorEqualsBothNull()
    {
      // Arrange
      Person
        person1 = null,
        person2 = null;

      // Act and assert
      Assert.IsTrue(person1 == person2, "Correct result");
    }

    #endregion

    #region contained mocks
    
    public class Person : Entity<uint> {}

    public class Product : Entity<uint> {}
    
    #endregion
  }
}

