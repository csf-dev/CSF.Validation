//
// ValidationFailureException`1.cs
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
  /// An exception thrown by instances of <c>IValidator&lt;TTarget&gt;</c> when validation fails and the parameters of
  /// the validator indicate that an exception should be thrown.
  /// </summary>
  /// <remarks>
  /// This exception may also be thrown if an individual validation test raises an exception.
  /// </remarks>
  public class ValidationFailureException<TTarget> : ValidationFailureException
  {
    #region properties
    
    /// <summary>
    /// Gets the type of the object that was being validated.
    /// </summary>
    /// <value>
    /// The <see cref="System.Type"/> of the target object.
    /// </value>
    public override Type TargetType
    {
      get {
        return typeof(TTarget);
      }
    }
    
    /// <summary>
    /// Gets a collection of the validation failures that lead to the raising of this exception.
    /// </summary>
    /// <value>
    /// A collection of validation failures.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public ValidationTestList<TTarget> Failures
    {
      get {
        return (ValidationTestList<TTarget>) this.Data["Failures"];
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        this.Data["Failures"] = value;
      }
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name='failures'>
    /// The collection of validation failures.
    /// </param>
    public ValidationFailureException(ValidationTestList<TTarget> failures) : this(failures, null) {}
    
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name='failures'>
    /// The collection of validation failures.
    /// </param>
    /// <param name='inner'>
    /// An <see cref="System.Exception"/> that was encountered whilst performing validation.
    /// </param>
    public ValidationFailureException(ValidationTestList<TTarget> failures, Exception inner) : base(String.Format("Validation failure of a `{0}'",
                                                                                                                  typeof(TTarget).FullName),
                                                                                                    inner)
    {
      this.Failures = failures;
    }
    
    #endregion
  }
}

