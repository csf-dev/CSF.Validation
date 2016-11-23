//
// NullSafe.cs
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

namespace CSF
{
  /// <summary>
  /// Static convertor type that converts to nullable types, which return a null state if conversion failed.
  /// </summary>
  public static class NullSafe
  {
    /// <summary>
    /// Converts the specified value to a nullable instance of the specified type.
    /// </summary>
    /// <param name='value'>
    /// The value to convert.
    /// </param>
    /// <typeparam name='TDestination'>
    /// The target/destination type.
    /// </typeparam>
    public static Nullable<TDestination> ConvertTo<TDestination>(object value)
      where TDestination : struct
    {
      TDestination? output = null;

      try
      {
        output = (TDestination) Convert.ChangeType(value, typeof(TDestination));
      }
      // Just drop any of these exceptions on the floor, they all indicate that the conversion failed
      catch(ArgumentNullException) {}
      catch(FormatException) {}
      catch(OverflowException) {}
      catch(InvalidCastException) {}

      return output;
    }
  }
}

