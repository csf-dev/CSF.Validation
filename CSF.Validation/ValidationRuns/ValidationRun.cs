﻿//
// ValidationRun.cs
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

namespace CSF.Validation.ValidationRuns
{
  /// <summary>
  /// Default implementation of <see cref="IValidationRun"/>, representing a constructed validation run.
  /// </summary>
  public class ValidationRun : IValidationRun
  {
    readonly IEnumerable<IRunnableRule> rules;

    /// <summary>
    /// Gets the rules contained within the current run.
    /// </summary>
    /// <value>The rules.</value>
    public IEnumerable<IRunnableRule> Rules => rules;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationRun"/> class.
    /// </summary>
    /// <param name="rules">Rules.</param>
    public ValidationRun(IEnumerable<IRunnableRule> rules)
    {
      if(rules == null)
        throw new ArgumentNullException(nameof(rules));

      this.rules = rules;
    }
  }
}
