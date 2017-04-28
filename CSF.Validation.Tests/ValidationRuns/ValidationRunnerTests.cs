//
// ValidationRunnerTests.cs
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
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Options;
using CSF.Validation.ValidationRuns;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Tests.ValidationRuns
{
  [TestFixture]
  public class ValidationRunnerTests
  {
    [Test]
    public void ExecuteRunAndGetResults_executes_all_rules()
    {
      // Arrange
      var rules = Enumerable.Range(0, 5).Select(x => GetRunnableRule()).ToArray();
      var validated = new object();
      var validationRun = GetContext(rules, validated);
      var sut = new ValidationRunner();

      // Act
      sut.ExecuteRunAndGetResults(validationRun);

      // Assert
      foreach(var rule in rules)
      {
        Mock.Get(rule).Verify(x => x.Execute(validated), Times.Once());
      }
    }

    [Test]
    public void ExecuteRunAndGetResults_returns_results_for_all_rules()
    {
      // Arrange
      var rules = Enumerable.Range(0, 3).Select(x => {
        var result = Mock.Of<IRunnableRuleResult>();
        return new { Result = result, Rule = GetRunnableRule(result: result) };
      }).ToArray();
      var validated = new object();
      var validationRun = GetContext(rules.Select(r => r.Rule).ToArray(), validated);
      var sut = new ValidationRunner();

      // Act
      var actualResults = sut.ExecuteRunAndGetResults(validationRun);

      // Assert
      CollectionAssert.AreEquivalent(rules.Select(x => x.Result).ToArray(), actualResults);
    }

    [Test]
    public void ExecuteRunAndGetResults_throws_exception_for_stalled_validation_runs()
    {
      // Arrange
      var rules = new [] {
        GetRunnableRule(),
        Mock.Of<IRunnableRule>(x => x.MayBeExecuted == false),
      };
      var validated = new object();
      var validationRun = GetContext(rules, validated);
      var sut = new ValidationRunner();

      // Act & Assert
      Assert.Throws<StalledValidationRunException>(() => sut.ExecuteRunAndGetResults(validationRun));
    }

    [Test]
    public void ExecuteRunAndGetResults_executes_all_rules_in_a_dependency_chain()
    {
      // Arrange
      var ruleOne = GetRunnableRule();
      var ruleTwo = GetRunnableRule(dependencies: new [] { ruleOne });
      var ruleThree = GetRunnableRule(dependencies: new [] { ruleOne });
      var ruleFour = GetRunnableRule(dependencies: new [] { ruleTwo, ruleThree });
      var ruleFive = GetRunnableRule(dependencies: new [] { ruleOne, ruleFour });

      var rules = new [] { ruleOne, ruleTwo, ruleThree, ruleFour, ruleFive };

      var validated = new object();
      var validationRun = GetContext(rules, validated);
      var sut = new ValidationRunner();

      // Act
      sut.ExecuteRunAndGetResults(validationRun);

      // Assert
      foreach(var rule in rules)
      {
        Mock.Get(rule).Verify(x => x.Execute(validated), Times.Once());
      }
    }

    [Test]
    public void ExecuteRunAndGetResults_skips_unwanted_rules()
    {
      // Arrange
      var ruleOne = GetRunnableRule();
      var ruleTwo = GetRunnableRule();
      var ruleThree = GetRunnableRule();

      var rules = new [] { ruleOne, ruleTwo, ruleThree };

      var option = new Mock<IRuleSkippingOption>();
      option
        .Setup(x => x.ShouldSkipRule(It.IsAny<IRunnableRule>()))
        .Returns((IRunnableRule rule) => ReferenceEquals(rule, ruleTwo));

      var validated = new object();
      var validationRun = GetContext(rules, validated, new [] { option.Object });
      var sut = new ValidationRunner();

      // Act
      sut.ExecuteRunAndGetResults(validationRun);

      // Assert
      Mock.Get(ruleTwo).Verify(x => x.IntentionallySkip(validated), Times.Once());
    }

    [Test]
    public void ExecuteRunAndGetResults_executes_desired_rules()
    {
      // Arrange
      var ruleOne = GetRunnableRule();
      var ruleTwo = GetRunnableRule();
      var ruleThree = GetRunnableRule();

      var rules = new [] { ruleOne, ruleTwo, ruleThree };

      var option = new Mock<IRuleSkippingOption>();
      option
        .Setup(x => x.ShouldSkipRule(It.IsAny<IRunnableRule>()))
        .Returns((IRunnableRule rule) => ReferenceEquals(rule, ruleTwo));

      var validated = new object();
      var validationRun = GetContext(rules, validated, new [] { option.Object });
      var sut = new ValidationRunner();

      // Act
      sut.ExecuteRunAndGetResults(validationRun);

      // Assert
      Mock.Get(ruleOne).Verify(x => x.Execute(validated), Times.Once());
      Mock.Get(ruleThree).Verify(x => x.Execute(validated), Times.Once());
    }

    [Test]
    public void ExecuteRunAndGetResults_does_not_skip_rules_which_are_dependencies_of_desired_rules()
    {
      // Arrange
      var ruleOne = GetRunnableRule();
      var ruleTwo = GetRunnableRule();
      var ruleThree = GetRunnableRule(dependencies: new [] { ruleTwo });

      var rules = new [] { ruleOne, ruleTwo, ruleThree };

      var option = new Mock<IRuleSkippingOption>();
      option
        .Setup(x => x.ShouldSkipRule(It.IsAny<IRunnableRule>()))
        .Returns((IRunnableRule rule) => !ReferenceEquals(rule, ruleThree));

      var validated = new object();
      var validationRun = GetContext(rules, validated, new [] { option.Object });
      var sut = new ValidationRunner();

      // Act
      sut.ExecuteRunAndGetResults(validationRun);

      // Assert
      Mock.Get(ruleOne).Verify(x => x.IntentionallySkip(validated), Times.Once());
      Mock.Get(ruleTwo).Verify(x => x.Execute(validated), Times.Once());
      Mock.Get(ruleThree).Verify(x => x.Execute(validated), Times.Once());
    }

    [Test]
    public void ExecuteRunAndGetResults_does_not_skip_multi_tier_dependencies()
    {
      // Arrange
      var ruleOne = GetRunnableRule();
      var ruleTwo = GetRunnableRule(dependencies: new [] { ruleOne });
      var ruleThree = GetRunnableRule(dependencies: new [] { ruleTwo });

      var rules = new [] { ruleOne, ruleTwo, ruleThree };

      var option = new Mock<IRuleSkippingOption>();
      option
        .Setup(x => x.ShouldSkipRule(It.IsAny<IRunnableRule>()))
        .Returns((IRunnableRule rule) => !ReferenceEquals(rule, ruleThree));

      var validated = new object();
      var validationRun = GetContext(rules, validated, new [] { option.Object });
      var sut = new ValidationRunner();

      // Act
      sut.ExecuteRunAndGetResults(validationRun);

      // Assert
      Mock.Get(ruleOne).Verify(x => x.Execute(validated), Times.Once());
      Mock.Get(ruleTwo).Verify(x => x.Execute(validated), Times.Once());
      Mock.Get(ruleThree).Verify(x => x.Execute(validated), Times.Once());
    }

    IRunnableRule GetRunnableRule(IRunnableRuleResult result = null,
                                  IEnumerable<IRunnableRule> dependencies = null)
    {
      var hasRun = false;

      result = result?? Mock.Of<IRunnableRuleResult>();
      dependencies = dependencies?? Enumerable.Empty<IRunnableRule>();

      var rule = new Mock<IRunnableRule>();

      rule
        .SetupGet(x => x.MayBeExecuted)
        .Returns(() => {
          return !hasRun
                 && !dependencies.Any(x => !x.HasResult);
        });
      rule
        .SetupGet(x => x.HasResult)
        .Returns(() => hasRun);
      rule.Setup(x => x.GetResult()).Returns(result);
      rule
        .Setup(x => x.Execute(It.IsAny<object>()))
        .Callback((object val) => hasRun = true);
      rule
        .Setup(x => x.IntentionallySkip(It.IsAny<object>()))
        .Callback((object val) => hasRun = true);

      rule
        .Setup(x => x.GetDependencies())
        .Returns(dependencies);

      return rule.Object;
    }

    IValidationRunContext GetContext(IEnumerable<IRunnableRule> rules,
                                     object validated,
                                     IEnumerable<IRuleSkippingOption> ruleSkippingOptions = null)
    {
      if(validated == null)
        throw new ArgumentNullException(nameof(validated));
      if(rules == null)
        throw new ArgumentNullException(nameof(rules));

      ruleSkippingOptions = ruleSkippingOptions?? Enumerable.Empty<IRuleSkippingOption>();

      return Mock.Of<IValidationRunContext>(x => x.ValidationRun.Rules == rules
                                                 && x.Validated == validated
                                                 && x.Options.GetRuleSkippingOptions() == ruleSkippingOptions);
    }
  }
}
