//
// CultureInfoExtensions.cs
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
using System.Globalization;

namespace CSF.Globalization
{
  /// <summary>
  /// Extension methods for a calendar object.
  /// </summary>
  public static class CultureInfoExtensions
  {
    /// <summary>
    /// Gets all of the months in the year and their names.
    /// </summary>
    /// <returns>
    /// The months in the year.
    /// </returns>
    /// <param name='culture'>
    /// Culture information, relating to which culture to return this information in.
    /// </param>
    public static IDictionary<int, string> GetAllMonths(this CultureInfo culture)
    {
      return culture.GetAllMonths(DateTime.Today.Year);
    }

    /// <summary>
    /// Gets all of the months in the year and their names.
    /// </summary>
    /// <returns>
    /// The months in the year.
    /// </returns>
    /// <param name='culture'>
    /// Culture information, relating to which culture to return this information in.
    /// </param>
    /// <param name='year'>
    /// The year for which we want the months data.
    /// </param>
    public static IDictionary<int, string> GetAllMonths(this CultureInfo culture, int year)
    {
      IDictionary<int, string> output = new Dictionary<int, string>();

      if(culture == null)
      {
        throw new ArgumentNullException("culture");
      }

      int monthsInYear = culture.Calendar.GetMonthsInYear(year);
      for(int month = 1; month <= monthsInYear; month++)
      {
        output.Add(month, culture.DateTimeFormat.GetMonthName(month));
      }

      return output;
    }
  }
}

