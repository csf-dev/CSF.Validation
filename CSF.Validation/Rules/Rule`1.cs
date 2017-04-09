//
// Rule1.cs
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
  /// Generic implementation of <see cref="Rule"/>, which allows the instance under validation to be strongly-typed.
  /// </summary>
  public abstract class Rule<TValidated> : Rule, IRule
  {
    /// <summary>
    /// Executes the rule and returns its result.
    /// </summary>
    /// <returns>The result.</returns>
    /// <param name="validated">The object undergoing validation.</param>
    IRuleResult IRule.GetResult(object validated)
    {
      try
      {
        var typedValidated = (TValidated) validated;
        var outcome = GetOutcome(typedValidated);
        return new RuleResult(outcome, validated);
      }
      catch(Exception ex)
      {
        return new ExceptionResult(ex, validated);
      }
    }

    /// <summary>
    /// Sealed implementation of the non-generic <see cref="GetOutcome(object)"/>.
    /// </summary>
    /// <returns>The outcome.</returns>
    /// <param name="validated">The object undergoing validation.</param>
    protected sealed override RuleOutcome GetOutcome(object validated)
    {
      return GetOutcome((TValidated) validated);
    }

    /// <summary>
    /// Gets the outcome of the validation, override this method in derived types.
    /// </summary>
    /// <returns>The outcome.</returns>
    /// <param name="validated">The object undergoing validation.</param>
    protected abstract RuleOutcome GetOutcome(TValidated validated);
  }
}
