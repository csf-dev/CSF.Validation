//
// MemberInfoExtensions.cs
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
using System.Reflection;
using System.Linq;

namespace CSF.Reflection
{
  /// <summary>
  /// Helper type provides extension methods to the <see cref="MemberInfo"/> type.
  /// </summary>
  public static class MemberInfoExtensions
  {
    #region extension methods
    
    /// <summary>
    /// Gets a single <see cref="Attribute"/> that decorates the given member.
    /// </summary>
    /// <returns>
    /// The attribute present on the member, or null if no attribute was present.
    /// </returns>
    /// <param name='memberOrType'>
    /// A member or type.
    /// </param>
    /// <typeparam name='TAttribute'>
    /// The type of attribute desired.
    /// </typeparam>
    public static TAttribute GetAttribute<TAttribute>(this MemberInfo memberOrType) where TAttribute : Attribute
    {
      return memberOrType.GetAttribute<TAttribute>(false);
    }
    
    /// <summary>
    /// Gets a collection of <see cref="Attribute"/> that decorates the given member.
    /// </summary>
    /// <returns>
    /// A collection of attributes for the member, which may be empty.
    /// </returns>
    /// <param name='memberOrType'>
    /// A member or type.
    /// </param>
    /// <typeparam name='TAttribute'>
    /// The type of attribute desired.
    /// </typeparam>
    public static IList<TAttribute> GetAttributes<TAttribute>(this MemberInfo memberOrType) where TAttribute : Attribute
    {
      return memberOrType.GetAttributes<TAttribute>(false);
    }
    
    /// <summary>
    /// Gets a single <see cref="Attribute"/> that decorates the given member.
    /// </summary>
    /// <returns>
    /// The attribute present on the member, or null if no attribute was present.
    /// </returns>
    /// <param name='memberOrType'>
    /// A member or type.
    /// </param>
    /// <param name='inherit'>
    /// Whether or not to use inheritance when searching for the attribute.
    /// </param>
    /// <typeparam name='TAttribute'>
    /// The type of attribute desired.
    /// </typeparam>
    /// <exception cref='InvalidOperationException'>
    /// Is thrown if the member is decorated with multiple attributes of the desired type.
    /// </exception>
    public static TAttribute GetAttribute<TAttribute>(this MemberInfo memberOrType,
                                                      bool inherit) where TAttribute : Attribute
    {
      IList<TAttribute> attributes = memberOrType.GetAttributes<TAttribute>(inherit);
      
      if(attributes.Count > 1)
      {
        throw new InvalidOperationException(String.Format("Member `{0}' is decorated with `{1}' multiple times.",
                                                          memberOrType.Name,
                                                          typeof(TAttribute).FullName));
      }
      
      return (attributes.Count != 0)? attributes.First() : (TAttribute) null;
    }
    
    /// <summary>
    /// Gets a collection of <see cref="Attribute"/> that decorates the given member.
    /// </summary>
    /// <returns>
    /// A collection of attributes for the member, which may be empty.
    /// </returns>
    /// <param name='memberOrType'>
    /// A member or type.
    /// </param>
    /// <param name='inherit'>
    /// Whether or not to use inheritance when searching for the attribute.
    /// </param>
    /// <typeparam name='TAttribute'>
    /// The type of attribute desired.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" />.
    /// </exception>
    public static IList<TAttribute> GetAttributes<TAttribute>(this MemberInfo memberOrType,
                                                              bool inherit) where TAttribute : Attribute
    {
      if(memberOrType == null)
      {
        throw new ArgumentNullException ("memberOrType");
      }
      
      return memberOrType.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().ToList();
    }

    /// <summary>
    /// Determines whether this instance is decorated with the specified attribute.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is decorated with the specified attribute; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='memberOrType'>
    /// The member or type to inspect.
    /// </param>
    /// <typeparam name='TAttribute'>
    /// The attribute type to look for.
    /// </typeparam>
    public static bool HasAttribute<TAttribute>(this MemberInfo memberOrType) where TAttribute : Attribute
    {
      return memberOrType.HasAttribute<TAttribute>(false);
    }

    /// <summary>
    /// Determines whether this instance is decorated with the specified attribute.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is decorated with the specified attribute; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='memberOrType'>
    /// The member or type to inspect.
    /// </param>
    /// <param name='inherit'>
    /// Whether or not to search the <paramref name="memberOrType"/>'s inheritance chain to find the attribute or not.
    /// </param>
    /// <typeparam name='TAttribute'>
    /// The attribute type to look for.
    /// </typeparam>
    public static bool HasAttribute<TAttribute>(this MemberInfo memberOrType, bool inherit) where TAttribute : Attribute
    {
      return (memberOrType.GetAttributes<TAttribute>(inherit).Count > 0);
    }
    
    #endregion
  }
}

