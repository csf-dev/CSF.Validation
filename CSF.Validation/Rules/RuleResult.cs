//
// RuleResult.cs
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
  /// Concrete type representing the result of a standard validation rule run.
  /// </summary>
  public class RuleResult : IRuleResult
  {
    readonly RuleOutcome outcome;
    readonly object validated;

    /// <summary>
    /// Gets the outcome.
    /// </summary>
    /// <value>The outcome.</value>
    public RuleOutcome Outcome => outcome;

    /// <summary>
    /// Gets the object which was validated.
    /// </summary>
    /// <value>The validated.</value>
    public object Validated => validated;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.Rules.RuleResult"/> class.
    /// </summary>
    /// <param name="outcome">Outcome.</param>
    /// <param name="validated">Validated.</param>
    public RuleResult(RuleOutcome outcome, object validated)
    {
      outcome.RequireDefinedValue(nameof(outcome));

      this.validated = validated;
      this.outcome = outcome;
    }
  }
}
