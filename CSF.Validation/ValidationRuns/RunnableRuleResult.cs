﻿//
// SkippedRuleResult.cs
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
using CSF.Validation.Rules;

namespace CSF.Validation.ValidationRuns
{
  /// <summary>
  /// Represents the result from an <see cref="IRunnableRule"/>.
  /// </summary>
  public class RunnableRuleResult : RuleResult, IRunnableRuleResult
  {
    readonly object manifestIdentity;

    /// <summary>
    /// Gets the identity of the rule within the rule manifest.
    /// </summary>
    /// <value>The identity of the rule in the manifest.</value>
    public object ManifestIdentity => manifestIdentity;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.ValidationRuns.RunnableRuleResult"/> class.
    /// </summary>
    /// <param name="manifestIdentity">Manifest identity.</param>
    /// <param name="outcome">Outcome.</param>
    public RunnableRuleResult(object manifestIdentity, RuleOutcome outcome) : base(outcome)
    {
      if(manifestIdentity == null)
        throw new ArgumentNullException(nameof(manifestIdentity));

      this.manifestIdentity = manifestIdentity;
    }

    /// <summary>
    /// Static factory method which creates an instance of <see cref="RunnableRuleResult"/> from an
    /// <see cref="IRuleResult"/> and a manifest identity.
    /// </summary>
    /// <param name="manifestIdentity">Manifest identity.</param>
    /// <param name="ruleResult">Rule result.</param>
    internal static RunnableRuleResult Create(object manifestIdentity, IRuleResult ruleResult)
    {
      return new RunnableRuleResult(manifestIdentity, ruleResult.Outcome);
    }
  }
}
