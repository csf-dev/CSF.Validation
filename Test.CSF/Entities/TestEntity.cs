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
    public void TestOperatorEqualsCompareWithSelf()
    {
      // Arrange
      Mock<Person> mockPerson = new Mock<Person>() { CallBase = true };

      Person
        person1 = mockPerson.Object,
        person2 = mockPerson.Object;

      mockPerson.Setup(x => x.Equals(It.IsAny<Person>())).Returns(true);

      // Act and assert
      Assert.IsTrue(person1 == person2, "Correct result");
      mockPerson.Verify(x => x.Equals(It.IsAny<Person>()), Times.Never());
    }

    [Test]
    public void TestOperatorEqualsCompareWithNull()
    {
      // Arrange
      Mock<Person> mockPerson = new Mock<Person>() { CallBase = true };

      Person
        person1 = mockPerson.Object,
        person2 = null;

      mockPerson.Setup(x => x.Equals(It.IsAny<Person>())).Returns(false);

      // Act and assert
      Assert.IsFalse(person1 == person2, "Correct result");
      mockPerson.Verify(x => x.Equals(It.IsAny<Person>()), Times.Never());
    }

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

    [Test]
    public void TestOperatorEqualsIdsSame()
    {
      // Arrange
      Mock<Person>
        mockPerson1 = new Mock<Person>() { CallBase = true },
        mockPerson2 = new Mock<Person>() { CallBase = true };

      Person
        person1 = mockPerson1.Object,
        person2 = mockPerson2.Object;

      mockPerson1.Setup(x => x.Equals(It.IsAny<Person>())).Returns(true);
      mockPerson1.SetupGet(x => x.Identity).Returns(4);
      mockPerson1.SetupGet(x => x.HasIdentity).Returns(true);
      mockPerson1.Setup(x => x.GetRawIdentity()).Returns(new Identity<uint,Person>(4));
      mockPerson2.Setup(x => x.Equals(It.IsAny<Person>())).Returns(true);
      mockPerson2.SetupGet(x => x.Identity).Returns(4);
      mockPerson2.SetupGet(x => x.HasIdentity).Returns(true);
      mockPerson2.Setup(x => x.GetRawIdentity()).Returns(new Identity<uint,Person>(4));

      // Act and assert
      Assert.IsTrue(person1 == person2, "Correct result");
      mockPerson1.Verify(x => x.Equals(It.IsAny<Person>()), Times.Never());
      mockPerson2.Verify(x => x.Equals(It.IsAny<Person>()), Times.Never());
    }

    [Test]
    public void TestOperatorEqualsDownCast()
    {
      // Arrange
      Mock<Person>
        mockPerson1 = new Mock<Person>() { CallBase = true },
        mockPerson2 = new Mock<Person>() { CallBase = true };

      Person person1 = mockPerson1.Object;
      IEntity person2 = mockPerson2.Object;

      mockPerson1.Setup(x => x.Equals(It.IsAny<IEntity>())).Returns(true);
      mockPerson1.SetupGet(x => x.Identity).Returns(4);
      mockPerson1.SetupGet(x => x.HasIdentity).Returns(true);
      mockPerson1.Setup(x => x.GetRawIdentity()).Returns(new Identity<uint,Person>(4));
      mockPerson2.Setup(x => x.Equals(It.IsAny<Person>())).Returns(true);
      mockPerson2.SetupGet(x => x.Identity).Returns(4);
      mockPerson2.SetupGet(x => x.HasIdentity).Returns(true);
      mockPerson2.Setup(x => x.GetRawIdentity()).Returns(new Identity<uint,Person>(4));

      // Act and assert
      Assert.IsTrue(person1 == person2, "Correct result");
      mockPerson1.Verify(x => x.Equals(It.IsAny<IEntity>()), Times.Never());
      mockPerson2.Verify(x => x.Equals(It.IsAny<Person>()), Times.Never());
    }

    #endregion

    #region contained mocks
    
    public class Person : Entity<uint> {}

    public class Product : Entity<uint> {}
    
    #endregion
  }
}

