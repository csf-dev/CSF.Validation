//
// RuleSelectionOption.cs
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
using CSF.Validation.ValidationRuns;

namespace CSF.Validation.Options
{
  /// <summary>
  /// Represents a validation option which determines whether a given validation rule should be executed or not.
  /// </summary>
  public class RuleSelectionOption : IRuleSkippingOption
  {
    readonly Func<IRunnableRule,bool> predicate;

    /// <summary>
    /// Gets a predicate which examines a validation rule.  It should return <c>true</c> if the rule should be executed
    /// or <c>false</c> if it should be skipped.
    /// </summary>
    /// <value>The rule-selection predicate.</value>
    public Func<IRunnableRule,bool> Predicate => predicate;

    bool IRuleSkippingOption.ShouldSkipRule(IRunnableRule rule)
    {
      return !Predicate(rule);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RuleSelectionOption"/> class.
    /// </summary>
    /// <param name="predicate">Predicate.</param>
    public RuleSelectionOption(Func<IRunnableRule,bool> predicate)
    {
      if(predicate == null)
        throw new ArgumentNullException(nameof(predicate));

      this.predicate = predicate;
    }
  }
}
