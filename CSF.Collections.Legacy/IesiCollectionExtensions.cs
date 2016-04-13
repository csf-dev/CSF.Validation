//
// IesiCollectionExtensions.cs
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
using Iesi.Collections.Generic;

namespace CSF.Collections.Legacy
{
  /// <summary>
  /// Provides collection extension methods specific to the <c>Iesi.Collections</c> library.
  /// </summary>
  public static class IesiCollectionExtensions
  {
    /// <summary>
    /// Converts a generic collection of elements into an equivalent set.
    /// </summary>
    /// <returns>
    /// A generic <c>ISet</c> containing equivalent elements to the source collection.
    /// </returns>
    /// <param name='source'>
    /// A collection.
    /// </param>
    /// <typeparam name='T'>
    /// The type of item contained within the collection.
    /// </typeparam>
    public static Iesi.Collections.Generic.ISet<T> ToSet<T>(this ICollection<T> source)
    {
      if(source == null)
      {
        throw new ArgumentNullException("source");
      }

      return new HashedSet<T>(source);
    }
  }
}

