//
// TestTypeExtensions.cs
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
using CSF.Reflection;
using System.Collections.Generic;

namespace Test.CSF.Reflection
{
  [TestFixture]
  public class TestTypeExtensions
  {
    #region tests
    
    [Test]
    public void TestGetSubclasses()
    {
      IList<Type> types = typeof(Foo).GetSubclasses();
      
      Assert.AreEqual(2, types.Count, "Correct count");
      
      Assert.IsTrue(types.Contains(typeof(Bar)), "Contains 'bar'");
      Assert.IsTrue(types.Contains(typeof(Baz)), "Contains 'baz'");
    }

    [Test]
    public void TestImplementsInterface()
    {
      Type testType = typeof(Bar);

      Assert.IsTrue(testType.ImplementsInterface<IMarker>(), "Implements IMarker");
      Assert.IsFalse(testType.ImplementsInterface<IDisposable>(), "Does not implement IDisposable");
    }

    [Test]
    public void TestImplementsInterfaceGeneric()
    {
      Type testType = typeof(Baz);

      Assert.IsTrue(testType.ImplementsInterface<IMarker<int>>(), "Implements IMarker<int>");
      Assert.IsFalse(testType.ImplementsInterface<IDisposable>(), "Does not implement IDisposable");
    }

    #endregion
    
    #region contained classes
    
    class Foo {}
    
    class Bar : Foo, IMarker {}
    
    class Baz : Bar, IMarker<int> {}

    interface IMarker {}

    interface IMarker<T> {}
    
    #endregion
  }
}

