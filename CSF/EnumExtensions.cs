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
using System.Linq;
using System.Collections.Generic;

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
      if(value == null)
      {
        throw new ArgumentNullException("value");
      }

      return Enum.IsDefined(value.GetType(), value);
    }

    /// <summary>
    /// Combines a flags-based enumerated value with the given flags, returning the result.
    /// </summary>
    /// <returns>
    /// The original value, with the given flags added.
    /// </returns>
    /// <param name='original'>
    /// The original value (unmodified in this process).
    /// </param>
    /// <param name='addedFlags'>
    /// A collection of the flags to add.
    /// </param>
    /// <typeparam name='TEnum'>
    /// The type of the enumerated value.
    /// </typeparam>
    public static TEnum WithFlags<TEnum>(this TEnum original, params TEnum[] addedFlags) where TEnum : struct
    {
      Type enumType = typeof(TEnum);

      if(!enumType.IsEnum
         || !enumType.HasAttribute<FlagsAttribute>())
      {
        throw new ArgumentException("This method is valid only on enumerated types decorated with FlagsAttribute.",
                                    "original");
      }

      Type underlyingType = Enum.GetUnderlyingType(enumType);

      ulong
        numericOriginal = GetEnumValue(original, underlyingType),
        numericAdded = addedFlags.Aggregate((ulong) 0, (acc,next) => acc += GetEnumValue(next, underlyingType)),
        output = numericOriginal | numericAdded;

      return (TEnum) Enum.ToObject(enumType, output);
    }

    /// <summary>
    /// Excludes the given flags from a flags-based enumerated value, returning the result.
    /// </summary>
    /// <returns>
    /// The original value, with the given flags removed.
    /// </returns>
    /// <param name='original'>
    /// The original value (unmodified in this process).
    /// </param>
    /// <param name='removedFlags'>
    /// A collection of the flags to remove.
    /// </param>
    /// <typeparam name='TEnum'>
    /// The type of the enumerated value.
    /// </typeparam>
    public static TEnum WithoutFlags<TEnum>(this TEnum original, params TEnum[] removedFlags) where TEnum : struct
    {
      Type enumType = typeof(TEnum);

      if(!enumType.IsEnum
         || !enumType.HasAttribute<FlagsAttribute>())
      {
        throw new ArgumentException("This method is valid only on enumerated types decorated with FlagsAttribute.",
                                    "original");
      }

      Type underlyingType = Enum.GetUnderlyingType(enumType);

      ulong
        numericOriginal = GetEnumValue(original, underlyingType),
        numericRemoved = removedFlags.Aggregate((ulong) 0, (acc,next) => acc += GetEnumValue(next, underlyingType)),
        output = numericOriginal & (~numericRemoved);

      return (TEnum) Enum.ToObject(enumType, output);
    }

    /// <summary>
    /// Breaks a <see cref="FlagsAttribute"/>-decorated enumeration value down into its component parts and returns
    /// them,
    /// </summary>
    /// <returns>The individual enumeration values.</returns>
    /// <param name="combinedValue">The combined enumeration value.</param>
    /// <typeparam name="TEnum">The type of enumeration.</typeparam>
    public static TEnum[] GetIndividualValues<TEnum>(this TEnum combinedValue) where TEnum : struct
    {
      var enumType = combinedValue.GetType();
      if(!enumType.IsEnum)
      {
        throw new ArgumentException("Combined value must be an enumerated type.", "combinedValue");
      }
      else if(!enumType.HasAttribute<FlagsAttribute>())
      {
        throw new ArgumentException("Enumeration type must be decorated with System.FlagsAttribute.", "combinedValue");
      }

      List<TEnum> output = new List<TEnum>();

      var underlyingValue = GetEnumValue(combinedValue, enumType);
      int exponent = 0;

      while(underlyingValue > 0 && exponent < 64)
      {
        ulong testValue = (ulong) Math.Pow(2, exponent ++);

        if((underlyingValue & testValue) == testValue)
        {
          output.Add((TEnum) Enum.ToObject(enumType, testValue));
          underlyingValue = underlyingValue - testValue;
        }
      }

      return output.ToArray();
    }

    #endregion

    #region static methods

    /// <summary>
    /// Gets the numeric equivalent of an enumeration value, given that the enumeration uses the given underlying type.
    /// </summary>
    /// <returns>
    /// The numeric equivalent of the enumeration value.
    /// </returns>
    /// <param name='value'>
    /// The enumeration value.
    /// </param>
    /// <param name='underlyingTypeCode'>
    /// The <c>System.TypeCode</c> of the underlying type of the enumeration.
    /// </param>
    internal static ulong GetEnumValue(object value, TypeCode underlyingTypeCode)
    {
      ulong output;

      switch(underlyingTypeCode)
      {
      case TypeCode.SByte:
        output = (ulong) ((byte) ((sbyte) value));
        break;

      case TypeCode.Byte:
        output = (ulong) ((byte) value);
        break;

      case TypeCode.Int16:
        output = (ulong) ((ushort) ((short) value));
        break;

      case TypeCode.UInt16:
        output = (ulong) ((ushort) value);
        break;

      case TypeCode.Int32:
        output = (ulong) ((int) value);
        break;

      case TypeCode.UInt32:
        output = (ulong) ((uint) value);
        break;

      case TypeCode.Int64:
        output = (ulong) ((long) value);
        break;

      case TypeCode.UInt64:
        output = (ulong) value;
        break;

      default:
        throw new ArgumentException("The underlying type code must be valid for an enumeration", "underlyingTypeCode");
      }

      return output;
    }

    /// <summary>
    /// Gets the numeric equivalent of an enumeration value, given that the enumeration uses the given underlying type.
    /// </summary>
    /// <returns>
    /// The numeric equivalent of the enumeration value.
    /// </returns>
    /// <param name='value'>
    /// The enumeration value.
    /// </param>
    /// <param name='underlyingType'>
    /// The underlying type of the enumeration.
    /// </param>
    internal static ulong GetEnumValue(object value, Type underlyingType)
    {
      if(underlyingType == null)
      {
        throw new ArgumentNullException("underlyingType");
      }

      return GetEnumValue(value, Type.GetTypeCode(underlyingType));
    }

    #endregion
  }
}

