//
// DefaultManifestIdentity.cs
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
  /// The default type which is used to represent the identity of a single runnable validation rule in a
  /// validation manifest.
  /// </summary>
  public class DefaultManifestIdentity : IEquatable<DefaultManifestIdentity>
  {
    const string
      ParentIdentitySeparator = " ==> ",
      ComponentSeparator = ".",
      OpenGeneric = "<",
      CloseGeneric = ">",
      GenericSeparator = ",";
    const char GenericIndicator = '`';
    // Just some assorted prime numbers, no real reason I picked these ones
    static readonly int [] Primes = { 97, 263, 467, 773, 977 };

    readonly Type validatedType, ruleType;
    readonly string name;
    readonly MemberInfo validatedMember;
    readonly DefaultManifestIdentity parentIdentity;

    /// <summary>
    /// Gets the type of the validated object.
    /// </summary>
    /// <value>The type of the validated object.</value>
    public Type ValidatedType => validatedType;

    /// <summary>
    /// Gets the validation rule type.
    /// </summary>
    /// <value>The type of the rule.</value>
    public Type RuleType => ruleType;

    /// <summary>
    /// Gets an optional name for the rule.
    /// </summary>
    /// <value>The name.</value>
    public string Name => name;

    /// <summary>
    /// Gets an optional member associated with this validation rule.
    /// </summary>
    /// <value>The validated member.</value>
    public MemberInfo ValidatedMember => validatedMember;

    /// <summary>
    /// Gets an optional identity of a 'parent' validation rule.
    /// </summary>
    /// <value>The parent identity.</value>
    public DefaultManifestIdentity ParentIdentity => parentIdentity;

    /// <summary>
    /// Returns a <see cref="T:System.String"/> that represents the
    /// current <see cref="T:CSF.Validation.Manifest.DefaultManifestIdentity"/>.
    /// </summary>
    /// <returns>A <see cref="T:System.String"/> that represents the current
    /// <see cref="T:CSF.Validation.Manifest.DefaultManifestIdentity"/>.</returns>
    public override string ToString()
    {
      string parent = String.Empty;
      if(ParentIdentity != null)
      {
        parent = String.Concat(ParentIdentity.ToString(), ParentIdentitySeparator);
      }

      var type = FormatTypeName(ValidatedType);
      var member = (ValidatedMember != null)? ValidatedMember.Name : null;
      var rule = FormatTypeName(RuleType);

      var components = new [] { type, member, rule, name }
        .Where(x => !String.IsNullOrEmpty(x))
        .ToArray();

      return String.Concat(parent, String.Join(ComponentSeparator, components));
    }

    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal to the current
    /// <see cref="T:CSF.Validation.Manifest.DefaultManifestIdentity"/>.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> to compare with the current
    /// <see cref="T:CSF.Validation.Manifest.DefaultManifestIdentity"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
    /// <see cref="T:CSF.Validation.Manifest.DefaultManifestIdentity"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
      return Equals(obj as DefaultManifestIdentity);
    }

    /// <summary>
    /// Determines whether the specified <see cref="DefaultManifestIdentity"/> is equal to the
    /// current <see cref="DefaultManifestIdentity"/>.
    /// </summary>
    /// <param name="other">The <see cref="DefaultManifestIdentity"/> to 
    /// compare with the current <see cref="DefaultManifestIdentity"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="DefaultManifestIdentity"/> is equal to the
    /// current <see cref="DefaultManifestIdentity"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(DefaultManifestIdentity other)
    {
      if(ReferenceEquals(this, other))
        return true;
      
      if(Equals(other, null))
        return false;

      if(Equals(ValidatedType, other.ValidatedType)
         && Equals(RuleType, other.RuleType)
         && Equals(Name, other.Name)
         && Equals(ValidatedMember, other.ValidatedMember)
         && Equals(ParentIdentity, other.ParentIdentity))
        return true;

      return false;
    }

    /// <summary>
    /// Serves as a hash function for a
    /// <see cref="DefaultManifestIdentity"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in hashing algorithms
    /// and data structures such as a hash table.</returns>
    public override int GetHashCode()
    {
      var components = GetHashCodeComponents();

      return components.Aggregate(0, (next, acc) => acc ^ next);
    }

    string FormatTypeName(Type type)
    {
      if(type == null)
        throw new ArgumentNullException(nameof(type));

      var output = type.Name;

      if(type.IsGenericType)
      {
        var typeName = output.Split(GenericIndicator)[0];
        var genericNames = type.GenericTypeArguments.Select(x => FormatTypeName(x));
        output = String.Concat(typeName, OpenGeneric, String.Join(GenericSeparator, genericNames), CloseGeneric);
      }

      return output;
    }

    IEnumerable<int> GetHashCodeComponents()
    {
      return new [] {
        MultiplyUnchecked(ValidatedType.GetHashCode(), Primes[0]),
        MultiplyUnchecked(RuleType.GetHashCode(), Primes[1]),
        MultiplyUnchecked((Name?.GetHashCode()).GetValueOrDefault(1), Primes[2]),
        MultiplyUnchecked((ValidatedMember?.GetHashCode()).GetValueOrDefault(1), Primes[3]),
        MultiplyUnchecked((ParentIdentity?.GetHashCode()).GetValueOrDefault(1), Primes[4]),
      };
    }

    int MultiplyUnchecked(int first, int second)
    {
      unchecked
      {
        return first * second;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultManifestIdentity"/> class.
    /// </summary>
    /// <param name="validatedType">Validated type.</param>
    /// <param name="ruleType">Rule type.</param>
    /// <param name="name">Name.</param>
    /// <param name="validatedMember">Validated member.</param>
    /// <param name="parentIdentity">Parent identity.</param>
    public DefaultManifestIdentity(Type validatedType,
                                   Type ruleType,
                                   string name = null,
                                   MemberInfo validatedMember = null,
                                   DefaultManifestIdentity parentIdentity = null)
    {
      if(ruleType == null)
        throw new ArgumentNullException(nameof(ruleType));
      if(validatedType == null)
        throw new ArgumentNullException(nameof(validatedType));
      
      this.validatedType = validatedType;
      this.ruleType = ruleType;
      this.name = name;
      this.validatedMember = validatedMember;
      this.parentIdentity = parentIdentity;
    }
  }
}
