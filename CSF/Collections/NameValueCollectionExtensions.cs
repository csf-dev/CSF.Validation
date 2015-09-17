//
// NameValueCollectionExtensions.cs
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
using System.Collections.Specialized;

namespace CSF.Collections
{
  /// <summary>
  /// Extension methods to <see cref="System.Collections.Specialized.NameValueCollection"/>
  /// </summary>
  public static class NameValueCollectionExtensions
  {
    #region methods

    /// <summary>
    /// Returns a clone (deep copy) of the <see cref="NameValueCollection"/> as an <c>IDictionary</c> of strings.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The new/output collection is not synchronised with the source collection.  If either is altered after this
    /// method has been called, the other is left unchanged.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A dictionary of strings with the same content as the source collection.
    /// </returns>
    /// <param name='source'>
    /// A <see cref="NameValueCollection"/>.
    /// </param>
    public static IDictionary<string,string> ToDictionary(this NameValueCollection source)
    {
      IDictionary<string,string> output = new Dictionary<string, string>();

      if(source == null)
      {
        throw new ArgumentNullException("source");
      }

      foreach(string key in source.Keys)
      {
        output.Add(key, source[key]);
      }

      return output;
    }

    #endregion
  }
}

