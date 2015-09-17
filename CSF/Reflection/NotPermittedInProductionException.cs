//
// NotPermittedInProductionException.cs
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
using System.Runtime.Serialization;


namespace CSF.Reflection
{
  /// <summary>
  /// Exception type thrown when functionality that is marked as forbidden in production (non-debug) builds is used.
  /// </summary>
  [Serializable]
  public class NotPermittedInProductionException : Exception
  {
    #region constants

    private const string
      DEFAULT_MESSAGE_NO_TYPE           = "Functionality that is marked forbidden in production builds must not be " +
                                          "used.",
      DEFAULT_MESSAGE_WITH_TYPE         = "Functionality that is marked forbidden in production builds must not be " +
                                          "used.  See the 'Type' property for the type that is forbidden.",
      TYPE_KEY                          = "Forbidden type";

    #endregion

    #region properties

    /// <summary>
    /// Gets the type that is marked as forbidden.
    /// </summary>
    /// <value>
    /// The type.
    /// </value>
    public Type Type
    {
      get {
        return this.Data[TYPE_KEY] as Type;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Reflection.NotPermittedInProductionException"/> class.
    /// </summary>
    public NotPermittedInProductionException() : this(DEFAULT_MESSAGE_NO_TYPE, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Reflection.NotPermittedInProductionException"/> class.
    /// </summary>
    /// <param name='message'>
    /// Message.
    /// </param>
    public NotPermittedInProductionException(string message) : this(message, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Reflection.NotPermittedInProductionException"/> class.
    /// </summary>
    /// <param name='inner'>
    /// Inner.
    /// </param>
    public NotPermittedInProductionException(Exception inner) : this(DEFAULT_MESSAGE_NO_TYPE, inner) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Reflection.NotPermittedInProductionException"/> class.
    /// </summary>
    /// <param name='message'>
    /// Message.
    /// </param>
    /// <param name='inner'>
    /// Inner.
    /// </param>
    public NotPermittedInProductionException(string message, Exception inner) : base(message, inner) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Reflection.NotPermittedInProductionException"/> class.
    /// </summary>
    /// <param name='type'>
    /// The type that contained the forbidden functionality.
    /// </param>
    public NotPermittedInProductionException(Type type) : this(DEFAULT_MESSAGE_WITH_TYPE, null)
    {
      if(type == null)
      {
        throw new ArgumentNullException("type");
      }

      this.Data[TYPE_KEY] = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Reflection.NotPermittedInProductionException"/> class.
    /// </summary>
    /// <param name='info'>
    /// Serialization info.
    /// </param>
    /// <param name='cx'>
    /// Streaming context.
    /// </param>
    public NotPermittedInProductionException(SerializationInfo info, StreamingContext cx) : base(info, cx) {}

    #endregion
  }
}

