//
// GenericFailureMessageFormatter.cs
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
  /// A message formatter which makes placeholder replacements of data into the message template.
  /// </summary>
  public class PlaceholderMessageFormatter<TValidated> : NoOpFailureMessageFormatter
  {
    readonly IDictionary<string,Func<TValidated,object>> placeholderFillers;

    /// <summary>
    /// Gets a collection of the registered placeholder fillers.  Each is a string key (which indicates the placeholder)
    /// and a function to get data with which to fill the placeholder.
    /// </summary>
    /// <value>The placeholder fillers.</value>
    protected virtual IDictionary<string,Func<TValidated,object>> PlaceholderFillers => placeholderFillers;

    /// <summary>
    /// Adds a placeholder function.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="filler">Filler.</param>
    public virtual void AddPlaceholder(string key, Func<TValidated,object> filler)
    {
      if(filler == null)
        throw new ArgumentNullException(nameof(filler));
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      PlaceholderFillers.Add(key, filler);
    }

    /// <summary>
    /// Adds a placeholder value.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="filler">Filler.</param>
    public virtual void AddPlaceholder(string key, object filler)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      AddPlaceholder(key, x => filler);
    }

    /// <summary>
    /// Formats the message template and returns the message.
    /// </summary>
    /// <returns>The message.</returns>
    /// <param name="messageTemplate">Message template.</param>
    /// <param name="result">Result.</param>
    public override string FormatMessage(string messageTemplate, IRunnableRuleResult result)
    {
      return FormatMessage(messageTemplate, (TValidated) result.Validated);
    }

    /// <summary>
    /// Formats the message template and returns the message.
    /// </summary>
    /// <returns>The message.</returns>
    /// <param name="template">Template.</param>
    /// <param name="validated">Validated.</param>
    protected virtual string FormatMessage(string template,
                                           TValidated validated)
    {
      string output = template;

      foreach(var placeholderAndFiller in placeholderFillers)
      {
        var filler = placeholderAndFiller.Value(validated)?.ToString() ?? String.Empty;
        output = output.Replace(placeholderAndFiller.Key, filler);
      }

      return output;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.Messages.PlaceholderMessageFormatter`1"/> class.
    /// </summary>
    public PlaceholderMessageFormatter()
    {
      this.placeholderFillers = new Dictionary<string,Func<TValidated,object>>();
    }
  }
}
