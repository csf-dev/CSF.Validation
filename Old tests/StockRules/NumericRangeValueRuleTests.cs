//
// NullableNumericRangeValueRuleTests.cs
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
using NUnit.Framework;
using CSF.Validation.StockRules;

namespace CSF.Validation.Tests.StockRules
{
  [TestFixture]
  public class NumericRangeValueRuleTests
  {
    [Test]
    public void GetResult_returns_success_when_value_is_in_range()
    {
      // Arrange
      var sut = CreateSut();
      sut.Accessor = CreateAccessor(x => x.IntegerProperty);
      var validated = new StubValidatedObject { IntegerProperty = 6 };

      // Act
      var result = RuleRunner.Run(sut, validated);

      // Assert
      OutcomeAssert.IsSuccess(result);
    }

    [Test]
    public void GetResult_returns_failure_when_value_is_below_minimum()
    {
      // Arrange
      var sut = CreateSut();
      sut.Accessor = CreateAccessor(x => x.IntegerProperty);
      var validated = new StubValidatedObject { IntegerProperty = 4 };

      // Act
      var result = RuleRunner.Run(sut, validated);

      // Assert
      OutcomeAssert.IsFailure(result);
    }

    [Test]
    public void GetResult_returns_failure_when_value_is_above_maximum()
    {
      // Arrange
      var sut = CreateSut();
      sut.Accessor = CreateAccessor(x => x.IntegerProperty);
      var validated = new StubValidatedObject { IntegerProperty = 20 };

      // Act
      var result = RuleRunner.Run(sut, validated);

      // Assert
      OutcomeAssert.IsFailure(result);
    }

    [Test]
    public void GetResult_returns_error_when_parent_value_is_null()
    {
      // Arrange
      var sut = CreateSut();
      sut.Accessor = CreateAccessor(x => x.IntegerProperty);
      StubValidatedObject validated = null;

      // Act
      var result = RuleRunner.Run(sut, validated);

      // Assert
      OutcomeAssert.IsError(result);
    }

    Func<object,double> CreateAccessor(Func<StubValidatedObject,double> accessor)
    {
      return x => accessor((StubValidatedObject) x);
    }

    NumericRangeValueRule CreateSut()
    {
      return new NumericRangeValueRule() {
        Min = 5,
        Max = 10,
      };
    }
  }
}
