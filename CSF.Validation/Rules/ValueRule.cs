﻿//
// PropertyRule.cs
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
  /// A validation rule which operates upon a single value (such as the value of a single object member, like
  /// a property).
  /// </summary>
  public abstract class ValueRule<TValidated,TValue> : Rule<TValidated>
  {
    /// <summary>
    /// Gets or sets the accessor for that value, based upon the object under validation.
    /// </summary>
    /// <value>The accessor.</value>
    public virtual Func<TValidated,TValue> Accessor { get; set; }

    /// <summary>
    /// Sealed implementation of <see cref="M:GetOutcome(TValidated)"/>.
    /// </summary>
    /// <returns>The outcome.</returns>
    /// <param name="validated">The object undergoing validation.</param>
    protected sealed override RuleOutcome GetOutcome(TValidated validated)
    {
      if(Accessor == null)
        throw new InvalidOperationException(Resources.ExceptionMessages.AccessorFunctionMustNotBeNull);

      var value = Accessor(validated);

      return GetOutcome(validated, value);
    }

    /// <summary>
    /// Gets the outcome of the validation, override this method in derived types.
    /// </summary>
    /// <returns>The outcome.</returns>
    /// <param name="validated">The object undergoing validation.</param>
    /// <param name="value">The value for validation.</param>
    protected abstract RuleOutcome GetOutcome(TValidated validated, TValue value);
  }
}
