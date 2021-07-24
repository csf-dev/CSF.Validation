//
// ByIdentityFormatterProviderExtensions.cs
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
using System.Linq.Expressions;
using System.Reflection;
using CSF.Reflection;
using CSF.Validation.Manifest;

namespace CSF.Validation.Messages
{
  /// <summary>
  /// Extension methods for the <see cref="ByIdentityFormatterProvider"/> type.
  /// </summary>
  public static class ByIdentityFormatterProviderExtensions
  {
    /// <summary>
    /// Registers a <see cref="T:PlaceholderMessageFormatter{TValidated}"/> with a
    /// <see cref="ByIdentityFormatterProvider"/>, using a rule's <see cref="DefaultManifestIdentity"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Please note, that if you have changed your default identity generation strategy then you will not be able to
    /// use this extension method.  It depends upon rules using the <see cref="DefaultManifestIdentity"/> type as
    /// their identities.
    /// </para>
    /// </remarks>
    /// <param name="provider">Provider.</param>
    /// <param name="ruleType">The rule type.</param>
    /// <param name="name">Name.</param>
    /// <param name="validatedMember">Validated member.</param>
    /// <param name="registration">Registration.</param>
    /// <typeparam name="TValidated">The type under validation.</typeparam>
    public static void RegisterPlaceholderFormatter<TValidated>(this ByIdentityFormatterProvider provider,
                                                                Type ruleType,
                                                                string name,
                                                                Expression<Func<TValidated,object>> validatedMember,
                                                                Action<PlaceholderRegistrationHelper<TValidated>> registration)
      where TValidated : class
    {
      if(ruleType == null)
        throw new ArgumentNullException(nameof(ruleType));
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));
      if(provider == null)
        throw new ArgumentNullException(nameof(provider));

      var helper = GetHelper<TValidated>();
      var identity = GetIdentity(typeof(TValidated),
                                 ruleType,
                                 name,
                                 (validatedMember != null)? Reflect.Member(validatedMember) : null);

      registration(helper);

      provider.RegisteredFormatters.Add(identity, helper.Formatter);
    }

    /// <summary>
    /// Registers a <see cref="T:PlaceholderMessageFormatter{TValidated}"/> with a
    /// <see cref="ByIdentityFormatterProvider"/>, using a rule's <see cref="DefaultManifestIdentity"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Please note, that if you have changed your default identity generation strategy then you will not be able to
    /// use this extension method.  It depends upon rules using the <see cref="DefaultManifestIdentity"/> type as
    /// their identities.
    /// </para>
    /// </remarks>
    /// <param name="provider">Provider.</param>
    /// <param name="ruleType">The rule type.</param>
    /// <param name="validatedMember">Validated member.</param>
    /// <param name="registration">Registration.</param>
    /// <typeparam name="TValidated">The type under validation.</typeparam>
    public static void RegisterPlaceholderFormatter<TValidated>(this ByIdentityFormatterProvider provider,
                                                                Type ruleType,
                                                                Expression<Func<TValidated,object>> validatedMember,
                                                                Action<PlaceholderRegistrationHelper<TValidated>> registration)
      where TValidated : class
    {
      RegisterPlaceholderFormatter(provider, ruleType, null, validatedMember, registration);
    }

    /// <summary>
    /// Registers a <see cref="T:PlaceholderMessageFormatter{TValidated}"/> with a
    /// <see cref="ByIdentityFormatterProvider"/>, using a rule's <see cref="DefaultManifestIdentity"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Please note, that if you have changed your default identity generation strategy then you will not be able to
    /// use this extension method.  It depends upon rules using the <see cref="DefaultManifestIdentity"/> type as
    /// their identities.
    /// </para>
    /// </remarks>
    /// <param name="provider">Provider.</param>
    /// <param name="ruleType">The rule type.</param>
    /// <param name="registration">Registration.</param>
    /// <typeparam name="TValidated">The type under validation.</typeparam>
    public static void RegisterPlaceholderFormatter<TValidated>(this ByIdentityFormatterProvider provider,
                                                                Type ruleType,
                                                                Action<PlaceholderRegistrationHelper<TValidated>> registration)
      where TValidated : class
    {
      RegisterPlaceholderFormatter(provider, ruleType, null, null, registration);
    }

    /// <summary>
    /// Registers a <see cref="T:PlaceholderMessageFormatter{TValidated}"/> with a
    /// <see cref="ByIdentityFormatterProvider"/>, using a rule's <see cref="DefaultManifestIdentity"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Please note, that if you have changed your default identity generation strategy then you will not be able to
    /// use this extension method.  It depends upon rules using the <see cref="DefaultManifestIdentity"/> type as
    /// their identities.
    /// </para>
    /// </remarks>
    /// <param name="provider">Provider.</param>
    /// <param name="ruleType">The rule type.</param>
    /// <param name="name">Name.</param>
    /// <param name="registration">Registration.</param>
    /// <typeparam name="TValidated">The type under validation.</typeparam>
    public static void RegisterPlaceholderFormatter<TValidated>(this ByIdentityFormatterProvider provider,
                                                                Type ruleType,
                                                                      string name,
                                                                      Action<PlaceholderRegistrationHelper<TValidated>> registration)
      where TValidated : class
    {
      RegisterPlaceholderFormatter(provider, ruleType, name, null, registration);
    }

    static PlaceholderRegistrationHelper<TValidated> GetHelper<TValidated>()
      where TValidated : class
    {
      var formatter = new PlaceholderMessageFormatter<TValidated>();
      return new PlaceholderRegistrationHelper<TValidated>(formatter);
    }

    static DefaultManifestIdentity GetIdentity(Type validatedType,
                                               Type ruleType,
                                               string name,
                                               MemberInfo validatedMember)
    {
      return new DefaultManifestIdentity(validatedType, ruleType, name, validatedMember);
    }
  }
}
