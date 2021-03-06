﻿//
// IManifestRule.cs
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

namespace CSF.Validation.Manifest
{
  /// <summary>
  /// Represents a single rule in a manifest of validation rules.
  /// </summary>
  public interface IManifestRule
  {
    /// <summary>
    /// Gets the identity associated with the current instance.
    /// </summary>
    /// <value>The identity.</value>
    object Identity { get; }

    /// <summary>
    /// Gets the <c>System.Type</c> of the rule which this manifest item represents.
    /// </summary>
    /// <value>The type of the rule.</value>
    Type RuleType { get; }

    /// <summary>
    /// Gets an optional function which creates the rule instance.
    /// </summary>
    /// <value>The rule factory.</value>
    Func<IRule> RuleFactory { get; }

    /// <summary>
    /// Configures the given rule.
    /// </summary>
    /// <param name="rule">Rule.</param>
    void Configure(IRule rule);

    /// <summary>
    /// Gets a collection of the dependency identifiers.
    /// </summary>
    /// <value>The dependency identifiers.</value>
    IEnumerable<object> DependencyIdentifiers { get; }

    /// <summary>
    /// Gets the metadata describing the current rule instance.
    /// </summary>
    /// <value>The metadata.</value>
    IManifestMetadata Metadata { get; }
  }
}
