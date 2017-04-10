//
// RunnableRuleTests.cs
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
using NUnit.Framework;
using System;
using CSF.Validation.ValidationRuns;
using Moq;
using System.Linq;
using CSF.Validation.Rules;

namespace Test.CSF.ValidationRuns
{
  [TestFixture]
  public class RunnableRuleTests
  {
    [Test]
    public void MayBeExecuted_returns_true_when_rule_has_not_yet_been_run()
    {
      // Arrange
      var sut = new RunnableRule(new object(), StubRule.Success);

      // Act
      var result = sut.MayBeExecuted;

      // Assert
      Assert.IsTrue(result);
    }

    [Test]
    public void MayBeExecuted_returns_false_when_rule_has_already_been_run()
    {
      // Arrange
      var sut = new RunnableRule(new object(), StubRule.Success);
      sut.Execute(new object());

      // Act
      var result = sut.MayBeExecuted;

      // Assert
      Assert.IsFalse(result);
    }

    [Test]
    public void MayBeExecuted_returns_false_when_rule_dependencies_which_have_not_been_run()
    {
      // Arrange
      var sut = new RunnableRule(new object(), StubRule.Success);
      sut.ProvideDependencies(new [] { Mock.Of<IRunnableRule>(x => x.HasResult == false) });

      // Act
      var result = sut.MayBeExecuted;

      // Assert
      Assert.IsFalse(result);
    }

    [Test]
    public void HasResult_returns_false_when_rule_has_not_been_run()
    {
      // Arrange
      var sut = new RunnableRule(new object(), StubRule.Success);

      // Act
      var result = sut.HasResult;

      // Assert
      Assert.IsFalse(result);
    }

    [Test]
    public void HasResult_returns_true_after_rule_has_been_run()
    {
      // Arrange
      var sut = new RunnableRule(new object(), StubRule.Success);
      sut.Execute(new object());

      // Act
      var result = sut.HasResult;

      // Assert
      Assert.IsTrue(result);
    }

    [Test]
    public void HasResult_returns_true_when_rule_has_been_skipped_due_to_dependency_failure()
    {
      // Arrange
      var dependency = new RunnableRule(new object(), StubRule.Failure);
      dependency.Execute(new object());

      var sut = new RunnableRule(new object(), StubRule.Success);
      sut.ProvideDependencies(new [] { dependency });
      sut.Execute(new object());

      // Act
      var result = sut.HasResult;

      // Assert
      Assert.IsTrue(result);
    }

    [Test]
    public void Execute_throws_exception_if_rule_has_already_been_run()
    {
      // Arrange
      var sut = new RunnableRule(new object(), StubRule.Success);
      sut.Execute(new object());

      // Act & assert
      Assert.Throws<InvalidOperationException>(() => sut.Execute(new object()));
    }

    [Test]
    public void Execute_throws_exception_when_rule_dependencies_which_have_not_been_run()
    {
      // Arrange
      var sut = new RunnableRule(new object(), StubRule.Success);
      sut.ProvideDependencies(new [] { Mock.Of<IRunnableRule>(x => x.HasResult == false) });

      // Act & assert
      Assert.Throws<InvalidOperationException>(() => sut.Execute(new object()));
    }

    [Test]
    public void Execute_does_not_execute_underlying_rule_if_a_dependency_rule_has_failed()
    {
      // Arrange
      var dependency = new RunnableRule(new object(), StubRule.Failure);
      dependency.Execute(new object());

      var underlying = StubRule.Success;

      var sut = new RunnableRule(new object(), underlying);
      sut.ProvideDependencies(new [] { dependency });

      // Act
      sut.Execute(new object());

      // Assert
      Mock.Get(underlying).Verify(x => x.GetResult(It.IsAny<Object>()), Times.Never());
    }

    [Test]
    public void Execute_executes_underlying_rule_if_all_dependencies_are_successful()
    {
      // Arrange
      var dependency = new RunnableRule(new object(), StubRule.Success);
      dependency.Execute(new object());

      var underlying = StubRule.Success;

      var sut = new RunnableRule(new object(), underlying);
      sut.ProvideDependencies(new [] { dependency });

      // Act
      sut.Execute(new object());

      // Assert
      Mock.Get(underlying).Verify(x => x.GetResult(It.IsAny<Object>()), Times.Once());
    }

    [Test]
    public void Execute_executes_underlying_rule_if_there_are_no_dependencies()
    {
      // Arrange
      var underlying = StubRule.Success;
      var sut = new RunnableRule(new object(), underlying);

      // Act
      sut.Execute(new object());

      // Assert
      Mock.Get(underlying).Verify(x => x.GetResult(It.IsAny<Object>()), Times.Once());
    }

    [Test]
    public void ProvideDependencies_throws_exception_if_called_twice()
    {
      // Arrange
      var sut = new RunnableRule(new object(), StubRule.Success);

      // Act & assert
      sut.ProvideDependencies(Enumerable.Empty<IRunnableRule>());
      Assert.Throws<InvalidOperationException>(() => sut.ProvideDependencies(Enumerable.Empty<IRunnableRule>()));
    }

    [Test]
    public void GetResult_throws_exception_if_rule_has_not_been_executed()
    {
      // Arrange
      var sut = new RunnableRule(new object(), StubRule.Success);

      // Act & assert
      Assert.Throws<InvalidOperationException>(() => sut.GetResult());
    }

    [Test]
    public void GetResult_returns_result_with_skipped_outcome_if_dependency_rule_failed()
    {
      // Arrange
      var dependency = new RunnableRule(new object(), StubRule.Failure);
      dependency.Execute(new object());

      var sut = new RunnableRule(new object(), StubRule.Success);
      sut.ProvideDependencies(new [] { dependency });
      sut.Execute(new object());

      // Act
      var result = sut.GetResult();

      // Assert
      Assert.AreEqual(RuleOutcome.SkippedDueToDependencyFailure, result.Outcome);
    }

    [TestCase(RuleOutcome.Success)]
    [TestCase(RuleOutcome.Failure)]
    [TestCase(RuleOutcome.Error)]
    public void GetResult_returns_result_with_same_outcome_as_underlying_rule(RuleOutcome underlyingOutcome)
    {
      // Arrange
      var sut = new RunnableRule(new object(), StubRule.Create(underlyingOutcome));
      sut.Execute(new object());

      // Act
      var result = sut.GetResult();

      // Assert
      Assert.AreEqual(underlyingOutcome, result.Outcome);
    }

    [Test]
    public void GetResult_returns_result_with_same_manifest_identity()
    {
      // Arrange
      var identifier = new object();
      var sut = new RunnableRule(identifier, StubRule.Success);
      sut.Execute(new object());

      // Act
      var result = sut.GetResult();

      // Assert
      Assert.AreSame(identifier, result.ManifestIdentity);
    }
  }
}
