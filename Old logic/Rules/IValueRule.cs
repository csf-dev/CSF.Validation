//
// IValueRule.cs
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
namespace CSF.Validation.Rules
{
  /// <summary>
  /// Represents an <see cref="IRule"/> which validates a single value associated with the validated type.
  /// </summary>
  public interface IValueRule : IRule
  {
    /// <summary>
    /// Gets or sets the accessor function which is used to retrieve the value from the primary
    /// object under validation.
    /// </summary>
    /// <value>The accessor function.</value>
    Func<object,object> Accessor { get; set; }
  }

  /// <summary>
  /// Represents an <see cref="IRule"/> which validates a single value associated with the validated type.
  /// </summary>
  public interface IValueRule<TValue> : IRule, IValueRule
  {
    /// <summary>
    /// Gets or sets the accessor function which is used to retrieve the value from the primary
    /// object under validation.
    /// </summary>
    /// <value>The accessor function.</value>
    new Func<object,TValue> Accessor { get; set; }
  }

  /// <summary>
  /// Represents an <see cref="IRule"/> which validates a single value associated with the validated type.
  /// </summary>
  public interface IValueRule<TValidated,TValue> : IRule, IValueRule
  {
    /// <summary>
    /// Gets or sets the accessor function which is used to retrieve the value from the primary
    /// object under validation.
    /// </summary>
    /// <value>The accessor function.</value>
    new Func<TValidated,TValue> Accessor { get; set; }
  }
}
