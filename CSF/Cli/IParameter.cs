//
// IParameter.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2015 CSF Software Limited
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

namespace CSF.Cli
{
  /// <summary>
  /// Interface for a parameter specification (and later a parsed parameter).
  /// </summary>
  public interface IParameter
  {
    #region properties
    
    /// <summary>
    /// Gets or sets a unique identifier by which this parameter may be identified.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    object Identifier { get; set; }
    
    /// <summary>
    /// Gets or sets a collection of 'short' names (typically only 1 character long each) for this parameter.
    /// </summary>
    /// <value>
    /// The short names.
    /// </value>
    IList<string> ShortNames { get; set; }
    
    /// <summary>
    /// Gets or sets a collection of 'long' names (may be over 1 character each, but with no spaces) for this parameter.
    /// </summary>
    /// <value>
    /// The long names.
    /// </value>
    IList<string> LongNames { get; set; }
    
    /// <summary>
    /// Gets or sets the behaviour of this parameter.
    /// </summary>
    /// <value>
    /// The behaviour.
    /// </value>
    ParameterBehaviour Behaviour { get; set; }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Reads the value associated with this parameter.
    /// </summary>
    /// <returns>
    /// The value.
    /// </returns>
    object GetValue();
    
    /// <summary>
    /// Sets a value into the current instance.
    /// </summary>
    /// <param name='value'>
    /// The value to store.
    /// </param>
    void SetValue(string value);
    
    #endregion
  }
}

