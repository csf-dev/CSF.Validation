//
// TestStringExtensions.cs
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
  public class TestStringExtensions
  {
    #region tests

    [Test]
    public void TestParseAs()
    {
      SampleEnum value = "Foo".ParseAs<SampleEnum>();

      Assert.AreEqual(SampleEnum.Foo, value);
    }

    [Test]
    public void TestParseAsCaseInsensitive()
    {
      SampleEnum value = "FOO".ParseAs<SampleEnum>(true);

      Assert.AreEqual(SampleEnum.Foo, value);
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void TestParseAsCaseInsensitiveFailure()
    {
      "FOO".ParseAs<SampleEnum>();
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void TestParseAsFailure()
    {
      "WONG!".ParseAs<SampleEnum>();
    }

    [Test]
    public void TestTryParseAs()
    {
      SampleEnum? value = "Foo".TryParseAs<SampleEnum>();

      Assert.IsTrue(value.HasValue, "Has value");
      Assert.AreEqual(SampleEnum.Foo, value.Value);
    }

    [Test]
    public void TestTryParseAsCaseInsensitive()
    {
      SampleEnum? value = "FOO".TryParseAs<SampleEnum>(true);

      Assert.IsTrue(value.HasValue, "Has value");
      Assert.AreEqual(SampleEnum.Foo, value.Value);
    }

    [Test]
    public void TestTryParseAsCaseInsensitiveFailure()
    {
      SampleEnum? value = "FOO".TryParseAs<SampleEnum>();
      Assert.IsFalse(value.HasValue, "Has value");
    }

    [Test]
    public void TestTryParseAsFailure()
    {
      SampleEnum? value = "WONG!".TryParseAs<SampleEnum>();
      Assert.IsFalse(value.HasValue, "Has value");
    }

    [Test]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestParseAsInvalidType()
    {
      "Foo".ParseAs<int>();
    }

    [Test]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestTryParseAsInvalidType()
    {
      "Foo".TryParseAs<int>();
    }

    [Test]
    public void TestCapitalize()
    {
      string test1 = "Foo bar BAZ", expected1 = "Foo Bar Baz";
      string test2 = "FLOUNCE", expected2 = "Flounce";
      string test3 = String.Empty, expected3 = String.Empty;

      Assert.AreEqual(expected1, test1.Capitalize(), "Test 1");
      Assert.AreEqual(expected2, test2.Capitalize(), "Test 2");
      Assert.AreEqual(expected3, test3.Capitalize(), "Test 3");
    }

    #endregion

    #region enumeration

    private enum SampleEnum
    {
      Foo,
      Bar,
      Baz
    }

    #endregion
  }
}

