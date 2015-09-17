//
// ValidationFailureException.cs
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

namespace CSF.Validation
{
  /// <summary>
  /// Base type for an exception that is thrown as a result of a validation failure.
  /// </summary>
  /// <remarks>
  /// This exception may be thrown if the validator is configured to throw exceptions on failed validation, or if an
  /// individual validation test raises an exception.
  /// </remarks>
  public abstract class ValidationFailureException : Exception
  {
    #region properties
    
    /// <summary>
    /// Gets the type of the object that was being validated.
    /// </summary>
    /// <value>
    /// The <see cref="System.Type"/> of the target object.
    /// </value>
    public abstract Type TargetType { get; }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Validation.ValidationFailureException"/> class.
    /// </summary>
    /// <param name='message'>
    /// Message.
    /// </param>
    /// <param name='inner'>
    /// Inner.
    /// </param>
    public ValidationFailureException(string message, Exception inner) : base(message, inner) {}
    
    #endregion
  }
}

