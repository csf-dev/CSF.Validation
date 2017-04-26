﻿//
// IsDefinedEnumMemberValueRuleTests.cs
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
using CSF.Validation.StockRules;
using NUnit.Framework;

namespace CSF.Validation.Tests.StockRules
{
  [TestFixture]
  public class IsDefinedEnumMemberValueRuleTests
  {
    [Test]
    public void GetResult_returns_success_when_value_is_defined()
    {
      // Arrange
      var sut = CreateSut();
      sut.Accessor = CreateAccessor(x => x.EnumProperty);
      var validated = new StubValidatedObject { EnumProperty = StubEnum.One };

      // Act
      var result = RuleRunner.Run(sut, validated);

      // Assert
      OutcomeAssert.IsSuccess(result);
    }

    [Test]
    public void GetResult_returns_failure_when_value_not_defined()
    {
      // Arrange
      var sut = CreateSut();
      sut.Accessor = CreateAccessor(x => x.EnumProperty);
      var validated = new StubValidatedObject() { EnumProperty = (StubEnum) 88 };

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
      sut.Accessor = CreateAccessor(x => x.EnumProperty);
      StubValidatedObject validated = null;

      // Act
      var result = RuleRunner.Run(sut, validated);

      // Assert
      OutcomeAssert.IsError(result);
    }

    Func<object,StubEnum> CreateAccessor(Func<StubValidatedObject,StubEnum> accessor)
    {
      return x => accessor((StubValidatedObject) x);
    }

    IsDefinedEnumMemberValueRule<StubEnum> CreateSut()
    {
      return new IsDefinedEnumMemberValueRule<StubEnum>();
    }
  }
}
