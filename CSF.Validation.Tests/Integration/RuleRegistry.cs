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

namespace CSF.Validation.Tests.Integration
{
  public static class RuleRegistry
  {
    public static NotNullRule NotNullObject<TValidated>(TValidated v1)
      => new NotNullRule();

    public static NotNullValueRule<TValidated,TValue> NotNull<TValidated,TValue>(TValidated v1, TValue v2)
      => new NotNullValueRule<TValidated,TValue>();

    public static NumericRangeValueRule<TValidated,TValue> NumericRange<TValidated,TValue>(TValidated v1, TValue v2) where TValue : struct
      => new NumericRangeValueRule<TValidated,TValue>();

    public static NullableNumericRangeValueRule<TValidated,TValue> NullableNumericRange<TValidated,TValue>(TValidated v1, TValue? v2) where TValue : struct
      => new NullableNumericRangeValueRule<TValidated,TValue>();

    public static StringLengthValueRule<TValidated> StringLength<TValidated>(TValidated v1, string v2)
      => new StringLengthValueRule<TValidated>();
  }
}
