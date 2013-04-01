//  
//  EnumExtensions.cs
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

    /// <summary>
    /// Determines whether the given enumeration value is a defined value of its parent enumeration.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the given value is a defined value of its associated enumeration; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='value'>
    /// The enumeration value to analyse.
    /// </param>
    [Obsolete("This extension method is being moved to a counterpart type in the 'CSF' namespace.")]
    public static bool IsDefinedValue(this Enum value)
    {
      return CSF.EnumExtensions.IsDefined(value);
    }
    
    #endregion
  }
}

