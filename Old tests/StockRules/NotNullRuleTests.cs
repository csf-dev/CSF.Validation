//
// NotNullRuleTests.cs
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
using CSF.Validation.StockRules;
using NUnit.Framework;

namespace CSF.Validation.Tests.StockRules
{
  [TestFixture]
  public class NotNullRuleTests
  {
    [Test]
    public void GetResult_returns_success_when_validated_object_is_not_null()
    {
      // Arrange
      var sut = new NotNullRule();
      var validated = "I am not null";

      // Act
      var result = RuleRunner.Run(sut, validated);

      // Assert
      OutcomeAssert.IsSuccess(result);
    }

    [Test]
    public void GetResult_returns_failure_when_validated_object_is_null()
    {
      // Arrange
      var sut = new NotNullRule();
      object validated = null;

      // Act
      var result = RuleRunner.Run(sut, validated);

      // Assert
      OutcomeAssert.IsFailure(result);
    }

    [Test]
    public void GetResult_returns_failure_when_validated_object_is_null_nullable()
    {
      // Arrange
      var sut = new NotNullRule();
      DateTime? validated = null;

      // Act
      var result = RuleRunner.Run(sut, validated);

      // Assert
      OutcomeAssert.IsFailure(result);
    }
  }
}
