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
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Rules;
using CSF.Validation.ValidationRuns;

namespace CSF.Validation.Manifest
{
  /// <summary>
  /// Represents the manifest information for a single rule.
  /// </summary>
  public class ManifestRule<TRule> : IManifestRule where TRule : IRule
  {
    #region properties

    /// <summary>
    /// Gets the identity associated with the current instance.
    /// </summary>
    /// <value>The identity.</value>
    public virtual object Identity { get; private set; }

    /// <summary>
    /// Gets the <c>System.Type</c> of the rule which this manifest item represents.
    /// </summary>
    /// <value>The type of the rule.</value>
    public virtual Type RuleType => typeof(TRule);

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
    /// Gets a collection of the dependency identifiers.
    /// </summary>
    /// <value>The dependency identifiers.</value>
    public virtual IEnumerable<object> DependencyIdentifiers { get; private set; }

    /// <summary>
    /// Gets the metadata describing the current rule instance.
    /// </summary>
    /// <value>The metadata.</value>
    public virtual IManifestMetadata Metadata { get; private set; }

    #endregion

    #region methods

    /// <summary>
    /// Configures the given rule.
    /// </summary>
    /// <param name="rule">Rule.</param>
    public virtual void Configure(IRule rule)
    {
      if(RuleConfiguration != null)
      {
        RuleConfiguration((TRule) rule);
      }
    }

    #endregion

    #region IManifestRule implementation

    Func<IRule> IManifestRule.RuleFactory
    {
      get {
        if(RuleFactory == null)
          return null;

        return () => RuleFactory();
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ManifestRule{TRule}"/> class.
    /// </summary>
    /// <param name="identity">The rule identity.</param>
    /// <param name="metadata">The rule metadata.</param>
    /// <param name="configuration">An optional configuration action.</param>
    /// <param name="factory">An optional factory function.</param>
    /// <param name="dependencyIdentifiers">An optional collection of dependency identifiers.</param>
    public ManifestRule(object identity,
                        IManifestMetadata metadata,
                        Action<TRule> configuration = null,
                        Func<TRule> factory = null,
                        IEnumerable<object> dependencyIdentifiers = null)
    {
      if(metadata == null)
        throw new ArgumentNullException(nameof(metadata));
      if(identity == null)
        throw new ArgumentNullException(nameof(identity));

      Identity = identity;
      Metadata = metadata;
      RuleConfiguration = configuration;
      RuleFactory = factory;
      DependencyIdentifiers = dependencyIdentifiers?? Enumerable.Empty<object>();
    }

    #endregion
  }
}
