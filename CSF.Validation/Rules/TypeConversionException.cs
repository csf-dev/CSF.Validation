//
// TypeConversionException.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2017 Craig Fowler
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
namespace CSF.Validation.Rules
{
  /// <summary>
  /// An exception which is raised when a validation rules attempts to convert from one type to another.
  /// </summary>
  [System.Serializable]
  public class TypeConversionException : Exception
  {
    const string TYPE_FORMAT = "`{0}'", NULL = "<null>";

    /// <summary>
    /// Initializes a new instance of the <see cref="T:TypeConversionException"/> class
    /// </summary>
    public TypeConversionException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MyException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public TypeConversionException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MyException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public TypeConversionException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MyException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected TypeConversionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }

    internal static TypeConversionException CreateForValue(Exception inner,
                                                           object valueToConvert,
                                                           Type expectedType)
    {
      if(expectedType == null)
        throw new ArgumentNullException(nameof(expectedType));
      if(inner == null)
        throw new ArgumentNullException(nameof(inner));

      return Create(inner,
                    valueToConvert,
                    expectedType,
                    Resources.ExceptionMessages.ValueTypeConversionFailureFormat);
    }

    internal static TypeConversionException CreateForValidated(Exception inner,
                                                               object valueToConvert,
                                                               Type expectedType)
    {
      if(expectedType == null)
        throw new ArgumentNullException(nameof(expectedType));
      if(inner == null)
        throw new ArgumentNullException(nameof(inner));

      return Create(inner,
                    valueToConvert,
                    expectedType,
                    Resources.ExceptionMessages.ValidatedObjectTypeConversionFailureFormat);
    }

    static TypeConversionException Create(Exception inner,
                                          object valueToConvert,
                                          Type expectedType,
                                          string messageFormat)
    {
      if(messageFormat == null)
        throw new ArgumentNullException(nameof(messageFormat));
      if(expectedType == null)
        throw new ArgumentNullException(nameof(expectedType));
      if(inner == null)
        throw new ArgumentNullException(nameof(inner));

      string actualTypeName;

      if(ReferenceEquals(valueToConvert, null))
      {
        actualTypeName = NULL;
      }
      else
      {
        actualTypeName = String.Format(TYPE_FORMAT, valueToConvert.GetType().FullName);
      }

      var message = String.Format(messageFormat,
                                  expectedType.FullName,
                                  actualTypeName);
      return new TypeConversionException(message, inner);
    }
  }
}
