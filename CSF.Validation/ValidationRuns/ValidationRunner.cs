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

      while(readyToExecute.Any())
      {
        foreach(var rule in readyToExecute)
        {
          rule.Execute(validationContext.Validated);
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

    void CheckForRulesThatHaveNotRun(IEnumerable<IRunnableRule> rules)
    {
      if(rules.Any(x => !x.HasResult))
      {
        throw new StalledValidationRunException(Resources.ExceptionMessages.SomeValidationRulesCouldNotBeRun);
      }
    }
  }
}
