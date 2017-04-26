//
// StringAndDecimalPropertyIntegrationTests.cs
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
using System.Linq;
using CSF.Validation.Rules;
using NUnit.Framework;

namespace CSF.Validation.Tests.Integration
{
  public class StringAndDecimalPropertyIntegrationTests : IntegrationTestBase<StringAndDecimalPropertyValidatorCreator>
  {
    [Test]
    public void Validate_with_valid_values_returns_success_result()
    {
      // Arrange
      var sut = GetValidator();
      var validated = new StubValidatedObject { StringProperty = "abcdef", NullableDecimalProperty = 15 };

      // Act
      var result = sut.Validate(validated);

      // Assert
      Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public void Validate_with_valid_string_but_invalid_number_returns_two_successes_and_one_failure()
    {
      // Arrange
      var sut = GetValidator();
      var validated = new StubValidatedObject { StringProperty = "abcdef", NullableDecimalProperty = 55 };

      // Act
      var result = sut.Validate(validated);

      // Assert
      Assert.AreEqual(3,
                      result.RuleResults.Count(),
                      "Overall result count");
      Assert.AreEqual(2,
                      result.RuleResults.Count(x => x.RuleResult.Outcome == RuleOutcome.Success),
                      "Count of successes");
      Assert.AreEqual(1,
                      result.RuleResults.Count(x => x.RuleResult.Outcome == RuleOutcome.Failure),
                      "Count of failures");
    }
  }
}
