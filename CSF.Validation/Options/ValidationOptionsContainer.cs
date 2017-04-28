﻿//
// ValidationOptionsContainer.cs
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
using System.Linq;

namespace CSF.Validation.Options
{
  /// <summary>
  /// Default implementation of <see cref="IValidationOptions"/> which wraps a collection of objects.
  /// </summary>
  public class ValidationOptionsContainer : IValidationOptions
  {
    static readonly IValidationOptions empty;
    readonly IDictionary<Type,object> options;

    /// <summary>
    /// Gets an option of the given type, or a <c>null</c> reference if no such option is present.
    /// </summary>
    /// <returns>The option.</returns>
    /// <typeparam name="T">The desired option type.</typeparam>
    public T GetOption<T>() where T : class
    {
      var key = typeof(T);

      if(!options.ContainsKey(key))
        return null;

      return options[key] as T;
    }

    /// <summary>
    /// Gets a collection of the options which cause validation rules to be skipped.
    /// </summary>
    /// <returns>The rule skipping options.</returns>
    public IEnumerable<IRuleSkippingOption> GetRuleSkippingOptions()
    {
      return options
        .Values
        .OfType<IRuleSkippingOption>()
        .ToArray();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.Options.ValidationOptionsContainer"/> class.
    /// </summary>
    /// <param name="options">Options.</param>
    public ValidationOptionsContainer(IEnumerable<object> options)
    {
      if(options == null)
        throw new ArgumentNullException(nameof(options));

      this.options = options.ToDictionary(k => k.GetType(), v => v);
    }

    /// <summary>
    /// Initializes the <see cref="T:CSF.Validation.Options.ValidationOptionsContainer"/> class.
    /// </summary>
    static ValidationOptionsContainer()
    {
      empty = new ValidationOptionsContainer(Enumerable.Empty<object>());
    }

    /// <summary>
    /// Gets an empty set of validation options.
    /// </summary>
    /// <value>An empty options container.</value>
    public static IValidationOptions Empty => empty;
  }
}
