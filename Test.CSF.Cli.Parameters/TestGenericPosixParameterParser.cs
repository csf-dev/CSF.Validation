//
// TestGenericPosixParameterParser.cs
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
using System.Collections.Generic;
using CSF.Cli.Parameters;

namespace Test.CSF.Cli
{
  [TestFixture]
  public class TestGenericPosixParameterParser
  {
    #region tests

    [Test]
    public void TestParse()
    {
      // Arrange
      var parser = new ParameterParserBuilder<SampleClass>()
        .AddFlag(x => x.Foo, shortName: "f", longName: "foo")
        .AddFlag(x => x.Bar, longName: "bar")
        .AddValue(x => x.OptionalValue, longName: "optional")
        .AddValue(x => x.MandatoryValue, shortNames: new[] { "m", "A" }, optional: false)
        .AddValue(x => x.SecondMandatory, longNames: new[] { "second", "mandatory" }, optional: false)
        .RemainingArguments(x => x.RemainingArgs)
        .Build();

      var cliParams = "--optional ValueOne -f --mandatory ValueTwo ArgOne ArgTwo".Split(' ');

      // Act
      var result = parser.Parse(cliParams);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.IsTrue(result.Foo, "Parameter Foo present");
      Assert.IsFalse(result.Bar, "Parameter Bar present");
      Assert.NotNull(result.OptionalValue, "Parameter OptionalValue present");
      Assert.IsNull(result.MandatoryValue, "Parameter MandatoryValue present");
      Assert.NotNull(result.SecondMandatory, "Parameter SecondMandatory present");

      Assert.AreEqual("ValueOne", result.OptionalValue, "OptionalValue value");
      Assert.AreEqual("ValueTwo", result.SecondMandatory, "SecondMandatory value");

      Assert.AreEqual(new[] { "ArgOne", "ArgTwo" }, result.RemainingArgs, "Remaining args");
    }

    [Test]
    public void TestParseEmptyOptionalValue()
    {
      // Arrange
      var parser = new ParameterParserBuilder<SampleClass>()
        .AddFlag(x => x.Foo, shortName: "f", longName: "foo")
        .AddFlag(x => x.Bar, longName: "bar")
        .AddValue(x => x.OptionalValue, longName: "optional")
        .AddValue(x => x.MandatoryValue, shortNames: new[] { "m", "A" }, optional: false)
        .AddValue(x => x.SecondMandatory, longNames: new[] { "second", "mandatory" }, optional: false)
        .RemainingArguments(x => x.RemainingArgs)
        .Build();

      var cliParams = "--optional -f --mandatory ValueTwo ArgOne ArgTwo".Split(' ');

      // Act
      var result = parser.Parse(cliParams);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.IsTrue(result.Foo, "Parameter Foo present");
      Assert.IsFalse(result.Bar, "Parameter Bar present");
      Assert.NotNull(result.OptionalValue, "Parameter OptionalValue present");
      Assert.IsNull(result.MandatoryValue, "Parameter MandatoryValue present");
      Assert.NotNull(result.SecondMandatory, "Parameter SecondMandatory present");

      Assert.AreEqual(String.Empty, result.OptionalValue, "OptionalValue value");
      Assert.AreEqual("ValueTwo", result.SecondMandatory, "SecondMandatory value");

      Assert.AreEqual(new[] { "ArgOne", "ArgTwo" }, result.RemainingArgs, "Remaining args");
    }

    #endregion

    #region contained type

    public class SampleClass
    {
      public bool Foo
      {
        get;
        set;
      }

      public bool Bar
      {
        get;
        set;
      }

      public string OptionalValue
      {
        get;
        set;
      }

      public string MandatoryValue
      {
        get;
        set;
      }

      public string SecondMandatory
      {
        get;
        set;
      }

      public IList<string> RemainingArgs
      {
        get;
        set;
      }
    }

    #endregion
  }
}

