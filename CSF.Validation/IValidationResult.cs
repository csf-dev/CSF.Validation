//
// IValidationResult.cs
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
using CSF.Validation.Rules;

namespace CSF.Validation
{
  /// <summary>
  /// Represents the result of validation.
  /// </summary>
  public interface IValidationResult
  {
    /// <summary>
    /// Gets a value indicating whether or not the current instance has no failures or errors.
    /// That is - all of the rule results are either <see cref="RuleOutcome.Success"/> or
    /// <see cref="RuleOutcome.Inconclusive"/>.
    /// </summary>
    /// <value><c>true</c> if the current instance has no failures or errors; otherwise, <c>false</c>.</value>
    bool HasNoFailuresOrErrors { get; }

    /// <summary>
    /// Gets a collection of the results from each of the rules which was executed.
    /// </summary>
    /// <value>The rule results.</value>
    IEnumerable<IRuleResult> RuleResults { get; }
  }
}
