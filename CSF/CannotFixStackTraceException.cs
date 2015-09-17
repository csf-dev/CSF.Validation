//
// CannotFixStackTraceException.cs
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
  /// An exception thrown when <see cref="M:ExceptionExtensions.FixStackTrace{TException}"/> is unable to use any
  /// mechanism to fix an exception's stack trace.
  /// </summary>
  public class CannotFixStackTraceException : Exception
  {
    #region constants

    private const string MESSAGE = "An exception was thrown and attempts were made to 'fix'/preserve its stack " +
                                   "trace, these attempts failed.  Try adding a constructor of signature " +
                                   "(SerializationInfo, StreamingContext) to the inner exception type.  Otherwise do " +
                                   "not use FixStackTrace on exceptions of this type.";

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.CannotFixStackTraceException"/> class.
    /// </summary>
    /// <param name='inner'>
    /// The inner exception.
    /// </param>
    public CannotFixStackTraceException(Exception inner) : base(MESSAGE, inner) {}

    #endregion
  }
}

