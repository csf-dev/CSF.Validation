//  
//  TestIdentity.cs
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

namespace Test.CSF.Entities
{
  [TestFixture]
  public class TestIdentity
  {
    #region tests
    
    [Test]
    public void TestEquals()
    {
      string stringTest = "foo bar";
      uint numericTest = 3; 
      Identity<Person,uint> three = new Identity<Person,uint>(3);
      Identity<Person,uint> four = new Identity<Person,uint>(4);
      Identity<Person,uint> threeAgain = new Identity<Person,uint>(3);
      Identity<Product,uint> threeProduct = new Identity<Product,uint>(3);
      
      Assert.IsFalse(three.Equals(stringTest), "Identity does not equal a string");
      Assert.IsFalse(three.Equals(numericTest), "Identity does not equal a uint");
      Assert.IsFalse(three.Equals(four), "Non-matching identities not equal");
      
      Assert.IsTrue(three.Equals(three), "Copies of the same object are equal");
      Assert.IsTrue(three.Equals(threeAgain), "Identical instances are equal");
      
#pragma warning disable 618
      Assert.IsFalse(three.Equals(threeProduct), "Non-matching types not equal");
#pragma warning restore 618
    }
    
    [Test]
    public void TestToString()
    {
      Identity<Person,uint> three = new Identity<Person,uint>(3);
      Assert.AreEqual(String.Format("[{0}: {1}]", typeof(Person).FullName, 3),
                      three.ToString(),
                      "Correct string representation");
    }
    
    [Test]
    public void TestOperatorEquality()
    {
      Identity<Person,uint> three = new Identity<Person,uint>(3);
      Identity<Person,uint> four = new Identity<Person,uint>(4);
      Identity<Person,uint> threeAgain = new Identity<Person,uint>(3);
      Identity<Product,uint> threeProduct = new Identity<Product,uint>(3);
      
      Assert.IsFalse(three == four, "Non-matching identities not equal");
#pragma warning disable 1718
      // Disabling CS1718 - the point of this test is to compare the object to itself
      Assert.IsTrue(three == three, "Copies of the same object are equal");
#pragma warning restore 1718
      Assert.IsTrue(three == threeAgain, "Identical instances are equal");

#pragma warning disable 618
      Assert.IsFalse(three == threeProduct, "Non-matching types not equal");
#pragma warning restore 618
    }
    
    [Test]
    public void TestOperatorInequality()
    {
      Identity<Person,uint> three = new Identity<Person,uint>(3);
      Identity<Person,uint> four = new Identity<Person,uint>(4);
      Identity<Person,uint> threeAgain = new Identity<Person,uint>(3);
      Identity<Product,uint> threeProduct = new Identity<Product,uint>(3);

      Assert.IsTrue(three != four, "Non-matching identities not equal");
#pragma warning disable 1718
      // Disabling CS1718 - the point of this test is to compare the object to itself
      Assert.IsFalse(three != three, "Copies of the same object are equal");
#pragma warning restore 1718
      Assert.IsFalse(three != threeAgain, "Identical instances are equal");

#pragma warning disable 618
      Assert.IsTrue(three != threeProduct, "Non-matching types not equal");
#pragma warning restore 618
    }

    [Test]
    [Description("This test is only here to test the preservation of backwards-compatibility.")]
    public void TestTryParse()
    {
      Identity<Person,uint> output;
#pragma warning disable 618
      bool result = Identity.TryParse("57", out output);
#pragma warning restore 618

      Assert.IsTrue(result);
      Assert.AreEqual(57, output.Value);
    }

    [Test]
    [Description("This test is only here to test the preservation of backwards-compatibility.")]
    public void TestTryParseInterface()
    {
      IIdentity<Person> output;
#pragma warning disable 618
      bool result = Identity.TryParse<Person,uint>("57", out output);
#pragma warning restore 618

      Assert.IsTrue(result);
      Assert.AreEqual(57, output.Value);
    }

    [Test]
    [Description("This test ensures that backwards compatibility is maintained, per #39")]
    public void TestTryParseObsolete()
    {
#pragma warning disable 618
      IIdentity<Person,uint> output;
      bool result = Identity.TryParse("57", out output);
#pragma warning restore 618

      Assert.IsTrue(result);
      Assert.AreEqual(57, output.Value);
    }

    [Test]
    [Description("This test ensures that backwards compatibility is maintained, per #39")]
    public void TestParseBackwardsCompatible()
    {
#pragma warning disable 618
      IIdentity<Person,uint> output = Identity.Parse<Person,uint>("57");
#pragma warning restore 618

      Assert.AreEqual(57, output.Value);
    }

    #endregion
    
    #region mocks
    
    public class Person : Entity<Person,uint> {}
    
    public class Employee : Person {}
    
    public class Product : Entity<Product,uint> {}
    
    #endregion
  }
}

