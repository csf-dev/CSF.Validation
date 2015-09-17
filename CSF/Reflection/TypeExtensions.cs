//
// TypeExtensions.cs
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
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CSF.Reflection
{
  /// <summary>
  /// Helper type containing extension methods for <see cref="System.Type"/>.
  /// </summary>
  public static class TypeExtensions
  {
    #region extension methods
    
    /// <summary>
    /// Gets a collection of <see cref="System.Type"/> that are subclasses of the given <paramref name="type"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method only searches for subclasses within a specific assembly.  This overload looks within the assembly
    /// that defines <paramref name="type"/>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A collection of <see cref="System.Type"/>.
    /// </returns>
    /// <param name='type'>
    /// The type for which to find subclasses.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public static IList<Type> GetSubclasses(this Type type)
    {
      if(type == null)
      {
        throw new ArgumentNullException ("type");
      }
      
      return type.GetSubclasses(Assembly.GetAssembly(type));
    }
    
    /// <summary>
    /// Gets a collection of <see cref="System.Type"/> that are subclasses of the given <paramref name="type"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method only searches for subclasses within a specific assembly.  This overload looks within a given
    /// <paramref name="searchAssembly"/>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A collection of <see cref="System.Type"/>.
    /// </returns>
    /// <param name='type'>
    /// The type for which to find subclasses.
    /// </param>
    /// <param name='searchAssembly'>
    /// The <see cref="Assembly"/> in which to search for subclasses of <paramref name="type"/>.
    /// </param>
    public static IList<Type> GetSubclasses(this Type type, Assembly searchAssembly)
    {
      if(searchAssembly == null)
      {
        throw new ArgumentNullException ("searchAssembly");
      }
      else if(type == null)
      {
        throw new ArgumentNullException ("type");
      }

      return (from x in searchAssembly.GetTypes()
              where
                type.IsAssignableFrom(x)
                && x != type
              select x).ToList();
    }

    /// <summary>
    /// Determines whether the current type implements the specified interface.
    /// </summary>
    /// <returns>
    /// True if the <paramref name="type"/> implements the desired interface, false otherwise.
    /// </returns>
    /// <param name='type'>
    /// The type upon which to search for the interface
    /// </param>
    /// <typeparam name='TInterface'>
    /// The type of the interface to search for.
    /// </typeparam>
    public static bool ImplementsInterface<TInterface>(this Type type)
    {
      if(type == null)
      {
        throw new ArgumentNullException("type");
      }

      return type.ImplementsInterface(typeof(TInterface));
    }

    /// <summary>
    /// Determines whether the current type implements the specified interface.
    /// </summary>
    /// <returns>
    /// True if the <paramref name="type"/> implements the desired interface, false otherwise.
    /// </returns>
    /// <param name='type'>
    /// The type upon which to search for the interface
    /// </param>
    /// <param name='interfaceType'>
    /// The type of the interface to search for.
    /// </param>
    public static bool ImplementsInterface(this Type type, Type interfaceType)
    {
      if(type == null)
      {
        throw new ArgumentNullException("type");
      }
      if(interfaceType == null)
      {
        throw new ArgumentNullException("interfaceType");
      }
      else if(!interfaceType.IsInterface)
      {
        throw new ArgumentException("interfaceType must be an interface");
      }

      return interfaceType.IsAssignableFrom(type);
    }

    #endregion
  }
}

