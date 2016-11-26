//
// SimpleCalculator.cs
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
using System.Text.RegularExpressions;

namespace CSF
{
  /// <summary>
  /// Simple implementation of an <see cref="ICalculator"/>.
  /// </summary>
  public class SimpleCalculator : ICalculator
  {
    #region constants
    
    private const string CALCULATION_PATTERN = @"^\=?(([+*/-]?)((\d*\.\d+)|(\d+)))+$";
    
    private static readonly Regex ValidCalculation = new Regex(CALCULATION_PATTERN, RegexOptions.Compiled);
    
    #endregion

    #region ICalculator implementation
    
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
    public decimal CalculateDecimal (string input)
    {
      bool first = true;
      decimal output = 0m;
      
      if(input == null)
      {
        throw new ArgumentNullException ("input");
      }
      
      Match match = ValidCalculation.Match(input);
      if(!match.Success)
      {
        throw new FormatException("Input is not a valid calculation supported by this calculator.");
      }
      
      for(int i = 0; i < match.Groups[1].Captures.Count; i++)
      {
        decimal operand = Decimal.Parse(match.Groups[3].Captures[i].Value);
        string operatorString = match.Groups[2].Captures[i].Value;
        
        if(first && (operatorString == "*" || operatorString == "/"))
        {
          throw new FormatException("Input is invalid, may not begin with a multiplication or division operator.");
        }
        else if(first && String.IsNullOrEmpty(operatorString))
        {
          operatorString = "+";
        }
        
        switch(operatorString)
        {
        case "+":
          output = output + operand;
          break;
          
        case "-":
          output = output - operand;
          break;
          
        case "*":
          output = output * operand;
          break;
          
        case "/":
          output = output / operand;
          break;
          
        default:
          throw new Exception(String.Format("Incorrect regular expression, not a valid operator: '{0}'",
                                            operatorString));
        }
        
        first = false;
      }

      return output;
    }

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
    public bool TryCalculateDecimal (string input, out decimal result)
    {
      result = default(decimal);
      bool output = false;
      
      try
      {
        result = this.CalculateDecimal(input);
        output = true;
      }
      // Drop these exceptions on the floor - the output will be false and we don't care about the result parameter.
      catch(ArgumentNullException) {}
      catch(FormatException) {}
      
      return output;
    }
  
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
    public int CalculateInteger (string input)
    {
      return (int) this.CalculateDecimal(input);
    }

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
    public bool TryCalculateInteger (string input, out int result)
    {
      decimal decimalResult;
      
      bool output = this.TryCalculateDecimal(input, out decimalResult);
      result = (int) decimalResult;
      
      return output;
    }
    
    #endregion
    
  }
}

