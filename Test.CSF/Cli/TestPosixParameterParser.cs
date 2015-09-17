//
// TestPosixParameterParser.cs
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
using CSF.Cli;

namespace Test.CSF.Cli
{
  [TestFixture]
  public class TestPosixParameterParser
  {
    #region tests

    [Test]
    public void TestParse()
    {
      // Arrange
      var parser = new ParameterParserBuilder()
        .AddFlag("Foo", shortName: "f", longName: "foo")
        .AddFlag("Bar", longName: "bar")
        .AddValue("OptionalValue", longName: "optional")
        .AddValue("MandatoryValue", shortNames: new[] { "m", "A" }, optional: false)
        .AddValue("SecondMandatory", longNames: new[] { "second", "mandatory" }, optional: false)
        .Build();

      var cliParams = "--optional ValueOne -f --mandatory ValueTwo ArgOne ArgTwo".Split(' ');

      // Act
      var result = parser.Parse(cliParams);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.IsTrue(result.HasParameter("Foo"), "Parameter Foo present");
      Assert.IsFalse(result.HasParameter("Bar"), "Parameter Bar present");
      Assert.IsTrue(result.HasParameter("OptionalValue"), "Parameter OptionalValue present");
      Assert.IsFalse(result.HasParameter("MandatoryValue"), "Parameter MandatoryValue present");
      Assert.IsTrue(result.HasParameter("SecondMandatory"), "Parameter SecondMandatory present");

      Assert.AreEqual("ValueOne", result.GetParameterValue("OptionalValue"), "OptionalValue value");
      Assert.AreEqual("ValueTwo", result.GetParameterValue("SecondMandatory"), "SecondMandatory value");

      Assert.AreEqual(new[] { "ArgOne", "ArgTwo" }, result.GetRemainingArguments(), "Remaining args");
    }

    #endregion
  }
}

