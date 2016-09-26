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
using System.Reflection;
using System.Linq;

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
    public static IEnumerable<Type> GetSubclasses(this Type type, Assembly searchAssembly)
    {
      if(searchAssembly == null)
      {
        throw new ArgumentNullException (nameof(searchAssembly));
      }
      else if(type == null)
      {
        throw new ArgumentNullException (nameof(type));
      }

      return searchAssembly
        .GetTypes()
        .Where(x => type.IsAssignableFrom(x) && x != type);
    }

    #endregion
  }
}

