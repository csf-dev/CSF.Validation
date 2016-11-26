//
// Fraction.cs
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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CSF
{
  /// <summary>
  /// <para>
  /// Representation of a fraction, a rational number with an <see cref="Int32"/> numerator and an <see cref="Int32"/>
  /// denominator.  This type is immutable.
  /// </para>
  /// </summary>
  public struct Fraction : ICloneable
  {
    #region constants
    
    private static readonly Regex FractionParser = new Regex(@"^(-?\d+)/(-?\d+)$", RegexOptions.Compiled);
    
    #endregion
    
    #region fields
    
    private int _numerator, _denominator;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the numerator for the current instance.</para>
    /// </summary>
    public int Numerator
    {
      get {
        return _numerator;
      }
      private set {
        _numerator = value;
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the denoninator for the current instance.</para>
    /// </summary>
    public int Denominator
    {
      get {
        return _denominator;
      }
      private set {
        _denominator = value;
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Determines whether the current instance can be simplified down to an integer value.</para>
    /// </summary>
    public bool SimplifiesToInteger
    {
      get {
        return (this.Numerator % this.Denominator == 0);
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Overridden, compares for equality with <paramref name="obj"/>.</para>
    /// </summary>
    /// <param name="obj">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public override bool Equals (object obj)
    {
      bool output;
      
      if(obj is Fraction)
      {
        Fraction
          compareTo = ((Fraction) obj),
          myself = this;
        
        /* There's an optimisation here - if the two fractions are equal before simplification then we don't need to
         * simplify them at all.
         */
        if(myself.Numerator == compareTo.Numerator
           && myself.Denominator == compareTo.Denominator)
        {
          output = true;
        }
        else
        {
          compareTo = compareTo.Simplify();
          myself = myself.Simplify();
          output = (myself.Numerator == compareTo.Numerator
                    && myself.Denominator == compareTo.Denominator);
        }
      }
      else
      {
        output = false;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Gets a hash code for the current instance.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Int32"/>
    /// </returns>
    public override int GetHashCode ()
    {
      return this.Numerator ^ this.Denominator;
    }
    
    /// <summary>
    /// <para>Creates a copy of the current instance, reduced to its simplest form.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="Fraction"/>
    /// </returns>
    public Fraction Simplify()
    {
      int newNumerator = this.Numerator, newDenominator = this.Denominator;
      IEnumerable<int> commonFactors;
      
      if(newNumerator == 0)
      {
        // We can make an optimisation here if the fraction is zero-valued
        newDenominator = 1;
      }
      else
      {
        commonFactors = PrimeFactoriser.Default.GetCommonPrimeFactors(this.Numerator, this.Denominator);
        foreach(int factor in commonFactors)
        {
          newNumerator = newNumerator / factor;
          newDenominator = newDenominator / factor;
        }
      }
      
      return new Fraction(newNumerator, newDenominator);
    }
    
    /// <summary>
    /// <para>Creates a string representation of the current instance.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// The formatting of this output will always be <c>x/y</c>, where x and y may be preceded by a negative symbol.
    /// This formatting can result in vulgar fractions (numerator larger than the denominator).
    /// </para>
    /// </remarks>
    /// <returns>
    /// A <see cref="System.String"/>
    /// </returns>
    public override string ToString ()
    {
      return String.Format("{0}/{1}", this.Numerator, this.Denominator);
    }
    
    /// <summary>
    /// <para>Creates a string representation of the current instance.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// The formatting of this output will always be <c>z x/y</c>, where z is optional and may be preceded by a
    /// negative symbol and the <c>x/y</c> portion is also optional.
    /// This formatting will never result in vulgar fractions, since z contains the largest (absolute-value) integer
    /// that the current instance can create.
    /// </para>
    /// <para>
    /// If the fractional part of this output is missing (IE: only z is written) then the current instance represents
    /// an integral value.
    /// </para>
    /// <para>
    /// If, alternatively, the integer part of this output is missing (IE: only x/y is written) then the current
    /// instance represents an absolute value less than one.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A <see cref="System.String"/>
    /// </returns>
    public string ToCompositeString ()
    {
      string output;
      
      if(this.SimplifiesToInteger)
      {
        output = this.ToInteger().ToString();
      }
      else
      {
        int
          integerPortion = (int) Math.Truncate((double) this.Numerator / (double) this.Denominator),
          remainder = (this.Numerator % this.Denominator);
        
        if(integerPortion != 0)
        {
          output = String.Format("{0} {1}", integerPortion, new Fraction(Math.Abs(remainder), this.Denominator));
        }
        else
        {
          output = new Fraction(remainder, this.Denominator).ToString();
        }
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Creates and returns a <see cref="System.Decimal"/> representation of the current instance.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Decimal"/>
    /// </returns>
    public decimal ToDecimal()
    {
      return (decimal) this.Numerator / (decimal) this.Denominator;
    }
    
    /// <summary>
    /// <para>Creates and returns a <see cref="System.Int32"/> representation of the current instance.</para>
    /// </summary>
    /// <remarks>
    /// <para>This is only valid where <see cref="SimplifiesToInteger"/> is true.</para>
    /// </remarks>
    /// <returns>
    /// A <see cref="System.Int32"/>
    /// </returns>
    public int ToInteger()
    {
      if(!this.SimplifiesToInteger)
      {
        throw new InvalidOperationException("This fraction does not simplify to an integer");
      }
      
      return (int) (this.Numerator / this.Denominator);
    }
    
    #endregion
    
    #region ICloneable implementation
    
    /// <summary>
    /// <para>Creates a deep copy of the current instance.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="Fraction"/>
    /// </returns>
    public Fraction Clone ()
    {
      return new Fraction(this.Numerator, this.Denominator);
    }
    
    object ICloneable.Clone ()
    {
      return this.Clone();
    }
 
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Initialises this instance.</para>
    /// </summary>
    /// <param name="numerator">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <param name="denominator">
    /// A <see cref="System.Int32"/>
    /// </param>
    public Fraction (int numerator, int denominator)
    {
      if(denominator == 0)
      {
        throw new ArgumentOutOfRangeException("denominator", "Fraction denominator cannot be zero");
      }
      
      _numerator = numerator;
      _denominator = denominator;
    }
    
    #endregion
    
    #region static methods
    
    /// <summary>
    /// <para>Parses a <see cref="System.String"/> in the format <c>x/y</c> as a <see cref="Fraction"/>.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Both X and Y in the specification must be integers (positive or negative) and within the range of a signed
    /// <see cref="System.Int32"/>.
    /// </para>
    /// </remarks>
    /// <param name="specification">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="Fraction"/>
    /// </returns>
    public static Fraction Parse(string specification)
    {
      Match specificationMatch;
      
      if(specification == null)
      {
        throw new ArgumentNullException("specification");
      }
      
      specificationMatch = FractionParser.Match(specification);
      
      if(!specificationMatch.Success)
      {
        throw new ArgumentException("String to parse does not represent a valid fraction", "specification");
      }
      
      return new Fraction(Int32.Parse(specificationMatch.Groups[1].Value),
                          Int32.Parse(specificationMatch.Groups[2].Value));
    }
    
    #endregion
    
    #region operator overloads
    
    /// <summary>
    /// <para>Operator overload for equality.</para>
    /// </summary>
    /// <param name="x">
    /// A <see cref="Fraction"/>
    /// </param>
    /// <param name="y">
    /// A <see cref="Fraction"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public static bool operator == (Fraction x, Fraction y)
    {
      return x.Equals(y);
    }
    
    /// <summary>
    /// <para>Operator overload for inequality.</para>
    /// </summary>
    /// <param name="x">
    /// A <see cref="Fraction"/>
    /// </param>
    /// <param name="y">
    /// A <see cref="Fraction"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public static bool operator != (Fraction x, Fraction y)
    {
      return !(x == y);
    }
    
    /// <summary>
    /// <para>Operator overload for multiplying fractions together.</para>
    /// </summary>
    /// <param name="x">
    /// A <see cref="Fraction"/>
    /// </param>
    /// <param name="y">
    /// A <see cref="Fraction"/>
    /// </param>
    /// <returns>
    /// A <see cref="Fraction"/>
    /// </returns>
    public static Fraction operator * (Fraction x, Fraction y)
    {
      return new Fraction(x.Numerator * y.Numerator, x.Denominator * y.Denominator).Simplify();
    }
    
    /// <summary>
    /// <para>Operator overload for dividing fractions by one another.</para>
    /// </summary>
    /// <param name="x">
    /// A <see cref="Fraction"/>
    /// </param>
    /// <param name="y">
    /// A <see cref="Fraction"/>
    /// </param>
    /// <returns>
    /// A <see cref="Fraction"/>
    /// </returns>
    public static Fraction operator / (Fraction x, Fraction y)
    {
      return new Fraction(x.Numerator * y.Denominator, x.Denominator * y.Numerator).Simplify();
    }
    
    /// <summary>
    /// <para>Operator overload for adding two fractions together.</para>
    /// </summary>
    /// <param name="x">
    /// A <see cref="Fraction"/>
    /// </param>
    /// <param name="y">
    /// A <see cref="Fraction"/>
    /// </param>
    /// <returns>
    /// A <see cref="Fraction"/>
    /// </returns>
    public static Fraction operator + (Fraction x, Fraction y)
    {
      return new Fraction((x.Numerator * y.Denominator) + (y.Numerator * x.Denominator),
                          x.Denominator * y.Denominator).Simplify();
    }
    
    /// <summary>
    /// <para>Operator overload for subtracting one fraction from another.</para>
    /// </summary>
    /// <param name="x">
    /// A <see cref="Fraction"/>
    /// </param>
    /// <param name="y">
    /// A <see cref="Fraction"/>
    /// </param>
    /// <returns>
    /// A <see cref="Fraction"/>
    /// </returns>
    public static Fraction operator - (Fraction x, Fraction y)
    {
      return new Fraction((x.Numerator * y.Denominator) - (y.Numerator * x.Denominator),
                          x.Denominator * y.Denominator).Simplify();
    }
    
    #endregion
  }
}

