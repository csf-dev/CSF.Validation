//
// StalledValidationRunException.cs
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
namespace CSF.Validation.ValidationRuns
{
  /// <summary>
  /// Exception raised when a validation run has stalled - that is - it cannot proceed because no rules indicate that they
  /// are runnable, but not all have yet completed.
  /// </summary>
#if !NETSTANDARD1_0
  [System.Serializable]
#endif
  public class StalledValidationRunException : System.Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="StalledValidationRunException"/> class
    /// </summary>
    public StalledValidationRunException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StalledValidationRunException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public StalledValidationRunException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StalledValidationRunException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public StalledValidationRunException(string message, System.Exception inner) : base(message, inner)
    {
    }

#if !NETSTANDARD1_0
    /// <summary>
    /// Initializes a new instance of the <see cref="StalledValidationRunException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected StalledValidationRunException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
#endif
  }
}
