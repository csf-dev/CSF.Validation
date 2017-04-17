//
// ValidationRunner.cs
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

namespace CSF.Validation.ValidationRuns
{
  /// <summary>
  /// Default implementation of <see cref="IValidationRunner"/>.
  /// </summary>
  public class ValidationRunner : IValidationRunner
  {
    /// <summary>
    /// Executes all of the rules in a given validation run and returns a collection of their results.
    /// </summary>
    /// <returns>The validation rule results.</returns>
    /// <param name="validationContext">The validation context.</param>
    public IEnumerable<IRunnableRuleResult> ExecuteRunAndGetResults(IValidationRunContext validationContext)
    {
      var rules = validationContext.ValidationRun.Rules;
      var readyToExecute = GetRulesReadyForExecution(rules);
      var skipOptions = validationContext.Options.GetRuleSkippingOptions();

      while(readyToExecute.Any())
      {
        foreach(var rule in readyToExecute)
        {
          if(ShouldSkipRule(rule, skipOptions, rules))
          {
            rule.IntentionallySkip(validationContext.Validated);
          }
          else
          {
            rule.Execute(validationContext.Validated);
          }
        }

        readyToExecute = GetRulesReadyForExecution(rules);
      }

      CheckForRulesThatHaveNotRun(rules);

      return rules.Select(x => x.GetResult()).ToArray();
    }

    /// <summary>
    /// Gets a subset of the given rules, returning those which are ready to execute.
    /// </summary>
    /// <returns>The rules which are ready for execution.</returns>
    /// <param name="rules">A collection of runnable rules.</param>
    protected virtual IEnumerable<IRunnableRule> GetRulesReadyForExecution(IEnumerable<IRunnableRule> rules)
    {
      return rules.Where(x => x.MayBeExecuted).ToArray();
    }

    /// <summary>
    /// Determines whether or not the current rule should be skipped or not.
    /// This implementation avoids skipping a rule if it has any rules dependant upon it, which themselves should not
    /// be skipped.
    /// </summary>
    /// <returns><c>true</c>, if rule should be skipped, <c>false</c> otherwise.</returns>
    /// <param name="rule">Rule.</param>
    /// <param name="skipOptions">Skip options.</param>
    /// <param name="allRules">All rules.</param>
    protected virtual bool ShouldSkipRule(IRunnableRule rule,
                                          IEnumerable<IRuleSkippingOption> skipOptions,
                                          IEnumerable<IRunnableRule> allRules)
    {
      var rulesWhichDependUponThisRule = allRules
        .Where(x => x.GetDependencies().Any(d => d == rule))
        .ToArray();

      if(rulesWhichDependUponThisRule.Any(x => !ShouldSkipRule(x, skipOptions, allRules)))
        return false;

      return skipOptions.Any(x => x.ShouldSkipRule(rule));
    }

    void CheckForRulesThatHaveNotRun(IEnumerable<IRunnableRule> rules)
    {
      if(rules.Any(x => !x.HasResult))
      {
        throw new StalledValidationRunException(Resources.ExceptionMessages.SomeValidationRulesCouldNotBeRun);
      }
    }
  }
}
