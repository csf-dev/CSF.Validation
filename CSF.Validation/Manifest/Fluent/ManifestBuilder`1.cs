//
// ManifestBuilder.cs
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
using System.Linq.Expressions;
using System.Reflection;
using CSF.Reflection;
using CSF.Validation.Rules;

namespace CSF.Validation.Manifest.Fluent
{
  class ManifestBuilder<TValidated> : IManifestBuilder<TValidated> where TValidated : class
  {
    #region fields

    readonly object parentRuleIdentity;
    readonly ISet<IManifestRule> rules;

    #endregion

    #region properties

    /// <summary>
    /// Creates and returns a validation manifest from the current builder instance.
    /// </summary>
    /// <returns>The manifest.</returns>
    public IValidationManifest GetManifest() => new ValidationManifest(rules);

    #endregion

    #region methods

    void AddValueRule<TRule,TValue>(Func<TValidated, TValue> valueAccessor,
                                    Expression<Func<TValidated, TValue>> memberExpression,
                                    Action<IRuleConfigurator<TValidated, TRule>> configuration)
      where TRule : class,IValueRule<TValidated,TValue>
    {
      if(valueAccessor == null)
        throw new ArgumentNullException(nameof(valueAccessor));

      var configurator = new ValueRuleConfigurator<TValidated,TRule,TValue>(parentRuleIdentity, valueAccessor);
      if(configuration != null)
      {
        configuration(configurator);
      }

      rules.Add(CreateManifestRule(configurator, GetMember(memberExpression)));
    }

    void AddRule<TRule>(Action<IRuleConfigurator<TValidated, TRule>> configuration)
      where TRule : class,IRule
    {
      var configurator = new RuleConfigurator<TValidated,TRule>(parentRuleIdentity);
      if(configuration != null)
      {
        configuration(configurator);
      }

      rules.Add(CreateManifestRule(configurator));
    }

    IManifestRule CreateManifestRule<TRule>(RuleConfigurator<TValidated,TRule> configurator,
                                            MemberInfo member = null)
      where TRule : class,IRule
    {
      if(configurator == null)
        throw new ArgumentNullException(nameof(configurator));
      
      var metadata = ManifestMetadata.Create(typeof(TValidated),
                                             typeof(TRule),
                                             member,
                                             configurator.RuleName,
                                             configurator.ParentRuleIdentity,
                                             configurator.SupplementalMetadata);

      return new ManifestRule<TRule>(metadata,
                                     configurator.ManifestIdentity,
                                     configurator.ConfigurationCallback,
                                     configurator.FactoryFunction,
                                     configurator.Dependencies);
    }

    MemberInfo GetMember<TValue>(Expression<Func<TValidated, TValue>> memberExpression)
    {
      if(memberExpression != null)
      {
        var member = Reflect.Member(memberExpression);
        EnsureIsMemberOfValidatedType(member);
        return member;
      }

      return null;
    }

    void EnsureIsMemberOfValidatedType(MemberInfo member)
    {
      if(member == null)
        throw new ArgumentNullException(nameof(member));

      if(member.ReflectedType != typeof(TValidated))
      {
        string message = String.Format(Resources.ExceptionMessages.MemberMustBelongToValidatedType,
                                       typeof(TValidated).Name,
                                       member.Name);
        throw new ArgumentException(message, nameof(member));
      }
    }

    #endregion

    #region IManifestBuilder implementation

    void IManifestBuilder<TValidated>.AddMemberRule<TRule, TValue>(Expression<Func<TValidated, TValue>> memberExpression,
                                                                   Func<TValidated,TValue,TRule> ruleDelegate)
    {
      AddValueRule<TRule, TValue>(memberExpression.Compile(), memberExpression, null);
    }

    void IManifestBuilder<TValidated>.AddMemberRule<TRule, TValue>(Expression<Func<TValidated, TValue>> memberExpression,
                                                                   Func<TValidated,TValue,TRule> ruleDelegate,
                                                                   Action<IRuleConfigurator<TValidated, TRule>> configuration)
    {
      AddValueRule<TRule, TValue>(memberExpression.Compile(), memberExpression, configuration);
    }

    void IManifestBuilder<TValidated>.AddRule<TRule>(Func<TValidated,TRule> ruleDelegate)
    {
      AddRule<TRule>(null);
    }

    void IManifestBuilder<TValidated>.AddRule<TRule>(Func<TValidated,TRule> ruleDelegate,
                                                     Action<IRuleConfigurator<TValidated, TRule>> configuration)
    {
      AddRule<TRule>(configuration);
    }

    void IManifestBuilder<TValidated>.AddValueRule<TRule, TValue>(Func<TValidated, TValue> valueAccessor,
                                                                  Func<TValidated,TValue,TRule> ruleDelegate)
    {
      AddValueRule<TRule, TValue>(valueAccessor, null, null);
    }

    void IManifestBuilder<TValidated>.AddValueRule<TRule, TValue>(Func<TValidated, TValue> valueAccessor,
                                                                  Func<TValidated,TValue,TRule> ruleDelegate,
                                                                  Action<IRuleConfigurator<TValidated, TRule>> configuration)
    {
      AddValueRule(valueAccessor, null, configuration);
    }

    #endregion

    #region constructor

    internal ManifestBuilder(object parentIdentity = null)
    {
      this.parentRuleIdentity = parentIdentity;
      this.rules = new HashSet<IManifestRule>();
    }

    #endregion
  }
}
