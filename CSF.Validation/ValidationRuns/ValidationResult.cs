//
// ValidationResult.cs
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
using CSF.Validation.Rules;

namespace CSF.Validation.ValidationRuns
{
  /// <summary>
  /// Concrete implementation of <see cref="IValidationResult"/>, holding the rule results.
  /// </summary>
  public class ValidationResult : IValidationResult
  {
    static readonly RuleOutcome[]
      OutcomesToTreatAsSucceses = { RuleOutcome.Success, RuleOutcome.IntentionallySkipped };

    readonly IEnumerable<IRunnableRuleResult> ruleResults;

    /// <summary>
    /// Gets a value indicating whether or not the current instance has only successful results.
    /// That is - all of the rule results should be treated as successes.
    /// </summary>
    /// <value><c>true</c> if the current instance indicates a success; otherwise, <c>false</c>.</value>
    public bool IsSuccess => RuleResults.All(TreatAsSuccess);

    /// <summary>
    /// Gets a collection of the results from each of the rules which was executed.
    /// </summary>
    /// <value>The rule results.</value>
    public IEnumerable<IRunnableRuleResult> RuleResults => ruleResults;

    bool TreatAsSuccess(IRunnableRuleResult result)
    {
      if(result == null)
        throw new ArgumentNullException(nameof(result));

      return OutcomesToTreatAsSucceses.Contains(result.Outcome);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.ValidationRuns.ValidationResult"/> class.
    /// </summary>
    /// <param name="ruleResults">Rule results.</param>
    public ValidationResult(IEnumerable<IRunnableRuleResult> ruleResults)
    {
      if(ruleResults == null)
        throw new ArgumentNullException(nameof(ruleResults));

      this.ruleResults = ruleResults;
    }
  }
}
