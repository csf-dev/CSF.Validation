//
// ValidationRunContext.cs
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
using CSF.Validation.Options;

namespace CSF.Validation.ValidationRuns
{
  /// <summary>
  /// Default implementation of <see cref="IValidationRunContext"/>.
  /// </summary>
  public class ValidationRunContext : IValidationRunContext
  {
    readonly IValidationOptions options;
    readonly object validated;
    readonly IValidationRun validationRun;

    /// <summary>
    /// Gets a reference to the validation run with which the current context is associated.
    /// </summary>
    /// <value>The validation run.</value>
    public IValidationOptions Options => options;

    /// <summary>
    /// Gets a reference to the object under validation.
    /// </summary>
    /// <value>The validated object.</value>
    public object Validated => validated;

    /// <summary>
    /// Gets the options applied to the current validation run.
    /// </summary>
    public IValidationRun ValidationRun => validationRun;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.ValidationRuns.ValidationRunContext"/> class.
    /// </summary>
    /// <param name="options">Options.</param>
    /// <param name="validated">Validated.</param>
    /// <param name="validationRun">Validation run.</param>
    public ValidationRunContext(IValidationOptions options,
                                object validated,
                                IValidationRun validationRun)
    {
      if(validationRun == null)
        throw new ArgumentNullException(nameof(validationRun));
      if(options == null)
        throw new ArgumentNullException(nameof(options));

      this.options = options;
      this.validated = validated;
      this.validationRun = validationRun;
    }
  }
}
