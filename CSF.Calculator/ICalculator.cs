//
// ICalculator.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2015 CSF Software Limited
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

namespace CSF
{
  /// <summary>
  /// Interface for a type that provides calculation services for <see cref="System.String"/> values, returning numeric
  /// results.
  /// </summary>
  public interface ICalculator
  {
    #region methods
    
    /// <summary>
    /// Calculates the input and returns the result as a <see cref="System.Decimal"/>.
    /// </summary>
    /// <param name='input'>
    /// The string-based input from which to calculate a result.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="input"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="FormatException">
    /// Thrown if the <paramref name="input"/> was not formatted in a manner that this calculator can understand.
    /// </exception>
    decimal CalculateDecimal(string input);
    
    /// <summary>
    /// Tries the input and exposes the <paramref name="result"/> the result as a <see cref="System.Decimal"/> or
    /// otherwise return <c>false</c>.
    /// </summary>
    /// <returns>
    /// A value that indicates whether or not the calculation was a success or not.
    /// </returns>
    /// <param name='input'>
    /// The string-based input from which to calculate a result.
    /// </param>
    /// <param name='result'>
    /// If the return value is <c>true</c>, this parameter exposes the calculated result.  If <c>false</c> then the
    /// value exposed by this property is undefined.
    /// </param>
    bool TryCalculateDecimal(string input, out decimal result);
    
    /// <summary>
    /// Calculates the input and returns the result as a <see cref="System.Int32"/>.
    /// </summary>
    /// <param name='input'>
    /// The string-based input from which to calculate a result.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="input"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="FormatException">
    /// Thrown if the <paramref name="input"/> was not formatted in a manner that this calculator can understand.
    /// </exception>
    int CalculateInteger(string input);
    
    /// <summary>
    /// Tries the input and exposes the <paramref name="result"/> the result as a <see cref="System.Int32"/> or
    /// otherwise return <c>false</c>.
    /// </summary>
    /// <returns>
    /// A value that indicates whether or not the calculation was a success or not.
    /// </returns>
    /// <param name='input'>
    /// The string-based input from which to calculate a result.
    /// </param>
    /// <param name='result'>
    /// If the return value is <c>true</c>, this parameter exposes the calculated result.  If <c>false</c> then the
    /// value exposed by this property is undefined.
    /// </param>
    bool TryCalculateInteger(string input, out int result);
    
    #endregion
  }
}

