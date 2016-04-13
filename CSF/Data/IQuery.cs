//
// IQuery.cs
//
// Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
// Copyright (c) 2016 Craig Fowler
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

namespace CSF.Data
{
  /// <summary>
  /// Interface for a query component that is capable of returning a queryable data source.
  /// </summary>
  public interface IQuery
  {
    /// <summary>
    /// Creates an instance of the given object-type, based upon a theory that it exists in the underlying data-source.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method will always return a non-null object instance, even if the underlying object does not exist in the
    /// data source.  If a 'thoery object' is created for an object which does not actually exist, then an exception
    /// could be thrown if that theory object is used.
    /// </para>
    /// </remarks>
    /// <param name="identityValue">The identity value for the object to retrieve.</param>
    /// <typeparam name="TQueried">The type of object to retrieve.</typeparam>
    TQueried Theorise<TQueried>(object identityValue) where TQueried : class;

    /// <summary>
    /// Gets a single instance from the underlying data source, identified by an identity value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method will either get an object instance, or it will return <c>null</c> (if no instance is found).
    /// </para>
    /// </remarks>
    /// <param name="identityValue">The identity value for the object to retrieve.</param>
    /// <typeparam name="TQueried">The type of object to retrieve.</typeparam>
    TQueried Get<TQueried>(object identityValue) where TQueried : class;

    /// <summary>
    /// Gets a new queryable data-source.
    /// </summary>
    /// <typeparam name="TQueried">The type of queried-for object.</typeparam>
    TQueried Query<TQueried>() where TQueried : class;
  }
}

