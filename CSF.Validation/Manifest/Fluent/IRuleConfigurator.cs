//
// IRuleBuilder.cs
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
using CSF.Validation.Rules;

namespace CSF.Validation.Manifest.Fluent
{
  /// <summary>
  /// Helper type which configures a single validation rule.
  /// </summary>
  public interface IRuleConfigurator
  {
    /// <summary>
    /// Gets the identity of the parent validation rule (if any).
    /// </summary>
    /// <value>The parent rule identity.</value>
    object ParentRuleIdentity { get; }

    /// <summary>
    /// Gets the type of the validated object.
    /// </summary>
    /// <value>The type of the validated object.</value>
    Type ValidatedType { get; }

    /// <summary>
    /// Explicitly sets the identity of this validation rule.
    /// </summary>
    /// <param name="identity">Identity.</param>
    void Identity(object identity);

    /// <summary>
    /// Sets a name for the validation rule.
    /// </summary>
    /// <param name="name">Name.</param>
    void Name(string name);

    /// <summary>
    /// Adds a dependency from the current rule to another validation rule, by its identity.
    /// </summary>
    /// <param name="identity">Identity.</param>
    void AddDependency(object identity);

    /// <summary>
    /// Add an item of supplemental metadata about the current validation rule.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    void AddMetadata(string key, object value);
  }

  /// <summary>
  /// Helper type which configures a single validation rule.
  /// </summary>
  public interface IRuleConfigurator<TValidated,TRule> : IRuleConfigurator
    where TValidated : class
    where TRule : class,IRule
  {
    /// <summary>
    /// Sets a custom factory function which will create the validation rule instance.
    /// </summary>
    /// <param name="factory">Factory.</param>
    void Factory(Func<TRule> factory);

    /// <summary>
    /// Sets a custom callback which will be used to configure the validation rule after it is instantiated.
    /// </summary>
    /// <param name="callback">Callback.</param>
    void Configure(Action<TRule> callback);
  }
}
