//
// StandardMetadataName.cs
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
namespace CSF.Validation.Manifest
{
  /// <summary>
  /// Enumerates a number of <c>string</c> values which represent some standard pieces of well-known metadata.
  /// </summary>
  public static class StandardMetadataName
  {
    static readonly string
      validatedType = "Validated type",
      ruleType = "Rule type",
      ruleName = "Rule name",
      validatedMember = "Validated member",
      parentRule = "Parent rule manifest";

    /// <summary>
    /// Gets the metadata name which indicates the <c>System.Type</c> of the validated object.
    /// </summary>
    /// <value>The type under validation.</value>
    public static string ValidatedType => validatedType;

    /// <summary>
    /// Gets the metadata name which indicates the <c>System.Type</c> of the validation rule.
    /// </summary>
    /// <value>The type of the validation rule.</value>
    public static string RuleType => ruleType;

    /// <summary>
    /// Gets the metadata name which indicates a <c>string</c> name for the given rule.
    /// This is an arbitrary name provided by the calling code.
    /// </summary>
    /// <value>The name of the rule.</value>
    public static string RuleName => ruleName;

    /// <summary>
    /// Gets the metadata name which indicates a <c>System.Reflection.MemberInfo</c> for the member which is undergoing
    /// validation.
    /// </summary>
    /// <value>The validated member.</value>
    public static string ValidatedMember => validatedMember;

    /// <summary>
    /// Gets the metadata name which indicates a <see cref="IManifestRule"/> representing the parent validation
    /// rule (if any).
    /// </summary>
    /// <value>The parent rule.</value>
    public static string ParentRule => parentRule;
  }
}
