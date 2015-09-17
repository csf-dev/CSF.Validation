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
using System.Reflection;
using System.ComponentModel;

namespace CSF.Reflection
{
  /// <summary>
  /// Type containing extension methods that are useful to enumerated types.
  /// </summary>
  /// <remarks>
  /// <para>
  /// These extension methods relate specifically to reflection, and thus belong in the reflection namespace.
  /// </para>
  /// </remarks>
  public static class EnumExtensions
  {
    #region extension methods
    
    /// <summary>
    /// Gets the description from an enumeration member.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method uses the presence of <see cref="DescriptionAttribute"/> to get a human-readable description from an
    /// enumeration value.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The description, or a null reference if no description attribute was present on the enumeration member indicated
    /// by <paramref name="value"/>.
    /// </returns>
    /// <param name='value'>
    /// The enumeration value
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when the specified value does not correspond to a member of the enumeration.
    /// </exception>
    public static string GetUIText(this Enum value)
    {
      UITextAttribute attribute = value.GetFieldInfo().GetAttribute<UITextAttribute>();
      
      return (attribute != null)? attribute.Text : null;
    }
    
    /// <summary>
    /// Gets a <see cref="FieldInfo"/> instance from an enumeration value.
    /// </summary>
    /// <returns>
    /// Information about the member that represents the enumeration value.
    /// </returns>
    /// <param name='value'>
    /// The enumeration value for which to derive a <see cref="FieldInfo"/>.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" />.
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown if the <paramref name="value"/> is not a defined enumeration constant.
    /// </exception>
    public static FieldInfo GetFieldInfo(this Enum value)
    {
      if(value == null)
      {
        throw new ArgumentNullException ("value");
      }
      
      Type enumerationType = value.GetType();
      
      if(!Enum.IsDefined(enumerationType, value))
      {
        throw new ArgumentException(String.Format("Enumeration value `{0}' is not defined for type `{1}'",
                                                  value,
                                                  enumerationType.FullName));
      }
      
      FieldInfo output = enumerationType.GetField(value.ToString());
      
      if(output == null)
      {
        throw new InvalidOperationException("Theoretically impossible scenario; enumeration value is defined but " +
                                            "could not get a matching FieldInfo.  Report this to a developer if you " +
                                            "see it!");
      }
      
      return output;
    }

    #endregion
  }
}

