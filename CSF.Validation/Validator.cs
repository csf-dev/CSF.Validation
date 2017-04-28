//
// Validator.cs
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
using System.Collections.Generic;
using CSF.Validation.Options;
using CSF.Validation.ValidationRuns;

namespace CSF.Validation
{
  /// <summary>
  /// Default implementation of <see cref="IValidator"/> which coordinates between components and returns the overall
  /// result.
  /// </summary>
  public class Validator : IValidator
  {
    readonly IValidationRun run;
    readonly IValidationRunner runner;

    /// <summary>
    /// Validate the specified object and get the result.
    /// </summary>
    /// <param name="validated">Validated.</param>
    public virtual IValidationResult Validate(object validated)
    {
      return Validate(validated, ValidationOptionsContainer.Empty);
    }

    /// <summary>
    /// Validate the specified object and get the result.
    /// </summary>
    /// <param name="validated">Validated.</param>
    /// <param name="options">Validation options.</param>
    public virtual IValidationResult Validate(object validated, IValidationOptions options)
    {
      var context = GetContext(validated, options);

      var ruleResults = runner.ExecuteRunAndGetResults(context);

      return GetResult(ruleResults);
    }

    /// <summary>
    /// Gets a validation context for the given validated object and options.
    /// </summary>
    /// <returns>The validation run context.</returns>
    /// <param name="validated">Validated.</param>
    /// <param name="options">Options.</param>
    protected virtual IValidationRunContext GetContext(object validated, IValidationOptions options)
    {
      return new ValidationRunContext(options, validated, run);
    }

    /// <summary>
    /// Gets the overall validation result from the rule results.
    /// </summary>
    /// <returns>The result.</returns>
    /// <param name="ruleResults">Rule results.</param>
    protected virtual IValidationResult GetResult(IEnumerable<IRunnableRuleResult> ruleResults)
    {
      return new ValidationResult(ruleResults);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Validator"/> class.
    /// </summary>
    /// <param name="run">Run.</param>
    /// <param name="runner">Runner.</param>
    public Validator(IValidationRun run,
                     IValidationRunner runner)
    {
      if(runner == null)
        throw new ArgumentNullException(nameof(runner));
      if(run == null)
        throw new ArgumentNullException(nameof(run));

      this.run = run;
      this.runner = runner;
    }
  }
}
