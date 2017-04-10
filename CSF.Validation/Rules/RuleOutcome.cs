//
// RuleOutcome.cs
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
  /// Enumerates the possible outcomes of running a single validation rule.
  /// </summary>
  public enum RuleOutcome
  {
    /// <summary>
    /// The outcome indicates that the rule executed and returned a failure response.
    /// </summary>
    Failure = 0,

    /// <summary>
    /// The outcome indicates successful execution of the rule.
    /// </summary>
    Success,

    /// <summary>
    /// The outcome indicates that the rule raised an unexpected error during execution.
    /// </summary>
    Error,

    /// <summary>
    /// The rule was not executed, because one or more other rules upon which it
    /// depended returned a failure outcome.
    /// </summary>
    SkippedDueToDependencyFailure,
  }
}
