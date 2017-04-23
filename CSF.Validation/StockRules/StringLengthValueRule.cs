//
// StringMaxLengthRule.cs
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

namespace CSF.Validation.StockRules
{
  /// <summary>
  /// Validation rule which fails validation if a string length is less than or greater than given
  /// minimum/maximum values.
  /// Note that this rule will pass if the string is <c>null</c>.  Combine this with another rule if the string
  /// must not be null.
  /// </summary>
  public class StringLengthValueRule : ValueRule<string>
  {
    /// <summary>
    /// Gets or sets the minimum length.
    /// </summary>
    /// <value>The minimum length.</value>
    public int? MinLength { get; set; }

    /// <summary>
    /// Gets or sets the maximum length.
    /// </summary>
    /// <value>The length of the max.</value>
    public int? MaxLength { get; set; }

    /// <summary>
    /// Gets the outcome.
    /// </summary>
    /// <returns>The outcome.</returns>
    /// <param name="validated">Validated.</param>
    /// <param name="value">Value.</param>
    protected override RuleOutcome GetValueOutcome(string value)
    {
      if(MinLength.HasValue
         && MaxLength.HasValue
         && MinLength.Value > MaxLength.Value)
        throw new InvalidValidationeRuleException(Resources.ExceptionMessages.MinMustNotBeGreaterThanMax);

      if(MinLength.GetValueOrDefault() < 0)
        throw new InvalidValidationeRuleException(Resources.ExceptionMessages.LengthMustNotBeNegative);

      if(MaxLength.GetValueOrDefault() < 0)
        throw new InvalidValidationeRuleException(Resources.ExceptionMessages.LengthMustNotBeNegative);

      if(value == null)
        return Success;

      var length = value.Length;
      if(MinLength.HasValue && length < MinLength.Value)
        return Failure;
      
      if(MaxLength.HasValue && length > MaxLength.Value)
        return Failure;

      return Success;
    }
  }
}
