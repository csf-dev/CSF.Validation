//
// StringPropertyIntegrationTests.cs
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
using NUnit.Framework;

namespace CSF.Validation.Tests.Integration
{
  [TestFixture]
  public class StringPropertyIntegrationTests
  {
    [Test]
    public void Validate_with_null_value_returns_failure_result()
    {
      // Arrange
      var sut = GetValidator();
      var validated = new StubValidatedObject { StringProperty = null };

      // Act
      var result = sut.Validate(validated);

      // Assert
      Assert.IsFalse(result.IsSuccess);
    }

    [Test]
    public void Validate_with_null_value_returns_one_failure_and_one_skipped_rule()
    {
      // Arrange
      var sut = GetValidator();
      var validated = new StubValidatedObject { StringProperty = null };

      // Act
      var result = sut.Validate(validated);

      // Assert
      Assert.AreEqual(2, result.RuleResults.Count(), "Overall count of results");
      Assert.IsTrue(result.RuleResults.Any(x => x.RuleResult.Outcome == Rules.RuleOutcome.Failure),
                    "One failure result");
      Assert.IsTrue(result.RuleResults.Any(x => x.RuleResult.Outcome == Rules.RuleOutcome.SkippedDueToDependencyFailure),
                    "One skipped result");
    }

    IValidator GetValidator()
    {
      return new StringPropertyFluentManifestCreator().CreateValidator();
    }
  }
}
