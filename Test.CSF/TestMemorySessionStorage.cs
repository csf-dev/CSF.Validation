//
// TestMemorySessionStorage.cs
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
using CSF;

namespace Test.CSF
{
  [TestFixture]
  [Category("Requires configuration")]
  [Description("The tests in this fixture depend upon a working session storage configuration.")]
  public class TestMemorySessionStorage
  {
    #region fields
    
    private ISessionStorage storage;
    
    #endregion
    
    #region setup and teardown
    
    [TestFixtureSetUp]
    public void FixtureSetUp()
    {
      storage = new MemorySessionStorage();
    }
    
    [SetUp]
    public void Setup()
    {
      storage.Abandon();
    }
    
    #endregion
    
    #region tests
    
    [Test]
    public void TestStoreValue()
    {
      storage.Store<DateTime>("test", DateTime.Today);
      Assert.AreEqual(DateTime.Today, storage.Get<DateTime>("test"), "Test value is as expected");
    }
    
    [Test]
    public void TestClearValue()
    {
      DateTime discarded;
      storage.Store<DateTime>("test", DateTime.Today);
      Assert.AreEqual(DateTime.Today, storage.Get<DateTime>("test"), "Test value is as expected");
      
      storage.Remove("test");
      Assert.IsFalse(storage.TryGet<DateTime>("test", out discarded), "Data has been removed");
    }
    
    [Test]
    public void TestAbandon()
    {
      DateTime discarded;
      storage.Store<DateTime>("test", DateTime.Today);
      Assert.AreEqual(DateTime.Today, storage.Get<DateTime>("test"), "Test value is as expected");
      
      storage.Abandon();
      Assert.IsFalse(storage.TryGet<DateTime>("test", out discarded), "Data has been removed");
    }
    
    #endregion
  }
}

