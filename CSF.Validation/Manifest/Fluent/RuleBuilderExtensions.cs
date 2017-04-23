//
// RuleBuilderExtensions.cs
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
using System.Reflection;
using CSF.Reflection;
using CSF.Validation.Rules;

namespace CSF.Validation.Manifest.Fluent
{
  /// <summary>
  /// Extension methods for <see cref="T:IRuleBuilder{TValidated,TRule}"/>.
  /// </summary>
  public static class RuleBuilderExtensions
  {
    /// <summary>
    /// Adds a dependency to the manifest rule builder based upon the same validated type, identifying the depended-upon
    /// rule via properties of its <see cref="DefaultManifestIdentity"/>.
    /// </summary>
    /// <param name="builder">The rule builder.</param>
    /// <param name="ruleDelegate">An expression or delegate used to indicate the rule type.
    /// This method is not executed, it is only used to determine the rule type.</param>
    /// <param name="name">The name of the depended-upon rule.</param>
    /// <typeparam name="TValidated">The validated type.</typeparam>
    /// <typeparam name="TRule">The type of the current validation rule.</typeparam>
    /// <typeparam name="TOtherRule">The type of the validation rule being depended upon.</typeparam>
    public static void AddDependency<TOtherRule>(this IRuleConfigurator builder,
                                                                  string name = null)
      where TOtherRule : class,IRule
    {
      AddDependency<TOtherRule>(builder, null, name);
    }

    /// <summary>
    /// Adds a dependency to the manifest rule builder based upon the same validated type, identifying the depended-upon
    /// rule via properties of its <see cref="DefaultManifestIdentity"/>.
    /// </summary>
    /// <param name="builder">The rule builder.</param>
    /// <param name="ruleDelegate">An expression or delegate used to indicate the rule type.
    /// This method is not executed, it is only used to determine the rule type.</param>
    /// <param name="memberExpression">An expression identifying the member for the depended-upon rule.</param>
    /// <param name="name">The name of the depended-upon rule.</param>
    /// <typeparam name="TValidated">The validated type.</typeparam>
    /// <typeparam name="TValue">The type of value which <typeparamref name="TOtherRule"/> validates.</typeparam>
    /// <typeparam name="TRule">The type of the current validation rule.</typeparam>
    /// <typeparam name="TOtherRule">The type of the validation rule being depended upon.</typeparam>
    public static void AddDependency<TOtherRule,TValidated>(this IRuleConfigurator builder,
                                                            Expression<Func<TValidated,object>> memberExpression,
                                                            string name = null)
      where TOtherRule : class,IRule
      where TValidated : class
    {
      var member = Reflect.Member(memberExpression);
      AddDependency<TOtherRule>(builder, member, name);
    }

    /// <summary>
    /// Adds a dependency to the manifest rule builder based upon the same validated type, identifying the depended-upon
    /// rule via properties of its <see cref="DefaultManifestIdentity"/>.
    /// </summary>
    /// <param name="builder">The rule builder.</param>
    /// <param name="member">The member for the depended-upon rule.</param>
    /// <param name="name">The name of the depended-upon rule.</param>
    /// <typeparam name="TValidated">The validated type.</typeparam>
    /// <typeparam name="TRule">The type of the current validation rule.</typeparam>
    /// <typeparam name="TOtherRule">The type of the validation rule being depended upon.</typeparam>
    static void AddDependency<TOtherRule>(IRuleConfigurator builder,
                                          MemberInfo member,
                                          string name = null)
      where TOtherRule : class,IRule
    {
      if(builder == null)
        throw new ArgumentNullException(nameof(builder));
      if(!ReferenceEquals(builder.ParentRuleIdentity, null)
         && !(builder.ParentRuleIdentity is DefaultManifestIdentity))
      {
        var message = String.Format(Resources.ExceptionMessages.ParentRuleIdentityMustBeTypeOrNull,
                                    typeof(DefaultManifestIdentity).Name);
        throw new ArgumentException(message, nameof(builder));
      }
        
      var identity = new DefaultManifestIdentity(builder.ValidatedType,
                                                 typeof(TOtherRule),
                                                 name,
                                                 member,
                                                 (DefaultManifestIdentity) builder.ParentRuleIdentity);
      builder.AddDependency(identity);
    }
  }
}
