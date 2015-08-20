//
// PrimeFactoriser.cs
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
using System.Linq;

namespace CSF
{
  /// <summary>
  /// <para>A utility type for the generation of prime numbers and factorising numbers.</para>
  /// </summary>
  /// <remarks>
  /// <para>
  /// This type is highly (almost completely) based on the excellent work found at
  /// http://handcraftsman.wordpress.com/2010/09/02/ienumerable-of-prime-numbers-in-csharp/ and
  /// http://handcraftsman.wordpress.com/2010/09/02/prime-factorization-in-csharp/
  /// </para>
  /// </remarks>
  public class PrimeFactoriser
  {
    #region methods

    /// <summary>
    /// <para>Gets a collection of the prime factors that are common between two numbers.</para>
    /// </summary>
    /// <param name="firstNumber">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <param name="secondNumber">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <returns>
    /// A collection of <see cref="System.Int32"/>
    /// </returns>
    public IEnumerable<int> GetCommonPrimeFactors(int firstNumber, int secondNumber)
    {
      List<int>
        firstPrimeFactors = this.GetPrimeFactors(firstNumber).ToList(),
        secondPrimeFactors = this.GetPrimeFactors(secondNumber).ToList(),
        output = new List<int>();
      
      for(int i = 0; i < firstPrimeFactors.Count; i++)
      {
        int
          current = firstPrimeFactors[i],
          currentIndex = secondPrimeFactors.IndexOf(current);
        
        if(currentIndex >= 0)
        {
          output.Add(current);
          secondPrimeFactors.RemoveAt(currentIndex);
        }
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Gets a collection of the prime factors of a number.</para>
    /// </summary>
    /// <param name="number">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <returns>
    /// A collection of <see cref="System.Int32"/>
    /// </returns>
    public IEnumerable<int> GetPrimeFactors(int number)
    {
      /* A future plan for this method would be to cache the primes that this instance has already generated.
       * Then, future calls will use the cache as much as possible rather than generating lots more prime numbers.
       */
      int first = this.GeneratePrimes(number)
                  .TakeWhile(x => x <= Math.Sqrt(number))
                  .FirstOrDefault(x => number % x == 0);
      
      return (first == 0)? new[] { number } : new[] { first }.Concat(this.GetPrimeFactors(number / first));
    }
    
    /// <summary>
    /// <para>Gets a collection of the prime numbers up to a given ceiling.</para>
    /// </summary>
    /// <param name="upTo">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <returns>
    /// A collection of <see cref="System.Int32"/>
    /// </returns>
    public IEnumerable<int> GeneratePrimes(int upTo)
    {
      /* A future plan for this method would be to have a startAt parameter as well, which only starts generating prime
       * numbers from the starting point.
       */
      List<int> cachedPrimes = new List<int>();
      int sqrt = 1;
      var primes = this.PotentialPrimes(upTo).Where (x =>
      {
        sqrt = this.GetSqrtCeiling(x, sqrt);
        return !cachedPrimes.TakeWhile (y => y <= sqrt).Any (y => x % y == 0);
      });
      
      foreach (int prime in primes) {
        yield return prime;
        cachedPrimes.Add (prime);
      }
    }

    private int GetSqrtCeiling (int value, int start)
    {
      while (start * start < value) {
        start++;
      }
      return start;
    }

    private IEnumerable<int> PotentialPrimes(int upTo)
    {
      yield return 2;
      yield return 3;
      int k = 1;
      while (k > 0 && (6 * k - 1) <= upTo) {
        yield return 6 * k - 1;
        yield return 6 * k + 1;
        k++;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// <para>Initialises the current instance.</para>
    /// </summary>
    private PrimeFactoriser () {}

    /// <summary>
    /// <para>Initialises the singleton <see cref="Default"/> generator.</para>
    /// </summary>
    static PrimeFactoriser ()
    {
      Default = new PrimeFactoriser ();
    }

    #endregion

    #region static properties

    /// <summary>
    /// <para>Read-only.  Gets a singleton instance of <see cref="PrimeFactoriser"/>.</para>
    /// </summary>
    public static PrimeFactoriser Default { get; private set; }
    
    #endregion
  }
}

