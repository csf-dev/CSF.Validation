//
// DateTimeExtensions.cs
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
  /// Extension methods for the <see cref="System.DateTime"/> object.
  /// </summary>
  public static class DateTimeExtensions
  {
    /// <summary>
    /// Returns a value that represents the current instance as an age in years, using the current date as the
    /// 'reference point' (IE: the age in years, as of today).
    /// </summary>
    /// <returns>
    /// The age in years.
    /// </returns>
    /// <param name='birthDate'>
    /// A birth date
    /// </param>
    public static int AsAgeInYears(this DateTime birthDate)
    {
      return birthDate.AsAgeInYears(DateTime.Today);
    }

    /// <summary>
    /// Returns a value that represents the current instance as an age in years, using an arbitrary reference date
    /// (IE: the age in years, as of the reference date).
    /// </summary>
    /// <returns>
    /// The age in years.
    /// </returns>
    /// <param name='birthDate'>
    /// A birth date
    /// </param>
    /// <param name='atDate'>
    /// The reference date
    /// </param>
    public static int AsAgeInYears(this DateTime birthDate, DateTime atDate)
    {
      if(atDate < birthDate)
      {
        throw new ArgumentOutOfRangeException("atDate", "Reference date must not be before the birth date.");
      }

      bool hasHadBirthdayAlready;
      int previousYears, thisYear;
        
      /* Every year of difference in 'previous' years is definitely part of their age.
       * 
       * EG:  If they born in 1980 and the current year is 2010 then no matter what today's date is, they are at least
       * 29 years old already. We would need to determine whether they have had their birthday this year or not to
       * know if they are 30.
       */
      previousYears = (atDate.Year - birthDate.Year) - 1;
        
      // Credit them with an additional year of age if they have already had their birthday for the 'last' year
      if(atDate.Month > birthDate.Month)
      {
        hasHadBirthdayAlready = true;
      }
      else if(atDate.Month == birthDate.Month
              && atDate.Day >= birthDate.Day)
      {
        hasHadBirthdayAlready = true;
      }
      else
      {
        hasHadBirthdayAlready = false;
      }

      thisYear = hasHadBirthdayAlready? 1 : 0;
      
      return previousYears + thisYear;

    }

    /// <summary>
    /// Gets a <c>System.DateTime</c> which represents midnight on the last day of the same month represented by the
    /// given instance.
    /// </summary>
    /// <returns>A DateTime representing the last day of the month.</returns>
    /// <param name="date">Date.</param>
    public static DateTime GetLastDayOfMonth(this DateTime date)
    {
      var firstDay = new DateTime(date.Year, date.Month, 1);
      return firstDay.AddMonths(1).AddDays(-1);
    }
  }
}

