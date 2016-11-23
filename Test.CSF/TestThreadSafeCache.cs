//
// TestThreadSafeCache.cs
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

using NUnit.Framework;
using System;
using CSF.Caches;

namespace Test.CSF
{
  [TestFixture]
  public class TestThreadSafeCache
  {
    #region constants

    [Datapoints]
    public readonly string[] Keys = new [] {
      "Foo",
      "Bar",
      "Baz",
    };

    [Datapoints]
    public readonly DateTime[] Values = new[] {
      DateTime.Today,
      DateTime.Today.AddDays(-3),
      DateTime.Today.AddDays(3),
    };

    #endregion

    #region fields

    private ThreadSafeCache<string,DateTime> _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _sut = new ThreadSafeCache<string, DateTime>();
    }

    #endregion

    #region tests

    [Theory]
    public void TestAdd(string key, DateTime value)
    {
      // Act
      var result = _sut.Add(key, value);

      // Assert
      Assert.IsTrue(result);
    }

    [Theory]
    public void TestAddTwice(string key, DateTime value)
    {
      // Act
      var resultOne = _sut.Add(key, value);
      var resultTwo = _sut.Add(key, value);

      // Assert
      Assert.IsTrue(resultOne, "Result one");
      Assert.IsFalse(resultTwo, "Result two");
    }

    [Theory]
    public void TestContains(string addKey, DateTime value, string testKey)
    {
      Assume.That(addKey == testKey);

      // Arrange
      _sut.Add(addKey, value);

      // Act
      var result = _sut.Contains(testKey);

      // Assert
      Assert.IsTrue(result);
    }

    [Theory]
    public void TestNotContains(string addKey, DateTime value, string testKey)
    {
      Assume.That(addKey != testKey);

      // Arrange
      _sut.Add(addKey, value);

      // Act
      var result = _sut.Contains(testKey);

      // Assert
      Assert.IsFalse(result);
    }

    [Theory]
    public void TestContainsAfterRemove(string addKey, DateTime value)
    {
      // Arrange
      _sut.Add(addKey, value);
      _sut.Remove(addKey);

      // Act
      var result = _sut.Contains(addKey);

      // Assert
      Assert.IsFalse(result);
    }

    [Theory]
    public void TestRemove(string addKey, DateTime value, string removeKey)
    {
      Assume.That(addKey == removeKey);

      // Arrange
      _sut.Add(addKey, value);

      // Act
      var result = _sut.Remove(removeKey);

      // Assert
      Assert.IsTrue(result);
    }

    [Theory]
    public void TestNotRemove(string addKey, DateTime value, string removeKey)
    {
      Assume.That(addKey != removeKey);

      // Arrange
      _sut.Add(addKey, value);

      // Act
      var result = _sut.Remove(removeKey);

      // Assert
      Assert.IsFalse(result);
    }

    [Theory]
    public void TestGet(string key, DateTime value)
    {
      // Arrange
      _sut.Add(key, value);

      // Act
      var result = _sut.Get(key);

      // Assert
      Assert.AreEqual(value, result);
    }

    [Theory]
    [ExpectedException(typeof(NotAvailableInCacheException))]
    public void TestGetException(string key, DateTime value, string badKey)
    {
      Assume.That(badKey != key);

      // Arrange
      _sut.Add(key, value);

      // Act
      _sut.Get(badKey);
    }

    [Theory]
    public void TestTryGet(string key, DateTime value)
    {
      // Arrange
      _sut.Add(key, value);
      DateTime output;

      // Act
      var result = _sut.TryGet(key, out output);

      // Assert
      Assert.IsTrue(result, "Correct result");
      Assert.AreEqual(value, output, "Correct output");
    }

    [Theory]
    public void TestTryGetFailure(string key, DateTime value, string badKey)
    {
      Assume.That(badKey != key);

      // Arrange
      _sut.Add(key, value);
      DateTime output;

      // Act
      var result = _sut.TryGet(badKey, out output);

      // Assert
      Assert.IsFalse(result);
    }

    [Theory]
    public void TestGetOrAddCacheHit(string key, DateTime value, string testKey, DateTime factoryValue)
    {
      Assume.That(key == testKey);
      Assume.That(factoryValue != value);

      // Arrange
      _sut.Add(key, value);
      Func<DateTime> factory = () => factoryValue;
      bool cacheHit;

      // Act
      var result = _sut.GetOrAdd(testKey, factory, out cacheHit);

      // Assert
      Assert.AreEqual(value, result, "Correct result");
      Assert.IsTrue(cacheHit);
    }

    [Theory]
    public void TestGetOrAddCacheMiss(string key, DateTime value, string testKey, DateTime factoryValue)
    {
      Assume.That(key != testKey);
      Assume.That(factoryValue != value);

      // Arrange
      _sut.Add(key, value);
      Func<DateTime> factory = () => factoryValue;
      bool cacheHit;

      // Act
      var result = _sut.GetOrAdd(testKey, factory, out cacheHit);

      // Assert
      Assert.AreEqual(factoryValue, result, "Correct result");
      Assert.IsFalse(cacheHit);
    }

    #endregion
  }
}

