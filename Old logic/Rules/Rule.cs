//
// Rule.cs
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
namespace CSF.Validation.Rules
{
  /// <summary>
  /// Base type for a validation rule.
  /// </summary>
  public abstract class Rule : IRule
  {
    RuleOutcome
      success = RuleOutcome.Success,
      failure = RuleOutcome.Failure,
      error = RuleOutcome.Error;

    IRuleResult IRule.GetResult(object validated)
    {
      try
      {
        var outcome = GetOutcome(validated);
        return new RuleResult(outcome);
      }
      catch(Exception ex)
      {
        return new ExceptionResult(ex);
      }
    }

    /// <summary>
    /// Gets a success result.
    /// </summary>
    /// <value>A success result.</value>
    protected RuleOutcome Success => success;

    /// <summary>
    /// Gets a failure result.
    /// </summary>
    /// <value>A failure result.</value>
    protected RuleOutcome Failure => failure;

    /// <summary>
    /// Gets an error result.
    /// </summary>
    /// <value>An error result.</value>
    protected RuleOutcome Error => error;

    /// <summary>
    /// Gets the outcome of the validation, override this method in derived types.
    /// </summary>
    /// <returns>The outcome.</returns>
    /// <param name="validated">The object undergoing validation.</param>
    protected abstract RuleOutcome GetOutcome(object validated);
  }
}
