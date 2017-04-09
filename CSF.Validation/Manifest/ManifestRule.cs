//
// ManifestRule.cs
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
using CSF.Validation.ValidationRuns;

namespace CSF.Validation.Manifest
{
  /// <summary>
  /// Represents the manifest information for a single rule.
  /// </summary>
  public class ManifestRule<TRule> : IManifestRule where TRule : IRule
  {
    /// <summary>
    /// Gets the identity associated with the current instance.
    /// </summary>
    /// <value>The identity.</value>
    public virtual object Identity { get; private set; }

    /// <summary>
    /// Gets an optional operation which performs configuration upon the rule before it is executed.
    /// </summary>
    /// <value>The rule configuration.</value>
    public virtual Action<TRule> RuleConfiguration { get; private set; }

    /// <summary>
    /// Gets an optional operation which constructs the rule instance.
    /// </summary>
    /// <value>The rule factory.</value>
    public virtual Func<TRule> RuleFactory { get; private set; }

    /// <summary>
    /// Gets the options which apply to the current rule in the manifest.
    /// </summary>
    /// <value>The options.</value>
    public RuleOptions Options { get; private set; }

    /// <summary>
    /// Creates the rule, ready to be executed.
    /// </summary>
    /// <returns>The runnable rule.</returns>
    public virtual IRunnableRule CreateRunnableRule()
    {
      // var rule = GetConfiguredRule();

      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    /// <summary>
    /// Creates and returns the <see cref="IRule"/> instance, in a fully-configured state.
    /// </summary>
    /// <returns>The configured rule.</returns>
    protected virtual TRule GetConfiguredRule()
    {
      var rule = CreateRule();
      ConfigureRule(rule);
      return rule;
    }

    /// <summary>
    /// Creates the rule, either from a factory method, or from a parameterless constructor.
    /// </summary>
    /// <returns>The rule.</returns>
    protected virtual TRule CreateRule()
    {
      if(RuleFactory != null)
      {
        return RuleFactory();
      }

      return Activator.CreateInstance<TRule>();
    }

    /// <summary>
    /// Configures the rule, using the <see cref="RuleConfiguration"/> delegate if provided.
    /// </summary>
    /// <param name="rule">Rule.</param>
    protected virtual void ConfigureRule(TRule rule)
    {
      if(RuleConfiguration != null)
      {
        RuleConfiguration(rule);
      }
    }
  }
}
