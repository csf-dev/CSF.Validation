//
// MessageProvider.cs
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
using CSF.Validation.ValidationRuns;

namespace CSF.Validation.Messages
{
  /// <summary>
  /// Implementation of <see cref="IMessageProvider"/> which makes use of a template provider and a formatter provider.
  /// </summary>
  public class MessageProvider : IMessageProvider
  {
    readonly ITemplateProvider templateProvider;
    readonly IFormatterProvider formatterProvider;

    /// <summary>
    /// Gets the failure message for a single failed rule.
    /// </summary>
    /// <returns>The failure message.</returns>
    /// <param name="result">Result.</param>
    public virtual string GetFailureMessage(IRunnableRuleResult result)
    {
      if(result == null)
        throw new ArgumentNullException(nameof(result));

      var template = templateProvider.GetTemplate(result);
      if(template == null)
        return null;

      var formatter = formatterProvider.GetFormatter(result);
      if(formatter == null)
        return template;

      return formatter.FormatMessage(template, result);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.Messages.MessageProvider"/> class.
    /// </summary>
    /// <param name="templateProvider">Template provider.</param>
    /// <param name="formatterProvider">Formatter provider.</param>
    public MessageProvider(ITemplateProvider templateProvider,
                           IFormatterProvider formatterProvider)
    {
      if(formatterProvider == null)
        throw new ArgumentNullException(nameof(formatterProvider));
      if(templateProvider == null)
        throw new ArgumentNullException(nameof(templateProvider));

      this.formatterProvider = formatterProvider;
      this.templateProvider = templateProvider;
    }
  }
}
