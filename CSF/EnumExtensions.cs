//
// EnumExtensions.cs
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
using System.Linq;
using System.Reflection;
using CSF.Resources;
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
    /// <c>true</c> if the given value is a defined value of its associated enumeration; otherwise, <c>false</c>.
    /// <param name="value">The enumeration value to analyse.</param>
    /// <typeparam name="TEnum">The enumeration type.</typeparam>
    public static bool IsDefinedValue<TEnum>(this TEnum value) where TEnum : struct
    {
      var type = typeof(TEnum);

      RequireEnum(type);

      return Enum.IsDefined(typeof(TEnum), value);
    }

    /// <summary>
    /// Asserts that the given enumeration value is a defined value of its parent enumeration and raises an
    /// exception if it is not.
    /// </summary>
    /// <exception cref="RequiresDefinedEnumerationConstantException">If the assertion fails.</exception>
    /// <param name="value">The enumeration value upon which to perform the assertion.</param>
    /// <param name="name">An object name, to aid in identifying the object should an exception be raised.</param>
    /// <typeparam name="TEnum">The enumeration type.</typeparam>
    public static void RequireDefinedValue<TEnum>(this TEnum value, string name) where TEnum : struct
    {
      var type = typeof(TEnum);

      RequireEnum(type);

      if(!Enum.IsDefined(typeof(TEnum), value))
      {
        throw new RequiresDefinedEnumerationConstantException(value.GetType(), name);
      }
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

      RequireEnum(enumType);
      RequireFlagsAttribute(enumType);

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

      RequireEnum(enumType);
      RequireFlagsAttribute(enumType);

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
    public static IEnumerable<TEnum> GetIndividualValues<TEnum>(this TEnum combinedValue) where TEnum : struct
    {
      var enumType = typeof(TEnum);

      RequireEnum(enumType);
      RequireFlagsAttribute(enumType);

      var numericEnumValue = GetEnumValue(combinedValue, enumType);

      for(int exponent = 0; exponent < 64; exponent++)
      {
        if(numericEnumValue == 0)
        {
          yield break;
        }

        var testValue = (ulong) Math.Pow(2, exponent);

        if((numericEnumValue & testValue) == testValue)
        {
          numericEnumValue -= testValue;
          yield return (TEnum) Enum.ToObject(enumType, testValue);
        }
      }
    }

    /// <summary>
    /// Gets a <c>System.Reflection.FieldInfo</c> instance from an enumeration value.
    /// </summary>
    /// <returns>
    /// Information about the member that represents the enumeration value.
    /// </returns>
    /// <param name='value'>
    /// The enumeration value.
    /// </param>
    public static FieldInfo GetFieldInfo<TEnum>(this TEnum value) where TEnum : struct
    {
      var enumType = typeof(TEnum);

      RequireDefinedValue(value, nameof(value));

      return enumType.GetField(value.ToString());
    }

    #endregion

    #region methods

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
        string message = String.Format(ExceptionMessages.TypeCodeMustBeValid, typeof(TypeCode).Name);
        throw new ArgumentException(message, nameof(underlyingTypeCode));
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
        throw new ArgumentNullException(nameof(underlyingType));
      }

      return GetEnumValue(value, Type.GetTypeCode(underlyingType));
    }

    /// <summary>
    /// Asserts that the given type is an enumeration.
    /// </summary>
    /// <param name="type">The type to analyse.</param>
    internal static void RequireEnum(Type type)
    {
      if(!type.IsEnum)
      {
        string message = String.Format(ExceptionMessages.OnlySupportedForEnumTypesFormat, type.FullName);
        throw new NotSupportedException(message);
      }
    }

    /// <summary>
    /// Asserts that the given type is decorated with <c>FlagsAttribute</c>.
    /// </summary>
    /// <param name="type">The type to analyse.</param>
    internal static void RequireFlagsAttribute(Type type)
    {
      if(type.GetCustomAttribute<FlagsAttribute>() == null)
      {
        string message = String.Format(ExceptionMessages.MustBeDecoratedWithFlagsAttribute,
                                       typeof(FlagsAttribute).Name,
                                       type.FullName);
        throw new NotSupportedException(message);
      }
    }

    #endregion
  }
}

