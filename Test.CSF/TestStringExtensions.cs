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

