//
//  StringExtensions.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2013 Craig Fowler
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
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;

namespace CSF
{
  /// <summary>
  /// Extension methods for the string type.
  /// </summary>
  public static class StringExtensions
  {
    #region constants

    private const string FirstCharacterPattern = @"\b(\w)";
    private static readonly Regex FirstCharacterFinder = new Regex(FirstCharacterPattern, RegexOptions.Compiled);

    #endregion

    #region extension methods

    /// <summary>
    /// Capitalizes the specified string.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The definition of this operation is to firstly convert the entire string to lowercase.  Then, the first
    /// character of every word is replaced with its uppercase equivalent.
    /// </para>
    /// </remarks>
    /// <param name='value'>
    /// The string to be capitalized.
    /// </param>
    public static string Capitalize(this string value)
    {
      return value.Capitalize(CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Capitalizes the specified string.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The definition of this operation is to firstly convert the entire string to lowercase.  Then, the first
    /// character of every word is replaced with its uppercase equivalent.
    /// </para>
    /// </remarks>
    /// <param name='value'>
    /// The string to be capitalized.
    /// </param>
    /// <param name='culture'>
    /// The culture to use for any lowercase/uppercase transformations.
    /// </param>
    public static string Capitalize(this string value, CultureInfo culture)
    {
      if(value == null)
      {
        throw new ArgumentNullException("value");
      }
      if(culture == null)
      {
        throw new ArgumentNullException("culture");
      }

      string lowercaseVersion = value.ToLower(culture);
      StringBuilder output = new StringBuilder(lowercaseVersion);
      var matches = FirstCharacterFinder.Matches(lowercaseVersion);

      foreach(Match match in matches)
      {
        output.Replace(match.Value, match.Value.ToUpper(culture), match.Index, 1);
      }

      return output.ToString();
    }

    /// <summary>
    /// Capitalizes the specified string using the invariant culture.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The definition of this operation is to firstly convert the entire string to lowercase.  Then, the first
    /// character of every word is replaced with its uppercase equivalent.
    /// </para>
    /// </remarks>
    /// <param name='value'>
    /// The string to be capitalized.
    /// </param>
    public static string CapitalizeInvariant(this string value)
    {
      return value.Capitalize(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Parses the string as an enumeration member.
    /// </summary>
    /// <returns>
    /// The enumeration member represented by the <paramref name="value"/>.
    /// </returns>
    /// <param name='value'>
    /// The string value to parse.
    /// </param>
    /// <typeparam name='TEnum'>
    /// The target enumeration type.
    /// </typeparam>
    /// <exception cref="ArgumentNullException">
    /// If the <paramref name="value"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If the <paramref name="value"/> does not represent an enumeration constant of the target type.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// If the target type is not an enumeration type.
    /// </exception>
    public static TEnum ParseAs<TEnum>(this string value) where TEnum : struct
    {
      return value.ParseAs<TEnum>(false);
    }

    /// <summary>
    /// Parses the string as an enumeration member.
    /// </summary>
    /// <returns>
    /// The enumeration member represented by the <paramref name="value"/>.
    /// </returns>
    /// <param name='value'>
    /// The string value to parse.
    /// </param>
    /// <param name='ignoreCase'>
    /// A value that indicates whether the case of the <paramref name="value"/> should be ignored or not.
    /// </param>
    /// <typeparam name='TEnum'>
    /// The target enumeration type.
    /// </typeparam>
    /// <exception cref="ArgumentNullException">
    /// If the <paramref name="value"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If the <paramref name="value"/> does not represent an enumeration constant of the target type.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// If the target type is not an enumeration type.
    /// </exception>
    public static TEnum ParseAs<TEnum>(this string value, bool ignoreCase) where TEnum : struct
    {
      if(value == null)
      {
        throw new ArgumentNullException("value");
      }

      Type enumType = typeof(TEnum);

      if(!enumType.IsEnum)
      {
        throw new InvalidOperationException("The target type to parse-as must be an enumeration type.");
      }

      return (TEnum) Enum.Parse(typeof(TEnum), value, ignoreCase);
    }

    /// <summary>
    /// Attempts to parse the string as an enumeration member, returning a null reference if parsing fails.
    /// </summary>
    /// <returns>
    /// The enumeration member represented by the <paramref name="value"/>, or a null reference.
    /// </returns>
    /// <param name='value'>
    /// The string value to parse.
    /// </param>
    /// <typeparam name='TEnum'>
    /// The target enumeration type.
    /// </typeparam>
    /// <exception cref="InvalidOperationException">
    /// If the target type is not an enumeration type.
    /// </exception>
    public static TEnum? TryParseAs<TEnum>(this string value) where TEnum : struct
    {
      return value.TryParseAs<TEnum>(false);
    }

    /// <summary>
    /// Attempts to parse the string as an enumeration member, returning a null reference if parsing fails.
    /// </summary>
    /// <returns>
    /// The enumeration member represented by the <paramref name="value"/>, or a null reference.
    /// </returns>
    /// <param name='value'>
    /// The string value to parse.
    /// </param>
    /// <param name='ignoreCase'>
    /// A value that indicates whether the case of the <paramref name="value"/> should be ignored or not.
    /// </param>
    /// <typeparam name='TEnum'>
    /// The target enumeration type.
    /// </typeparam>
    /// <exception cref="InvalidOperationException">
    /// If the target type is not an enumeration type.
    /// </exception>
    public static TEnum? TryParseAs<TEnum>(this string value, bool ignoreCase) where TEnum : struct
    {
      TEnum? output;

      try
      {
        output = value.ParseAs<TEnum>(ignoreCase);
      }
      catch(ArgumentException)
      {
        output = null;
      }

      return output;
    }

    #endregion
  }
}

