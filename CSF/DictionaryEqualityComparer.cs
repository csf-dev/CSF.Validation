//
// DictionaryEqualityComparer.cs
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
using System.Collections;
using System.Collections.Generic;

namespace CSF
{
  /// <summary>
  /// Provides an <see cref="IEqualityComparer"/> designed to compare two objects that implement the generic
  /// <c>IDictionary</c> interface.
  /// </summary>
  public class DictionaryEqualityComparer<TKey, TValue> : IEqualityComparer
  {
    #region methods

    /// <summary>
    /// Tests for equality between the two objects (which should both be dictionaries) of the appropriate generic types.
    /// </summary>
    /// <param name='obj1'>
    /// A dictionary instance
    /// </param>
    /// <param name='obj2'>
    /// A dictionary instance
    /// </param>
    public bool AreEqual(object obj1, object obj2)
    {
      bool output;

      if(Object.ReferenceEquals(obj1, obj2))
      {
        output = true;
      }
      else if(obj1 == null)
      {
        output = false;
      }
      else
      {
        try
        {
          IDictionary<TKey, TValue>
            dictionary1 = (IDictionary<TKey, TValue>) obj1,
            dictionary2 = (IDictionary<TKey, TValue>) obj2;

          output = (dictionary1.Count == dictionary2.Count);

          if(output)
          {
            foreach(TKey key in dictionary1.Keys)
            {
              if(!dictionary2.ContainsKey(key)
                 || !dictionary2[key].Equals(dictionary1[key]))
              {
                output = false;
                break;
              }
            }
          }
        }
        catch(InvalidCastException)
        {
          output = obj1.Equals(obj2);
        }
      }

      return output;
    }

    /// <summary>
    /// Unsupported method, would get the hash code of a DateTime.
    /// </summary>
    /// <param name='obj'>
    /// An object
    /// </param>
    public int GetHashCode(object obj)
    {
      throw new NotImplementedException("This method is unsupported");
    }

    #endregion

    #region explicit interface implementation

    bool IEqualityComparer.Equals(object obj1, object obj2)
    {
      return this.AreEqual(obj1, obj2);
    }

    #endregion
  }
}

