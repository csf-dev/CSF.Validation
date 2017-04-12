//
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
  public class RunnableRuleResult : IRunnableRuleResult
  {
    readonly object manifestIdentity;
    readonly IRuleResult sourceResult;
    readonly object validated;

    /// <summary>
    /// Gets the identity of the rule within the rule manifest.
    /// </summary>
    /// <value>The identity of the rule in the manifest.</value>
    public object ManifestIdentity => manifestIdentity;

    /// <summary>
    /// Gets a reference to the object under validation.
    /// </summary>
    /// <value>The validated object.</value>
    public object Validated => validated;

    /// <summary>
    /// Gets the original <see cref="IRuleResult"/> from which the current instance was created.
    /// </summary>
    /// <value>The source result.</value>
    public IRuleResult RuleResult => sourceResult;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.ValidationRuns.RunnableRuleResult"/> class.
    /// </summary>
    /// <param name="manifestIdentity">Manifest identity.</param>
    /// <param name="sourceResult">The rule result.</param>
    /// <param name="validated">The validated object.</param>
    public RunnableRuleResult(object manifestIdentity, IRuleResult sourceResult, object validated)
    {
      if(sourceResult == null)
        throw new ArgumentNullException(nameof(sourceResult));
      if(manifestIdentity == null)
        throw new ArgumentNullException(nameof(manifestIdentity));

      this.manifestIdentity = manifestIdentity;
      this.sourceResult = sourceResult;
      this.validated = validated;
    }
  }
}
