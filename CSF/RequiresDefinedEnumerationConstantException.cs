//
// RequiresDefinedEnumerationConstantException.cs
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
using CSF.Resources;

namespace CSF
{
  /// <summary>
  /// Exception raised when an enumeration value is required to be a defined enumeration constant, but it is not.
  /// </summary>
  [Serializable]
  public class RequiresDefinedEnumerationConstantException : Exception
  {
    #region fields

    private string _message;

    #endregion

    #region properties

    /// <summary>
    /// Gets the exception message.
    /// </summary>
    /// <value>The message.</value>
    public override string Message
    {
      get {
        return _message?? base.Message;
      }
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="T:RequiresDefinedEnumerationConstantException"/> class
    /// </summary>
    public RequiresDefinedEnumerationConstantException() : this(ExceptionMessages.MustBeEnumerationConstantDefaultMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:RequiresDefinedEnumerationConstantException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public RequiresDefinedEnumerationConstantException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:RequiresDefinedEnumerationConstantException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public RequiresDefinedEnumerationConstantException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.RequiresDefinedEnumerationConstantException"/> class.
    /// </summary>
    /// <param name="enumerationType">The enumeration type.</param>
    /// <param name="name">The name of the value.</param>
    public RequiresDefinedEnumerationConstantException(Type enumerationType, string name)
    {
      var typeName = (enumerationType != null)? enumerationType.FullName : "UNKNOWN";
      _message = String.Format(ExceptionMessages.MustBeEnumerationConstantFormat, name, typeName);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:RequiresDefinedEnumerationConstantException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected RequiresDefinedEnumerationConstantException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}

