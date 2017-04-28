//
// RegisteredFormatterProvider.cs
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
using CSF.Validation.ValidationRuns;

namespace CSF.Validation.Messages
{
  /// <summary>
  /// Service which selects and returns an <see cref="IMessageFormatter"/> based on the manifest identity
  /// of the <see cref="IRunnableRuleResult"/>.
  /// </summary>
  public class ByIdentityFormatterProvider : IFormatterProvider
  {
    static readonly IMessageFormatter defaultFormatter;
    readonly IDictionary<object,IMessageFormatter> registeredFormatters;

    /// <summary>
    /// Gets a collection of the registered formatters.
    /// </summary>
    /// <value>The registered formatters.</value>
    public virtual IDictionary<object,IMessageFormatter> RegisteredFormatters => registeredFormatters;

    /// <summary>
    /// Gets the message formatter.
    /// </summary>
    /// <returns>The formatter.</returns>
    /// <param name="result">Result.</param>
    public IMessageFormatter GetFormatter(IRunnableRuleResult result)
    {
      if(result == null)
        throw new ArgumentNullException(nameof(result));

      return GetRegisteredFormatter(result.ManifestIdentity)?? GetDefaultFormatter();
    }

    /// <summary>
    /// Gets a formatter which has been registered with the current instance.
    /// </summary>
    /// <returns>The formatter.</returns>
    /// <param name="identity">The identity of the validation rule.</param>
    protected virtual IMessageFormatter GetRegisteredFormatter(object identity)
    {
      if(!RegisteredFormatters.ContainsKey(identity))
      {
        return null;
      }

      return RegisteredFormatters[identity];
    }

    /// <summary>
    /// Gets a fallback/default formatter.
    /// </summary>
    /// <returns>The default formatter.</returns>
    protected virtual IMessageFormatter GetDefaultFormatter()
    {
      return defaultFormatter;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.Messages.ByIdentityFormatterProvider"/> class.
    /// </summary>
    public ByIdentityFormatterProvider()
    {
      registeredFormatters = new Dictionary<object,IMessageFormatter>();
    }

    /// <summary>
    /// Initializes the <see cref="T:CSF.Validation.Messages.ByIdentityFormatterProvider"/> class.
    /// </summary>
    static ByIdentityFormatterProvider()
    {
      defaultFormatter = new NoOpFailureMessageFormatter();
    }
  }
}
