//
//  EnumExtensions.cs
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
using CSF.Reflection;
using System.Reflection;

namespace CSF
{
  /// <summary>
  /// Type containing extension methods that are useful to enumerated types.
  /// </summary>
  /// <remarks>
  /// <para>
  /// These extension methods are somewhat general for enumerations and do not live in any special namespace.
  /// </para>
  /// </remarks>
  public static class EnumExtensions
  {
    #region extension methods

    /// <summary>
    /// Determines whether the given enumeration value is a defined value of its parent enumeration.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the given value is a defined value of its associated enumeration; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='value'>
    /// The enumeration value to analyse.
    /// </param>
    public static bool IsDefinedValue(this Enum value)
    {
      return IsDefined(value);
    }

    /// <summary>
    /// Determines whether the enumeration <paramref name="value"/> contains a given <paramref name="flagValue"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is only useable on enumerations decorated with <c>System.FlagsAttribute</c>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// <c>true</c> if the <paramref name="value"/> contains the <paramref name="flagValue"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='value'>
    /// The enumeration value to analyse.
    /// </param>
    /// <param name='flagValue'>
    /// An enumeration value to search for within the value.
    /// </param>
    public static bool HasFlag(this Enum value, Enum flagValue)
    {
      if(value == null)
      {
        throw new ArgumentNullException("value");
      }
      else if(flagValue == null)
      {
        throw new ArgumentNullException("flagValue");
      }

      Type enumType = value.GetType();
      Type flagValueType = flagValue.GetType();

      if(!enumType.HasAttribute<FlagsAttribute>())
      {
        throw new ArgumentException("The given enumeration type must be decorated with FlagsAttribute.", "value");
      }
      else if(enumType != flagValueType)
      {
        throw new ArgumentException("The flag-value to check for must be of the same enumeration type as the " +
                                    "enumeration value to analyse.",
                                    "flagValue");
      }

      Type underlyingType = Enum.GetUnderlyingType(enumType);

      if(underlyingType != typeof(int))
      {
        throw new ArgumentException("The underlying type of flags-based enumerations to compare must be System.Int32," +
                                    "other types are currently unsupported.",
                                    "value");
      }

      int sourceEnum = (int) Convert.ChangeType(value, underlyingType);
      int valueToCheck = (int) Convert.ChangeType(flagValue, underlyingType);

      return (sourceEnum & valueToCheck) == valueToCheck;
    }

    #endregion

    #region static methods

    /// <summary>
    /// Determines whether the given enumeration value is a defined value of its parent enumeration.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the given value is a defined value of its associated enumeration; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='value'>
    /// The enumeration value to analyse.
    /// </param>
    internal static bool IsDefined(Enum value)
    {
      if(value == null)
      {
        throw new ArgumentNullException("value");
      }

      Type enumerationType = value.GetType();
      return Enum.IsDefined(enumerationType, value);
    }

    #endregion
  }
}

