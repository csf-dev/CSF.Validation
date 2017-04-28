//
// PlaceholderRegistrationHelper.cs
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
namespace CSF.Validation.Messages
{
  /// <summary>
  /// Helper type for registration of placeholders &amp; fillers in a
  /// <see cref="T:PlaceholderMessageFormatter{TValidated}"/>.
  /// </summary>
  public class PlaceholderRegistrationHelper<TValidated> where TValidated : class
  {
    readonly PlaceholderMessageFormatter<TValidated> formatter;

    internal PlaceholderMessageFormatter<TValidated> Formatter => formatter;

    /// <summary>
    /// Registers a placeholder with the given filler.
    /// </summary>
    /// <param name="placeholder">Placeholder.</param>
    /// <param name="filler">Filler.</param>
    public void Placeholder(string placeholder, object filler)
    {
      Formatter.AddPlaceholder(placeholder, filler);
    }

    /// <summary>
    /// Registers a placeholder with the given filler function.
    /// </summary>
    /// <param name="placeholder">Placeholder.</param>
    /// <param name="filler">Filler.</param>
    public void Placeholder(string placeholder, Func<TValidated,object> filler)
    {
      Formatter.AddPlaceholder(placeholder, filler);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.Messages.PlaceholderRegistrationHelper`1"/> class.
    /// </summary>
    /// <param name="formatter">Formatter.</param>
    public PlaceholderRegistrationHelper(PlaceholderMessageFormatter<TValidated> formatter)
    {
      if(formatter == null)
        throw new ArgumentNullException(nameof(formatter));

      this.formatter = formatter;
    }
  }
}
