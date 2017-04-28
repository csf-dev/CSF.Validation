//
// RunnableRule.cs
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
  /// Concrete implementation of <see cref="IRunnableRule"/> which represents a validation rule which
  /// is a part of an overall validation run.
  /// </summary>
  public class RunnableRule : IRunnableRule
  {
    #region fields

    readonly IRule underlyingRule;
    readonly object manifestIdentity;
    readonly IManifestMetadata metadata;
    IRuleResult result;
    object validated;
    IEnumerable<IRunnableRule> dependencies;

    #endregion

    #region properties

    /// <summary>
    /// Gets a value indicating whether this rule has already been executed in the context of the current
    /// validation operation and thus has a result which may be observed via <see cref="GetResult"/>.
    /// </summary>
    /// <value><c>true</c> if the rule has already been executed; otherwise, <c>false</c>.</value>
    public virtual bool HasResult
    { get { return result != null; } }

    /// <summary>
    /// Gets a value indicating whether or not the rule wrapped by the current instance may be executed or not.
    /// </summary>
    /// <value><c>true</c> if the current instance may be executed; otherwise, <c>false</c>.</value>
    public virtual bool MayBeExecuted
    {
      get {
        // Already been executed
        if(HasResult)
          return false;

        // Has dependencies which themselves haven't been executed
        if(Dependencies.Any(x => !x.HasResult))
          return false;

        return true;
      }
    }

    /// <summary>
    /// Gets the identity for the current instance.
    /// </summary>
    /// <value>The identity.</value>
    public virtual object Identity => manifestIdentity;

    /// <summary>
    /// Gets the metadata describing the rule in its original manifest.
    /// </summary>
    /// <value>The metadata.</value>
    public virtual IManifestMetadata Metadata => metadata;

    /// <summary>
    /// Gets a collection of the dependency rules.  Returns an empty collection if
    /// dependencies have not yet been provided.
    /// </summary>
    /// <value>The dependencies.</value>
    protected virtual IEnumerable<IRunnableRule> Dependencies => dependencies?? Enumerable.Empty<IRunnableRule>();

    #endregion

    #region methods

    /// <summary>
    /// Execute the current rule, for the given validated object.
    /// </summary>
    /// <param name="validated">Validated.</param>
    /// <exception cref="InvalidOperationException">If <see cref="HasResult"/> is <c>true</c>.</exception>
    public virtual void Execute(object validated)
    {
      if(!MayBeExecuted)
        throw new InvalidOperationException(Resources.ExceptionMessages.RuleMayNotBeExecuted);

      this.validated = validated;

      if(Dependencies.Any(IsDependencyFailure))
      {
        result = new RuleResult(RuleOutcome.SkippedDueToDependencyFailure);
        return;
      }

      result = underlyingRule.GetResult(validated);
    }

    /// <summary>
    /// Intentionally skips execution of the current rule instance, marking it as such.
    /// </summary>
    /// <param name="validated">Validated.</param>
    public virtual void IntentionallySkip(object validated)
    {
      if(!MayBeExecuted)
        throw new InvalidOperationException(Resources.ExceptionMessages.RuleMayNotBeExecuted);

      this.validated = validated;

      result = new RuleResult(RuleOutcome.IntentionallySkipped);
    }

    /// <summary>
    /// Gets the result of the rule's execution.
    /// </summary>
    /// <returns>The result.</returns>
    /// <exception cref="InvalidOperationException">If <see cref="HasResult"/> is <c>false</c>.</exception>
    public virtual IRunnableRuleResult GetResult()
    {
      if(!HasResult)
      {
        string message = String.Format(Resources.ExceptionMessages.RuleMustHaveAResult, nameof(HasResult));
        throw new InvalidOperationException(message);
      }

      return new RunnableRuleResult(manifestIdentity, result, validated);
    }

    /// <summary>
    /// Gets a collection of the dependencies for the current instance.
    /// </summary>
    /// <returns>The dependencies.</returns>
    public virtual IEnumerable<IRunnableRule> GetDependencies()
    {
      if(dependencies == null)
      {
        string message = String.Format(Resources.ExceptionMessages.DependenciesCannotBeRetrievedUntilProvided,
                                       nameof(GetDependencies),
                                       nameof(ProvideDependencies));
        throw new InvalidOperationException(message);
      }

      return dependencies;
    }

    /// <summary>
    /// A call-once method which sets the dependencies for the current instance to the given collection of rules.
    /// </summary>
    /// <param name="rules">The other rules which the current instance depends-upon.</param>
    public virtual void ProvideDependencies(IEnumerable<IRunnableRule> rules)
    {
      if(dependencies != null)
      {
        string message = String.Format(Resources.ExceptionMessages.ProvideDependenciesMayOnlyBeCalledOnce,
                                       nameof(ProvideDependencies),
                                       typeof(IRunnableRule).Name);
        throw new InvalidOperationException(message);
      }

      dependencies = rules;
    }

    /// <summary>
    /// Determines whether or not the state of the given dependency indicates that the current rule should not be executed.
    /// </summary>
    /// <returns><c>true</c>, if execution was blocksed, <c>false</c> otherwise.</returns>
    /// <param name="dependency">Dependency.</param>
    protected virtual bool IsDependencyFailure(IRunnableRule dependency)
    {
      if(dependency == null)
        throw new ArgumentNullException(nameof(dependency));

      if(!dependency.HasResult)
        throw new ArgumentException(Resources.ExceptionMessages.DependencyMustHaveAResult, nameof(dependency));

      var outcome = dependency.GetResult().RuleResult.Outcome;

      switch(outcome)
      {
      case RuleOutcome.Success:
        return false;
      default:
        return true;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.ValidationRuns.RunnableRule"/> class.
    /// </summary>
    /// <param name="manifestIdentity">Manifest identity.</param>
    /// <param name="rule">Rule.</param>
    /// <param name="metadata">The rule metadata in its manifest.</param>
    public RunnableRule(object manifestIdentity,
                        IManifestMetadata metadata,
                        IRule rule)
    {
      if(metadata == null)
        throw new ArgumentNullException(nameof(metadata));
      if(rule == null)
        throw new ArgumentNullException(nameof(rule));
      if(manifestIdentity == null)
        throw new ArgumentNullException(nameof(manifestIdentity));

      this.manifestIdentity = manifestIdentity;
      this.underlyingRule = rule;
      this.metadata = metadata;
    }

    #endregion
  }
}
