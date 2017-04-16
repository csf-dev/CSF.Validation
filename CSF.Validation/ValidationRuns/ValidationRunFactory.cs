//
// ValidationRunFactory.cs
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
using CSF.Validation.Manifest;
using CSF.Validation.Rules;

namespace CSF.Validation.ValidationRuns
{
  /// <summary>
  /// Default implementation of <see cref="IValidationRunFactory"/>.
  /// </summary>
  public class ValidationRunFactory : IValidationRunFactory
  {
    readonly IRuleResolver resolver;
    readonly IManifestIdentityGenerator identityGenerator;

    /// <summary>
    /// Creates the run from the given manifest.
    /// </summary>
    /// <returns>The constructed validation run.</returns>
    /// <param name="manifest">A validation manifest.</param>
    public virtual IValidationRun CreateRun(IValidationManifest manifest)
    {
      if(manifest == null)
        throw new ArgumentNullException(nameof(manifest));

      var rules = CreateRunnableRules(manifest.Rules);
      return new ValidationRun(rules);
    }

    /// <summary>
    /// Creates a collection of <see cref="IRunnableRule"/> from a collection of <see cref="IManifestRule"/>.
    /// </summary>
    /// <returns>The runnable rules.</returns>
    /// <param name="manifestRules">Manifest rules.</param>
    public virtual IEnumerable<IRunnableRule> CreateRunnableRules(IEnumerable<IManifestRule> manifestRules)
    {
      if(manifestRules == null)
        throw new ArgumentNullException(nameof(manifestRules));
      
      var manifestsAndRules = manifestRules
        .Select(x => new { Manifest = x, Rule = CreateRunnableRule(x) })
        .ToArray();
      var allRules = manifestsAndRules
        .Select(x => x.Rule)
        .ToArray();

      foreach(var manifestAndRule in manifestsAndRules)
      {
        FindAndProvideDependencies(manifestAndRule.Rule, manifestAndRule.Manifest, allRules);
      }

      return allRules;
    }

    /// <summary>
    /// Creates and returns a single <see cref="IRunnableRule"/> from a <see cref="IManifestRule"/>.
    /// </summary>
    /// <returns>The runnable rule.</returns>
    /// <param name="manifestRule">Manifest rule.</param>
    public virtual IRunnableRule CreateRunnableRule(IManifestRule manifestRule)
    {
      if(manifestRule == null)
        throw new ArgumentNullException(nameof(manifestRule));

      var rule = CreateRule(manifestRule);

      var identity = manifestRule.Identity?? identityGenerator.GetIdentity(manifestRule);

      return new RunnableRule(identity, manifestRule.Metadata, rule);
    }

    /// <summary>
    /// Creates the underlying <see cref="IRule"/> instance from a <see cref="IManifestRule"/>.
    /// </summary>
    /// <returns>The rule.</returns>
    /// <param name="manifestRule">Manifest rule.</param>
    public virtual IRule CreateRule(IManifestRule manifestRule)
    {
      if(manifestRule == null)
        throw new ArgumentNullException(nameof(manifestRule));

      var rule = resolver.Resolve(manifestRule);

      manifestRule.Configure(rule);

      return rule;
    }

    /// <summary>
    /// Finds the rules which are dependencies for the given rule (based upon the identifiers for its dependencies),
    /// and then provides these dependencies to the rule via <see cref="IRunnableRule.ProvideDependencies"/>.
    /// </summary>
    /// <param name="currentRule">The rule for which we are providing dependencies.</param>
    /// <param name="manifest">The manifest for the current rule.</param>
    /// <param name="allRules">A collection of all of the rules in the parent manifest.</param>
    public virtual void FindAndProvideDependencies(IRunnableRule currentRule,
                                                   IManifestRule manifest,
                                                   IEnumerable<IRunnableRule> allRules)
    {
      var dependencies = (from rule in allRules
                          join identity in manifest.DependencyIdentifiers
                            on rule.Identity equals identity
                          select rule)
        .ToArray();
      
      currentRule.ProvideDependencies(dependencies);
    }
  }
}
