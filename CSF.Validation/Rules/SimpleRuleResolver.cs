﻿//
// RuleResolver.cs
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
using CSF.Validation.Manifest;

namespace CSF.Validation.Rules
{
  /// <summary>
  /// Simple implementation of <see cref="IRuleResolver"/> which makes use of a provided factory method if available, but
  /// otherwise makes use of a public parameterless constructor.
  /// </summary>
  public class SimpleRuleResolver : IRuleResolver
  {
    /// <summary>
    /// Resolve an <see cref="IRule"/> instance from its corresponding manifest item.
    /// </summary>
    /// <param name="manifest">The manifest item.</param>
    public IRule Resolve(IManifestRule manifest)
    {
      if(manifest == null)
        throw new ArgumentNullException(nameof(manifest));

      if(manifest.RuleFactory != null)
        return manifest.RuleFactory();

      return (IRule) Activator.CreateInstance(manifest.RuleType);
    }
  }
}
