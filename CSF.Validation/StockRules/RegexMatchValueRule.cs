﻿//
// RegexMatchValueRule.cs
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
using System.Text.RegularExpressions;

namespace CSF.Validation.StockRules
{
  /// <summary>
  /// Validation rule which checks for a match with a given regular expression pattern.
  /// This rule will indicate success if the input value is <c>null</c>.
  /// </summary>
  public class RegexMatchValueRule : ValueRule<string>
  {
    /// <summary>
    /// Gets or sets the regex pattern against which to test.
    /// </summary>
    /// <value>The pattern.</value>
    public string Pattern { get; set; }

    /// <summary>
    /// Gets or sets the regex options.
    /// </summary>
    /// <value>The options.</value>
    public RegexOptions Options { get; set; }

    /// <summary>
    /// Gets the outcome.
    /// </summary>
    /// <returns>The outcome.</returns>
    /// <param name="value">Value.</param>
    protected override RuleOutcome GetValueOutcome(string value)
    {
      if(ReferenceEquals(value, null))
        return Success;

      if(Regex.IsMatch(value, Pattern, Options))
        return Success;

      return Failure;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.StockRules.RegexMatchValueRule"/> class.
    /// </summary>
    public RegexMatchValueRule()
    {
      Options = RegexOptions.None;
    }
  }
}
