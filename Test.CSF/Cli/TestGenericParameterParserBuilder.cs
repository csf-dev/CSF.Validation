//
// TestGenericParameterParserBuilder.cs
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
using System.Linq;
using CSF.Reflection;

namespace Test.CSF.Cli
{
  [TestFixture]
  public class TestGenericParameterParserBuilder
  {
    #region tests

    [Test]
    public void TestAddFlag()
    {
      // Arrange
      var sut = new ParameterParserBuilder<SampleClass>();

      // Act
      sut.AddFlag(x => x.FlagProperty, shortName: "f", longName: "foo");
      var result = sut.Build();

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.GetRegisteredParameters().Length, "Count of parameters");
      var param = result.GetRegisteredParameters().First();
      Assert.AreEqual(Reflect.Property<SampleClass>(x => x.FlagProperty),
                      param.Identifier,
                      "Param identifier");
      Assert.AreEqual("f", param.ShortNames.First(), "Param short name");
      Assert.AreEqual("foo", param.LongNames.First(), "Param long name");
      Assert.AreEqual(ParameterBehaviour.Switch, param.Behaviour, "Param behaviour");
    }

    [Test]
    public void TestAddValue()
    {
      // Arrange
      var sut = new ParameterParserBuilder<SampleClass>();

      // Act
      sut.AddValue(x => x.ValueProperty, shortName: "f", longName: "foo", optional: false);
      var result = sut.Build();

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.GetRegisteredParameters().Length, "Count of parameters");
      var param = result.GetRegisteredParameters().First();
      Assert.AreEqual(Reflect.Property<SampleClass>(x => x.ValueProperty),
                      param.Identifier,
                      "Param identifier");
      Assert.AreEqual("f", param.ShortNames.First(), "Param short name");
      Assert.AreEqual("foo", param.LongNames.First(), "Param long name");
      Assert.AreEqual(ParameterBehaviour.ValueRequired, param.Behaviour, "Param behaviour");
    }

    [Test]
    public void TestAddOptionalValue()
    {
      // Arrange
      var sut = new ParameterParserBuilder<SampleClass>();

      // Act
      sut.AddValue(x => x.OptionalValueProperty, shortName: "f", longName: "foo", optional: true);
      var result = sut.Build();

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.GetRegisteredParameters().Length, "Count of parameters");
      var param = result.GetRegisteredParameters().First();
      Assert.AreEqual(Reflect.Property<SampleClass>(x => x.OptionalValueProperty),
                      param.Identifier,
                      "Param identifier");
      Assert.AreEqual("f", param.ShortNames.First(), "Param short name");
      Assert.AreEqual("foo", param.LongNames.First(), "Param long name");
      Assert.AreEqual(ParameterBehaviour.ValueOptional, param.Behaviour, "Param behaviour");
    }

    #endregion

    #region contained type

    public class SampleClass
    {
      public bool FlagProperty
      {
        get;
        set;
      }

      public string ValueProperty
      {
        get;
        set;
      }

      public string OptionalValueProperty
      {
        get;
        set;
      }
    }

    #endregion
  }
}

