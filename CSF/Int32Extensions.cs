//
// Int32Extensions.cs
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
using System.Text;
using System.Collections.Generic;

namespace CSF
{
  /// <summary>
  /// <para>Provider for extension methods relating to <see cref="System.Int32"/>.</para>
  /// </summary>
  public static class Int32Extensions
  {
    #region constants
    
    private const string
      ALPHABET                                    = "abcdefghijklmnopqrstuvwxyz";
    
    private const char
      NEGATIVE_SYMBOL                             = '-';
    
    private const bool
      DEFAULT_ZERO_BASED_ALPHABETIC_REFERENCES    = true;
    
    private static readonly int ALPHABET_LENGTH   = ALPHABET.Length;
    
    #endregion
    
    #region public static methods
    
    /// <summary>
    /// <para>
    /// Overloaded.  Creates a lowercase alphabetic reference for the given <see cref="System.Int32"/> using a
    /// zero-based generation strategy.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method creates an alphabetic reference from an integer.  This is essentially a base-26 representation of
    /// the integer.
    /// </para>
    /// <para>
    /// There is one caveat, which is that the alphabet has no character to represent zero.  This means that (using
    /// the default strategy used by this overload) <c>a</c> represents zero, <c>aa</c> represents 25 and so on.  If
    /// non-zero-based operation is required then you should instead look to
    /// <c>GenerateAlphabeticReference(int, bool)</c> in order to make use of the alternative strategy.
    /// </para>
    /// </remarks>
    /// <param name="integerValue">
    /// A <see cref="System.Int32"/> to represent as an alphabetic string.
    /// </param>
    /// <returns>
    /// A <see cref="System.String"/>, the generated alphabetic reference.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// <para>If <paramref name="integerValue"/> is less than zero.</para>
    /// </exception>
    public static string GenerateAlphabeticReference(int integerValue)
    {
      return GenerateAlphabeticReference(integerValue, DEFAULT_ZERO_BASED_ALPHABETIC_REFERENCES);
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Creates a lowercase alphabetic reference for the given <see cref="System.Int32"/> using the
    /// generation strategy specified by <paramref name="zeroBased"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method creates an alphabetic reference from an integer.  This is essentially a base-26 representation of
    /// the integer.
    /// </para>
    /// <para>
    /// This method offers two distinct strategies for generating the alphabetic reference.  The strategy used is
    /// controlled by the value of <paramref name="zeroBased"/>:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Strategy</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>Non-zero-based</term>
    /// <description>
    /// The generated alphabetic reference is based at one.  For example: <c>a</c> represents one, <c>c</c> represents
    /// three and so on.  Non-zero-based generation supports the generation of references that represent negative
    /// numbers (the minus symbol is prepended to the generated result).  The drawback of this approach is that the
    /// value of zero is represented by <see cref="String.Empty"/>.
    /// </description>
    /// </item>
    /// <item>
    /// <term>Zero-based</term>
    /// <description>
    /// The generated alphabetic reference is based at zero.  For example: <c>a</c> represents zero, <c>c</c>
    /// represents two and so on.  Zero-based generation does not support the generation of reference that represent
    /// negative numbers (attempting this will result in an exception being raised).  Zero-based generation does
    /// however ensure that the output will never be an empty string.
    /// </description>
    /// </item>
    /// </list>
    /// <example>
    /// <para>
    /// For example, using the zero-based strategy then the input <c>500</c> will result in the alphabetic reference
    /// <c>tg</c>.  The workings for this are shown below:
    /// <code>
    /// "g" is the 6th character in sequence (zero-based).
    /// If "a" is zero then at the zero'th character position (right-to-left) "g" represents:  6×(26^0)  = 6×1   = 6
    /// 
    /// "t" is the 19th character in sequence (zero-based).
    /// If "a" is zero then at the first character position (right-to-left) "t" represents:    19×(26^1) = 19×26 = 494
    /// 
    /// The final result is 494 + 6 = 500.
    /// </code>
    /// </para>
    /// <para>
    /// Using the non-zero-based strategy then the input <c>500</c> will result in the alphabetic reference <c>sf</c>.
    /// The workings for this are shown below:
    /// <code>
    /// "f" is the 6th character in sequence (one-based).
    /// If "a" is one then at the zero'th character position (right-to-left) "f" represents:   6×(26^0)  = 6×1   = 6
    /// 
    /// "s" is the 19th character in sequence (one-based).
    /// If "a" is one then at the first character position (right-to-left) "s" represents:     19×(26^1) = 19×26 = 494
    /// 
    /// The final result is 494 + 6 = 500.
    /// </code>
    /// </para>
    /// </example>
    /// <para>An example of the </para>
    /// </remarks>
    /// <param name="integerValue">
    /// A <see cref="System.Int32"/> to represent as an alphabetic string.
    /// </param>
    /// <param name="zeroBased">
    /// A <see cref="System.Boolean"/> that indicates the generation strategy to use.  If this is true then a
    /// zero-based strategy will be used, if false then a non-zero-based strategy will be used.
    /// </param>
    /// <returns>
    /// A <see cref="System.String"/>, the generated alphabetic reference.  This will equal <see cref="String.Empty"/>
    /// in the scenario that both <paramref name="zeroBased"/> is false and the <paramref name="integerValue"/> is
    /// zero.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// <para>
    /// If both <paramref name="zeroBased"/> is true and <paramref name="integerValue"/> is less than zero, see the
    /// remarks for more information.
    /// </para>
    /// </exception>
    public static string GenerateAlphabeticReference(int integerValue, bool zeroBased)
    {
      StringBuilder output = new StringBuilder();
      
      if(!zeroBased && integerValue == 0)
      {
        /* In the special case that we are not returning a zero-based reference, we return an empty string in order
         * to represent zero.
         */
        output.Append(String.Empty);
      }
      else if(zeroBased && integerValue < 0)
      {
        string exMessage = "Creating alphabetic references for negative integers is not supported when the " +
                           "reference is to be zero-based.";
        NotSupportedException ex = new NotSupportedException(exMessage);
        ex.Data["Integer value"] = integerValue;
        throw ex;
      }
      else
      {
        int targetValue;
        
        // Reformat the integer value if needed, unifying it for use with the private 'worker' method.
        targetValue = Math.Abs(integerValue);
        
        // Use the private 'worker' method to generate the actual result.
        GenerateAlphabeticReference(targetValue, 0, ref output, zeroBased);
        
        /* Prepend the minus/negative symbol if appropriate - the private 'worker' method only uses the absolute
         * (non-negative) value of the target, so the notation indicating a negative result is being done here.
         */
        output.Insert(0, ((integerValue < 0)? NEGATIVE_SYMBOL.ToString() : String.Empty));
      }
      
      return output.ToString();
    }
    
    /// <summary>
    /// Parses a <see cref="System.String"/> alphabetic reference to a number as a <see cref="System.Int32"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is essentially the inverse of <c>GenerateAlphabeticReference(int)</c>, see the documentation
    /// for that method for more information on how these references work.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The <see cref="System.Int32"/> representation of the alphabetic reference.
    /// </returns>
    /// <param name='reference'>
    /// The alphabetic reference to parse
    /// </param>
    public static int ParseAlphabeticReference(string reference)
    {
      return ParseAlphabeticReference(reference, DEFAULT_ZERO_BASED_ALPHABETIC_REFERENCES);
    }
    
    /// <summary>
    /// Parses a <see cref="System.String"/> alphabetic reference to a number as a <see cref="System.Int32"/>.
    /// </summary>
    /// <returns>
    /// The <see cref="System.Int32"/> representation of the alphabetic reference.
    /// </returns>
    /// <param name='reference'>
    /// The alphabetic reference to parse
    /// </param>
    /// <param name='zeroBased'>
    /// Whether or not to use the zero-based method for parsing the reference or not.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='FormatException'>
    /// Represents errors caused by passing incorrectly formatted arguments or invalid format specifiers to methods.
    /// </exception>
    public static int ParseAlphabeticReference(string reference, bool zeroBased)
    {
      int
        power,
        characterIndex,
        output = 0,
        multiple;
      bool negativeOutput = false;
      
      // Sanity-check the input
      if(reference == null)
      {
        throw new ArgumentNullException("reference");
      }
      else if(zeroBased && reference.Length == 0)
      {
        throw new FormatException("An empty string may represent zero in non-zero-based scenarios but it is not " +
                                  "permitted in zero-based scenarios.");
      }
      
      // Loop through the characters in the reference string from the right-most characters to the left
      for(int characterPosition = reference.Length - 1; characterPosition >= 0; characterPosition--)
      {
        power = (reference.Length - 1) - characterPosition;
        characterIndex = ALPHABET.IndexOf(reference[characterPosition]);
        
        /* If the character was not found in the alphabet (has index less than zero) then we might be able to proceed
         * and detect a negative symbol if:
         * 
         * * We are not in zero-based mode
         * * We are at character position zero (the left-most character)
         * * The character found is equal to the negative symbol
         * 
         * In this case we store that we have found a negative symbol so that we can invert the output later on.
         */
        if(characterIndex < 0 &&
           !zeroBased &&
           characterPosition == 0 &&
           reference[characterPosition] == NEGATIVE_SYMBOL)
        {
          negativeOutput = true;
        }
        else if(characterIndex < 0)
        {
          /* If the character was not found but it does not meet the criteria for a negative symbol then throw an
           * exception.
           */
          FormatException ex = new FormatException("Alphabetic reference does not conform to the required format.");
          ex.Data["Reference string"] = reference;
          throw ex;
        }
        else
        {
          /* Otherwise, in all normal circumstances we can incremenent the output by the one-based index of the
           * character in the alphabet, multiplied by an appropriate power of the alphabet's length.
           */
          multiple = characterIndex + ((zeroBased && characterPosition == reference.Length - 1)? 0 : 1);
          output += (int) (multiple * (int) Math.Pow(ALPHABET_LENGTH, power));
        }
      }
      
      // If we have detected a negative symbol then invert the output before returning it.
      if(negativeOutput)
      {
        output = 0 - output;
      }
      
      return output;
    }
    
    /// <summary>
    /// Gets a collection of the decimal bit numbers that would represent the binary bits that make up the binary
    /// representation of the input.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method returns the input <paramref name="integerValue"/> broken down into a collection of
    /// <see cref="System.Int32"/> values which - when added together - would equal the original input value.  The
    /// output values are all decimal representations of binary bit numbers (IE: powers of two).
    /// </para>
    /// <example>
    /// If the value <c>45</c> were passed into this method, the output would be the collection of integers:
    /// <c>{ 1, 4, 8, 32 }</c>.  These correspond to the bits <c>101101</c> (which is the binary representation of
    /// <c>45</c>).
    /// </example>
    /// <para>
    /// If the input to this method is negative then all of the output bits will also be negative.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The bit numbers.
    /// </returns>
    /// <param name='integerValue'>
    /// The value for which to get the bit numbers.
    /// </param>
    public static IList<int> GetBitNumbers(int integerValue)
    {
      int bitNumber = 0, remainingValue = integerValue, currentBit;
      IList<int> output = new List<int>();
      
      while(remainingValue != 0)
      {
        currentBit = ((remainingValue > 0)? 1 : -1) << bitNumber++;
        
        if((Math.Abs(remainingValue) & Math.Abs(currentBit)) == Math.Abs(currentBit))
        {
          output.Add(currentBit);
          remainingValue -= currentBit;
        }
      }
      
      return output;
    }
    
    #endregion
    
    #region private methods
    
    /// <summary>
    /// <para>
    /// Overloaded.  Private 'worker' method creates a lowercase alphabetic reference for the given
    /// <see cref="System.Int32"/>, starting from the given <paramref name="currentCharacterPosition"/>.  The resulting
    /// reference is written to a <see cref="StringBuilder"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is recursive within itself; it will recurse as many times as required, incrementing
    /// <paramref name="currentCharacterPosition"/> by one with each iteration until no more iterations are required.
    /// Each iteration prepends a new character to the <paramref name="output"/> and one iteration is completed for
    /// each power of 26 (the length of the alphabet) that the the <paramref name="targetValue"/> divides into.
    /// </para>
    /// <para>
    /// The <paramref name="currentCharacterPosition"/> indicates the position of the current character that is being
    /// generated.  This is zero based and reads from right-to-left (from the least significant character to the most).
    /// <example>
    /// <code>
    /// abc
    ///   ^ this is character position zero
    /// </code>
    /// </example>
    /// <example>
    /// <code>
    /// abc
    /// ^   this is character position two
    /// </code>
    /// </example>
    /// </para>
    /// </remarks>
    /// <param name="targetValue">
    /// A <see cref="System.Int32"/> to represent as an alphabetic string.
    /// </param>
    /// <param name="currentCharacterPosition">
    /// A <see cref="System.Int32"/> indicating the position of the character that this iteration should generate.
    /// This value is zero-based and indicates a character reading from right-to-left.
    /// </param>
    /// <param name="output">
    /// A <see cref="StringBuilder"/> that the output is written to.
    /// </param>
    /// <param name="zeroBased">
    /// A <see cref="System.Boolean"/> that indicates whether or not zero-based character references should be used
    /// or not.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para>Either:</para>
    /// <list type="bullet">
    /// <item>The <paramref name="targetValue"/> is less than zero.</item>
    /// <item><paramref name="zeroBased"/> is false and the <paramref name="targetValue"/> is less than one.</item>
    /// <item>The <paramref name="currentCharacterPosition"/> is less than zero.</item>
    /// </list>
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <para>The <paramref name="output"/> is null.</para>
    /// </exception>
    private static void GenerateAlphabeticReference(int targetValue,
                                                    int currentCharacterPosition,
                                                    ref StringBuilder output,
                                                    bool zeroBased)
    {
      int truncatedComponent, valueComponent, characterIndex;
      
      // Just some sanity checking
      if(targetValue < 0)
      {
        string message = "Integer value to represent may not be less than zero.";
        ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("integerValue", message);
        ex.Data["Integer value"] = targetValue;
        throw ex;
      }
      else if(!zeroBased && targetValue < 1)
      {
        string message = "Integer value to represent may not be less than one when performing non-zero-based " +
                         "generation.";
        ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("integerValue", message);
        ex.Data["Integer value"] = targetValue;
        throw ex;
      }
      else if(currentCharacterPosition < 0)
      {
        string message = "Current character position (right-to-left, zero based) cannot be less than zero.";
        ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("currentCharacterPosition", message);
        ex.Data["Character position"] = currentCharacterPosition;
        throw ex;
      }
      else if(output == null)
      {
        throw new ArgumentNullException("output");
      }
      
      /* The value component is the portion of the target value that is significant to generating the character at
       * the current character position.
       * 
       * To do this we divide [the target value] by ([the length of the alphabet] to the power of
       * [the current character position]) and then discard/truncate any digits after the decimal point.  This discards
       * any information that has already been used on a previous run of this method (remember that this method is
       * recursive!).
       * 
       * After discarding any unwanted information on the right-hand side of the interesting information, we discard
       * information on the left hand side (information that we will deal with in 
       */
      truncatedComponent = (int) Math.Truncate((double) targetValue / Math.Pow(ALPHABET_LENGTH,
                                                                               currentCharacterPosition));
      valueComponent = truncatedComponent % ALPHABET_LENGTH;
      characterIndex = valueComponent - 1;
      
      if(zeroBased && currentCharacterPosition == 0)
      {
        characterIndex ++;
      }
      else if(!zeroBased && output.Length > 0 && output[0] == ALPHABET[ALPHABET_LENGTH - 1])
      {
        characterIndex --;
      }
      
      if(characterIndex < 0)
      {
        characterIndex = ALPHABET_LENGTH - 1;
      }
      
      output.Insert(0, ALPHABET[characterIndex]);
      
      
      /* If the truncated value was more than or equal to the alphabet length then there is at least one more character
       * to add to the output, in which case we should recurse into ourself, incrementing the current character index.
       */
      if(truncatedComponent > ALPHABET_LENGTH)
      {
        GenerateAlphabeticReference(targetValue, ++currentCharacterPosition, ref output, zeroBased);
      }
      else if(zeroBased && truncatedComponent == ALPHABET_LENGTH)
      {
        GenerateAlphabeticReference(targetValue, ++currentCharacterPosition, ref output, zeroBased);
      }
    }
    
    #endregion
    
    #region extension methods
    
    /// <summary>
    /// <para>
    /// Overloaded.  Creates a lowercase alphabetic reference for the given <see cref="System.Int32"/> using a
    /// zero-based generation strategy.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method creates an alphabetic reference from an integer.  This is essentially a base-26 representation of
    /// the integer.
    /// </para>
    /// <para>
    /// There is one caveat, which is that the alphabet has no character to represent zero.  This means that (using
    /// the default strategy used by this overload) <c>a</c> represents zero, <c>aa</c> represents 25 and so on.  If
    /// non-zero-based operation is required then you should instead look to
    /// <c>GenerateAlphabeticReference(int, bool)</c> in order to make use of the alternative strategy.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A <see cref="System.String"/>, the generated alphabetic reference.
    /// </returns>
    /// <param name='number'>
    /// The number to turn into an alphabetic reference.
    /// </param>
    public static string ToAlphabeticReference(this int number)
    {
      return GenerateAlphabeticReference(number);
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Creates a lowercase alphabetic reference for the given <see cref="System.Int32"/> using the
    /// generation strategy specified by <paramref name="zeroBased"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method creates an alphabetic reference from an integer.  This is essentially a base-26 representation of
    /// the integer.
    /// </para>
    /// <para>
    /// This method offers two distinct strategies for generating the alphabetic reference.  The strategy used is
    /// controlled by the value of <paramref name="zeroBased"/>:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Strategy</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>Non-zero-based</term>
    /// <description>
    /// The generated alphabetic reference is based at one.  For example: <c>a</c> represents one, <c>c</c> represents
    /// three and so on.  Non-zero-based generation supports the generation of references that represent negative
    /// numbers (the minus symbol is prepended to the generated result).  The drawback of this approach is that the
    /// value of zero is represented by <see cref="String.Empty"/>.
    /// </description>
    /// </item>
    /// <item>
    /// <term>Zero-based</term>
    /// <description>
    /// The generated alphabetic reference is based at zero.  For example: <c>a</c> represents zero, <c>c</c>
    /// represents two and so on.  Zero-based generation does not support the generation of reference that represent
    /// negative numbers (attempting this will result in an exception being raised).  Zero-based generation does
    /// however ensure that the output will never be an empty string.
    /// </description>
    /// </item>
    /// </list>
    /// <example>
    /// <para>
    /// For example, using the zero-based strategy then the input <c>500</c> will result in the alphabetic reference
    /// <c>tg</c>.  The workings for this are shown below:
    /// <code>
    /// "g" is the 6th character in sequence (zero-based).
    /// If "a" is zero then at the zero'th character position (right-to-left) "g" represents:  6×(26^0)  = 6×1   = 6
    /// 
    /// "t" is the 19th character in sequence (zero-based).
    /// If "a" is zero then at the first character position (right-to-left) "t" represents:    19×(26^1) = 19×26 = 494
    /// 
    /// The final result is 494 + 6 = 500.
    /// </code>
    /// </para>
    /// <para>
    /// Using the non-zero-based strategy then the input <c>500</c> will result in the alphabetic reference <c>sf</c>.
    /// The workings for this are shown below:
    /// <code>
    /// "f" is the 6th character in sequence (one-based).
    /// If "a" is one then at the zero'th character position (right-to-left) "f" represents:   6×(26^0)  = 6×1   = 6
    /// 
    /// "s" is the 19th character in sequence (one-based).
    /// If "a" is one then at the first character position (right-to-left) "s" represents:     19×(26^1) = 19×26 = 494
    /// 
    /// The final result is 494 + 6 = 500.
    /// </code>
    /// </para>
    /// </example>
    /// <para>An example of the </para>
    /// </remarks>
    /// <param name="number">
    /// A <see cref="System.Int32"/> to represent as an alphabetic string.
    /// </param>
    /// <param name="zeroBased">
    /// A <see cref="System.Boolean"/> that indicates the generation strategy to use.  If this is true then a
    /// zero-based strategy will be used, if false then a non-zero-based strategy will be used.
    /// </param>
    /// <returns>
    /// A <see cref="System.String"/>, the generated alphabetic reference.  This will equal <see cref="String.Empty"/>
    /// in the scenario that both <paramref name="zeroBased"/> is false and the <paramref name="integerValue"/> is
    /// zero.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// <para>
    /// If both <paramref name="zeroBased"/> is true and <paramref name="integerValue"/> is less than zero, see the
    /// remarks for more information.
    /// </para>
    /// </exception>
    public static string ToAlphabeticReference(this int number, bool zeroBased)
    {
      return GenerateAlphabeticReference(number, zeroBased);
    }
    
    /// <summary>
    /// Gets a collection of the decimal bit numbers that would represent the binary bits that make up the binary
    /// representation of the input.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method returns the input <paramref name="number"/> broken down into a collection of
    /// <see cref="System.Int32"/> values which - when added together - would equal the original input value.  The
    /// output values are all decimal representations of binary bit numbers (IE: powers of two).
    /// </para>
    /// <example>
    /// If the value <c>45</c> were passed into this method, the output would be the collection of integers:
    /// <c>{ 1, 4, 8, 32 }</c>.  These correspond to the bits <c>101101</c> (which is the binary representation of
    /// <c>45</c>).
    /// </example>
    /// <para>
    /// If the input to this method is negative then all of the output bits will also be negative.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The bit numbers.
    /// </returns>
    /// <param name='number'>
    /// The value for which to get the bit numbers.
    /// </param>
    public static IList<int> ToBitNumbers(this int number)
    {
      return GetBitNumbers(number);
    }
    
    #endregion
  }
}

