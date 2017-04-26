//
// ValueRuleTests.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2017 Craig Fowler
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
using CSF.Validation.Rules;
using NUnit.Framework;

namespace CSF.Validation.Tests.Rules
{
  [TestFixture]
  public class ValueRuleTests
  {
    [Test]
    public void Single_Generic_GetResult_returns_expected_error_result_when_value_type_cannot_be_converted()
    {
      // Arrange
      IValueRule rule = new StubGenericRule();
      var validated = new StubValidatedObject();

      rule.Accessor = x => ((StubValidatedObject) x).StringProperty;

      // Act
      var result = rule.GetResult(validated);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.IsInstanceOf<ExceptionResult>(result, "Result type");
      var exceptionResult = (ExceptionResult) result;
      Assert.NotNull(exceptionResult.Exception, "Exception nullability");
      Assert.IsInstanceOf<TypeConversionException>(exceptionResult.Exception, "Exception type");
    }

    [Test]
    public void Double_Generic_GetResult_returns_expected_error_result_when_value_type_cannot_be_converted()
    {
      // Arrange
      IValueRule rule = new StubDoubleGenericRule();
      var validated = new StubValidatedObject();

      rule.Accessor = x => ((StubValidatedObject) x).StringProperty;

      // Act
      var result = rule.GetResult(validated);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.IsInstanceOf<ExceptionResult>(result, "Result type");
      var exceptionResult = (ExceptionResult) result;
      Assert.NotNull(exceptionResult.Exception, "Exception nullability");
      Assert.IsInstanceOf<TypeConversionException>(exceptionResult.Exception, "Exception type");
    }

    [Test]
    public void Double_Generic_GetResult_returns_expected_error_result_when_validated_type_cannot_be_converted()
    {
      // Arrange
      IValueRule rule = new StubDoubleGenericRule();
      var validated = new object();

      rule.Accessor = x => ((StubValidatedObject) x).StringProperty;

      // Act
      var result = rule.GetResult(validated);

      // Assert
      Assert.IsInstanceOf<ExceptionResult>(result, "Result type");
      var exceptionResult = (ExceptionResult) result;
      Assert.IsInstanceOf<TypeConversionException>(exceptionResult.Exception, "Exception type");
    }

    class StubGenericRule : ValueRule<int>
    {
      protected override RuleOutcome GetValueOutcome(int value)
      {
        return Success;
      }
    }

    class StubDoubleGenericRule : ValueRule<StubValidatedObject, int>
    {
      protected override RuleOutcome GetOutcome(StubValidatedObject validated, int value)
      {
        return Success;
      }
    }
  }
}
