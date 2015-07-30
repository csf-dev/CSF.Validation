//
//  DateTimeExtensions.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2012 Craig Fowler
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
      // TODO: Write this implementation
      throw new NotImplementedException();
    }
  }
}

