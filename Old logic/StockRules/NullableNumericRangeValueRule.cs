//
// NullableNumericRangeValueRule.cs
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
  /// Validation rule which fails validation if a numeric value is less than or greater than given
  /// minimum/maximum values.
  /// Note that this rule will pass if the input value is <c>null</c>.  Combine this with another rule if the value
  /// must not be null.
  /// </summary>
  public class NullableNumericRangeValueRule : ValueRule<double?>
  {
    /// <summary>
    /// Gets or sets the minimum value.
    /// </summary>
    /// <value>The minimum.</value>
    public double? Min { get; set; }

    /// <summary>
    /// Gets or sets the maximum.
    /// </summary>
    /// <value>The max.</value>
    public double? Max { get; set; }

    /// <summary>
    /// Converts an object to the type which we intend to validate.  This method may raise exceptions if applicable.
    /// </summary>
    /// <returns>The converted value.</returns>
    /// <param name="toConvert">The value to convert.</param>
    protected override double? ConvertValueUnsafe(object toConvert)
    {
      return Convert.ToDouble(toConvert);
    }

    /// <summary>
    /// Gets the outcome.
    /// </summary>
    /// <returns>The outcome.</returns>
    /// <param name="value">Value.</param>
    protected override RuleOutcome GetValueOutcome(double? value)
    {
      if(!value.HasValue)
        return Success;

      if(Min.HasValue
         && Max.HasValue
         && Min.Value > Max.Value)
        throw new InvalidValidationeRuleException(Resources.ExceptionMessages.MinMustNotBeGreaterThanMax);

      if(Min.HasValue && value.Value < Min.Value)
        return Failure;

      if(Max.HasValue && value.Value > Max.Value)
        return Failure;

      return Success;
    }
  }
}
