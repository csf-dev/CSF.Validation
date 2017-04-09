//
// IRunnableRule.cs
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
  /// Represents a single validation rule within a validation run.
  /// </summary>
  public interface IRunnableRule
  {
    /// <summary>
    /// Gets a value indicating whether or not the rule wrapped by the current instance may be executed or not.
    /// </summary>
    /// <value><c>true</c> if the current instance may be executed; otherwise, <c>false</c>.</value>
    bool MayBeExecuted { get; }

    /// <summary>
    /// Gets a value indicating whether this rule has already been executed in the context of the current
    /// validation operation.
    /// </summary>
    /// <value><c>true</c> if the rule has already been executed; otherwise, <c>false</c>.</value>
    bool HasAlreadyExecuted { get; }

    /// <summary>
    /// Gets the result of the rule's execution.
    /// </summary>
    /// <returns>The result.</returns>
    IRunnableRuleResult GetResult();

    /// <summary>
    /// Execute the current rule, for the given validated object.
    /// </summary>
    /// <param name="validated">Validated.</param>
    void Execute(object validated);
  }
}
