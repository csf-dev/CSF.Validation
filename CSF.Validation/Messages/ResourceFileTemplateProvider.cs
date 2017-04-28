//
// ResourceFileTemplateProvider.cs
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
using System.Reflection;
using System.Resources;
using CSF.Validation.ValidationRuns;

namespace CSF.Validation.Messages
{
  /// <summary>
  /// Implementation of <see cref="ITemplateProvider"/> which uses an assembly manifest resource to
  /// get the message templates.
  /// </summary>
  public class ResourceFileTemplateProvider : ITemplateProvider
  {
    ResourceManager resourceManager;

    /// <summary>
    /// Gets the message template based upon the manifest identity of the rule result.
    /// </summary>
    /// <returns>The template.</returns>
    /// <param name="result">Result.</param>
    public virtual string GetTemplate(IRunnableRuleResult result)
    {
      if(result == null)
        throw new ArgumentNullException(nameof(result));

      var key = GetResourceKey(result);
      if(key == null)
        return null;

      var resMan = GetResourceManager();

      return resMan.GetString(key);
    }

    /// <summary>
    /// Gets the string key for the resource in the manifest.
    /// </summary>
    /// <returns>The resource key.</returns>
    /// <param name="result">Result.</param>
    protected virtual string GetResourceKey(IRunnableRuleResult result)
    {
      return result?.ManifestIdentity?.ToString();
    }

    /// <summary>
    /// Gets a resource manager for the manifest resource which contains the message templates.
    /// </summary>
    /// <returns>The resource manager.</returns>
    protected virtual ResourceManager GetResourceManager()
    {
      if(resourceManager == null)
      {
        var type = GetType();
        resourceManager = new ResourceManager(type.FullName, Assembly.GetAssembly(type));
      }

      return resourceManager;
    }
  }
}
