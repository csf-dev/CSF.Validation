//
// IDictionaryExtensions.cs
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
using System.Collections.Specialized;
using System.Collections.Generic;

namespace CSF.Collections
{
  /// <summary>
  /// Extension methods for dictionary types.
  /// </summary>
  public static class IDictionaryExtensions
  {
    /// <summary>
    /// Converts a dictionary of strings (string keys and string values) into a <c>NameValueCollection</c> containing
    /// the same data.
    /// </summary>
    /// <returns>
    /// The name value collection.
    /// </returns>
    /// <param name='dictionary'>
    /// The dictionary to convert.
    /// </param>
    public static NameValueCollection ToNameValueCollection(this IDictionary<string,string> dictionary)
    {
      if(dictionary == null)
      {
        throw new ArgumentNullException("dictionary");
      }

      NameValueCollection output = new NameValueCollection();

      foreach(string key in dictionary.Keys)
      {
        output.Add(key, dictionary[key]);
      }

      return output;
    }
  }
}

