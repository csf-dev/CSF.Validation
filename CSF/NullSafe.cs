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

    /// <summary>
    /// Gets a value from a specified input object in null-safe manner.
    /// </summary>
    /// <param name='input'>
    /// The input object instance from which to extract a value.
    /// </param>
    /// <param name='getter'>
    /// A function (taking an input object and outputting the desired output object) that is used to get the output.
    /// </param>
    /// <param name='output'>
    /// If the return value of this method is <c>true</c> then this parameter contains the output of the
    /// getter-function.  If <c>false</c> then the value of this parameter is undefined.
    /// </param>
    /// <returns>
    /// A value that indicates whether or not this operation was successful or not.
    /// </returns>
    /// <typeparam name='TInput'>
    /// The type of input object.
    /// </typeparam>
    /// <typeparam name='TOutput'>
    /// The type of expected output object.
    /// </typeparam>
    public static bool Get<TInput,TOutput>(TInput input, Func<TInput,TOutput> getter, out TOutput output)
    {
      bool result = false;
      output = default(TOutput);

      try
      {
        output = getter(input);
        result = true;
      }
      catch(NullReferenceException) {}

      return result;
    }

    /// <summary>
    /// Gets a string representation of an object (taken from a specified input) in null-safe manner.
    /// </summary>
    /// <param name='input'>
    /// The input object instance from which to extract the value.
    /// </param>
    /// <param name='getter'>
    /// A function (taking an input object and outputting the desired output object) that is used to get the output.
    /// </param>
    /// <returns>
    /// A string representation of the output.  This may be a null reference if the output is null.
    /// </returns>
    /// <typeparam name='TInput'>
    /// The type of input object.
    /// </typeparam>
    /// <typeparam name='TOutput'>
    /// The type of expected output object.
    /// </typeparam>
    public static string ToString<TInput,TOutput>(TInput input, Func<TInput,TOutput> getter)
    {
      TOutput value = default(TOutput);
      bool result = NullSafe.Get<TInput,TOutput>(input, getter, out value);
      string output;

      if(result)
      {
        output = (value != null)? value.ToString() : null;
      }
      else
      {
        output = null;
      }

      return output;
    }
  }
}

