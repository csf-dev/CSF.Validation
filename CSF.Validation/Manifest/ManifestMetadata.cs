//
// ManifestMetadata.cs
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
using System.Reflection;

namespace CSF.Validation.Manifest
{
  /// <summary>
  /// Default implementation of <see cref="IManifestMetadata"/>, which wraps a dictionary of strings and objects.
  /// </summary>
  public class ManifestMetadata : IManifestMetadata
  {
    readonly IDictionary<string,object> metadata;

    /// <summary>
    /// Gets the value with the specified name, or a <c>null</c> reference if it does not exist.
    /// </summary>
    /// <param name="name">The metadata name.</param>
    public object Get(string name)
    {
      if(!metadata.ContainsKey(name))
        return null;

      return metadata[name];
    }

    /// <summary>
    /// Gets the value with the specified name, of the associated type, or a <c>null</c> reference if it does not exist.
    /// Note that this method will return <c>null</c> if called with an incorrect type, even if the item did exist as
    /// an instance of a different type.
    /// </summary>
    /// <param name="name">The metadata name.</param>
    /// <typeparam name="T">The desired metadata object type.</typeparam>
    public T Get<T>(string name) where T : class
    {
      return Get(name) as T;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ManifestMetadata"/> class.
    /// </summary>
    /// <param name="metadata">Metadata.</param>
    public ManifestMetadata(IDictionary<string,object> metadata)
    {
      if(metadata == null)
        throw new ArgumentNullException(nameof(metadata));

      this.metadata = metadata;
    }

    /// <summary>
    /// Creates a manifest metadata instance from the given parameter values.
    /// </summary>
    /// <param name="validatedType">The validated type.</param>
    /// <param name="ruleType">The rule type.</param>
    /// <param name="validatedMember">An optional validated member.</param>
    /// <param name="ruleName">An optional rule name.</param>
    /// <param name="parentIdentity">An optional identity for a parent rule.</param>
    /// <param name="supplementalMetadata">Optional supplemental metadata.</param>
    public static IManifestMetadata Create(Type validatedType,
                                           Type ruleType,
                                           MemberInfo validatedMember = null,
                                           string ruleName = null,
                                           object parentIdentity = null,
                                           IDictionary<string,object> supplementalMetadata = null)
    {
      var metadataDictionary = new Dictionary<string,object>();

      metadataDictionary.Add(StandardMetadataName.ValidatedType, validatedType);
      metadataDictionary.Add(StandardMetadataName.RuleType, ruleType);

      if(!ReferenceEquals(validatedMember, null))
      {
        metadataDictionary.Add(StandardMetadataName.ValidatedMember, validatedMember);
      }

      if(!ReferenceEquals(ruleName, null))
      {
        metadataDictionary.Add(StandardMetadataName.RuleName, ruleName);
      }

      if(!ReferenceEquals(parentIdentity, null))
      {
        metadataDictionary.Add(StandardMetadataName.ParentRuleIdentifier, parentIdentity);
      }

      if(supplementalMetadata != null)
      {
        metadataDictionary = metadataDictionary
          .Union(supplementalMetadata)
          .ToDictionary(k => k.Key, v => v.Value);
      }

      return new ManifestMetadata(metadataDictionary);
    }
  }
}
