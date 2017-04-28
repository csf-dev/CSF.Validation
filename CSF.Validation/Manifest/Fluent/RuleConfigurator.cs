//
// RuleConfigurator.cs
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
using CSF.Validation.Rules;

namespace CSF.Validation.Manifest.Fluent
{
  internal class RuleConfigurator<TValidated, TRule> : IRuleConfigurator<TValidated, TRule>
    where TValidated : class
    where TRule : class, IRule
  {
    #region fields

    readonly object parentRuleIdentity;
    readonly IDictionary<string,object> supplementalMetadata;
    ISet<object> dependencies;
    Action<TRule> configurationCallback;
    Func<TRule> factory;
    object identity;
    string name;

    #endregion

    #region IRuleConfigurator implementation

    public Type ValidatedType => typeof(TValidated);

    public object ParentRuleIdentity => parentRuleIdentity;

    public void AddDependency(object identity)
    {
      if(identity == null)
        throw new ArgumentNullException(nameof(identity));

      this.dependencies.Add(identity);
    }

    public void Name(string name)
    {
      if(name == null)
        throw new ArgumentNullException(nameof(name));

      this.name = name;
    }

    public void Configure(Action<TRule> configurationCallback)
    {
      if(configurationCallback == null)
        throw new ArgumentNullException(nameof(configurationCallback));

      this.configurationCallback = configurationCallback;
    }

    public void Factory(Func<TRule> factory)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));

      this.factory = factory;
    }

    public void Identity(object identity)
    {
      if(identity == null)
        throw new ArgumentNullException(nameof(identity));

      this.identity = identity;
    }

    public void AddMetadata(string key, object value)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      this.supplementalMetadata.Add(key, value);
    }

    #endregion

    #region properties

    internal virtual IEnumerable<object> Dependencies => dependencies;

    internal virtual Action<TRule> ConfigurationCallback => configurationCallback;

    internal virtual Func<TRule> FactoryFunction => factory;

    internal virtual object ManifestIdentity => identity;

    internal virtual IDictionary<string,object> SupplementalMetadata => supplementalMetadata;

    internal virtual string RuleName => name;

    #endregion

    #region constructor

    internal RuleConfigurator(object parentRuleIdentity)
    {
      this.parentRuleIdentity = parentRuleIdentity;
      this.supplementalMetadata = new Dictionary<string,object>();
      this.dependencies = new HashSet<object>();
    }

    #endregion
  }
}
