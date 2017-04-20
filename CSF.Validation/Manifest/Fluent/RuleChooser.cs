//
// RuleRegistry.cs
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
using CSF.Validation.StockRules;

namespace CSF.Validation.Manifest.Fluent
{
  /// <summary>
  /// A registry of the stock rules which come with the validation framework.
  /// You may implement this interface with your own interfaces to expand upon it with your own rules.
  /// Please read the remarks about this type, as none of the methods are ever actually executed.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This type is really used only for generic type inferrence.  None of the methods on this type
  /// are ever actually implemented with a concrete type.  Even if they were, they would never be executed.
  /// </para>
  /// <para>
  /// By providing methods which take the appropriate generic type parameters, it makes it easier to refer to the rules
  /// in a fluent manifest builder, without needing to worry about their generic type parameters.
  /// By using methods liks this, the generic parameters are inferred.
  /// </para>
  /// </remarks>
  public class RuleChooser
  {
    /// <summary>
    /// Selects the <see cref="NotNullRule"/>.
    /// </summary>
    public static NotNullRule ObjectNotNull() => null;

    /// <summary>
    /// Selects the <see cref="T:NotNullValueRule{TValidated,TValue}"/>.
    /// </summary>
    public static NotNullValueRule<TValidated,TValue> NotNull<TValidated,TValue>(TValidated v1, TValue v2) => null;

    /// <summary>
    /// Selects the <see cref="T:NumericRangeValueRule{TValidated,TValue}"/>.
    /// </summary>
    public static NumericRangeValueRule<TValidated,TValue> NumericRange<TValidated,TValue>(TValidated v1, TValue v2)
      where TValue : struct => null;

    /// <summary>
    /// Selects the <see cref="T:NullableNumericRangeValueRule{TValidated,TValue}"/>.
    /// </summary>
    public static NullableNumericRangeValueRule<TValidated,TValue> NumericRange<TValidated,TValue>(TValidated v1, TValue? v2)
      where TValue : struct => null;

    /// <summary>
    /// Selects the <see cref="T:StringLengthValueRule{TValidated,TValue}"/>.
    /// </summary>
    public static StringLengthValueRule<TValidated> StringLength<TValidated>(TValidated v1, string v2) => null;
  }
}
