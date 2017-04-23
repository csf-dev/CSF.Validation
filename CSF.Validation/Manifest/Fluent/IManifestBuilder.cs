//
// IFluentManifestBuilder.cs
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
using System.Linq.Expressions;
using CSF.Validation.Rules;

namespace CSF.Validation.Manifest.Fluent
{
  /// <summary>
  /// Builder type, providing a fluent interface, for creating validation manifests in code.
  /// </summary>
  public interface IManifestBuilder<TValidated>
    where TValidated : class
  {
    /// <summary>
    /// Creates and returns a validation manifest from the current builder instance.
    /// </summary>
    /// <returns>The manifest.</returns>
    IValidationManifest GetManifest();

    /// <summary>
    /// Adds a validation rule to the manifest.
    /// </summary>
    /// <typeparam name="TRule">The valiation rule type.</typeparam>
    void AddRule<TRule>()
      where TRule : class,IRule;

    /// <summary>
    /// Adds a validation rule to the manifest.
    /// </summary>
    /// <param name="configuration">A callback which configures the rule in the manifest.</param>
    /// <typeparam name="TRule">The valiation rule type.</typeparam>
    void AddRule<TRule>(Action<IRuleConfigurator<TValidated,TRule>> configuration)
      where TRule : class,IRule;

    /// <summary>
    /// Adds a validation rule to the manifest.
    /// </summary>
    /// <param name="accessor">A function which gets the value from the validated type.</param>
    /// <typeparam name="TRule">The valiation rule type.</typeparam>
    void AddValueRule<TRule>(Func<TValidated,object> accessor)
      where TRule : class,IValueRule;

    /// <summary>
    /// Adds a validation rule to the manifest.
    /// </summary>
    /// <param name="accessor">A function which gets the value from the validated type.</param>
    /// <param name="configuration">A callback which configures the rule in the manifest.</param>
    /// <typeparam name="TRule">The valiation rule type.</typeparam>
    void AddValueRule<TRule>(Func<TValidated,object> accessor,
                             Action<IRuleConfigurator<TValidated,TRule>> configuration)
      where TRule : class,IValueRule;

    /// <summary>
    /// Adds a validation rule to the manifest, where the rule is specific to a value returned from a member of the
    /// validated type.
    /// </summary>
    /// <param name="memberExpression">An expression which indicates the member whose value is to be validated.</param>
    /// <typeparam name="TRule">The valiation rule type.</typeparam>
    void AddMemberRule<TRule>(Expression<Func<TValidated,object>> memberExpression)
      where TRule : class,IValueRule;

    /// <summary>
    /// Adds a validation rule to the manifest, where the rule is specific to a value returned from a member of the
    /// validated type.
    /// </summary>
    /// <param name="memberExpression">An expression which indicates the member whose value is to be validated.</param>
    /// <param name="configuration">A callback which configures the rule in the manifest.</param>
    /// <typeparam name="TRule">The valiation rule type.</typeparam>
    void AddMemberRule<TRule>(Expression<Func<TValidated,object>> memberExpression,
                              Action<IRuleConfigurator<TValidated,TRule>> configuration)
      where TRule : class,IValueRule;
  }
}
