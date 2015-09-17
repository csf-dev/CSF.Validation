//
// ParsedParameters.cs
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
using System.Linq.Expressions;
using CSF.Reflection;
using System.Linq;

namespace CSF.Cli
{
  /// <summary>
  /// Represents the results of a <see cref="IParameterParser"/>'s parsing activity.  Contains the parameters that were
  /// parsed and their values.
  /// </summary>
  public class ParsedParameters
  {
    #region fields

    private HashSet<object> _flagParameters;
    private IDictionary<object,string> _valueParameters;
    private IList<string> _remainingArguments;

    #endregion

    #region methods

    /// <summary>
    /// Determines whether this instance has a parameter with the specified identifier.
    /// </summary>
    /// <returns><c>true</c> if this instance has a parameter with the specified identifier; otherwise, <c>false</c>.</returns>
    /// <param name="identifier">Identifier.</param>
    public bool HasParameter(object identifier)
    {
      return (_flagParameters.Contains(identifier)
              || _valueParameters.ContainsKey(identifier));
    }

    /// <summary>
    /// Gets the value for a 'value type' parameter.
    /// </summary>
    /// <returns>The parameter value.</returns>
    /// <param name="identifier">Identifier.</param>
    public string GetParameterValue(object identifier)
    {
      if(!_valueParameters.ContainsKey(identifier))
      {
        throw new ArgumentException("The parsed parameters must contain a parameter of the given identifier",
                                    "identifier");
      }

      return _valueParameters[identifier];
    }

    /// <summary>
    /// Gets a collection of the remaining <c>System.String</c> positional arguments, which are not parameters.
    /// </summary>
    /// <returns>The remaining arguments.</returns>
    public string[] GetRemainingArguments()
    {
      return _remainingArguments.ToArray();
    }

    #endregion
    
    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Cli.ParsedParameters"/> class.
    /// </summary>
    /// <param name="flagParameters">Flag parameters.</param>
    /// <param name="valueParameters">Value parameters.</param>
    /// <param name="remainingArguments">Remaining arguments.</param>
    public ParsedParameters(IEnumerable<object> flagParameters,
                            IDictionary<object,string> valueParameters,
                            IList<string> remainingArguments)
    {
      if(flagParameters == null)
      {
        throw new ArgumentNullException("flagParameters");
      }
      if(valueParameters == null)
      {
        throw new ArgumentNullException("valueParameters");
      }
      if(remainingArguments == null)
      {
        throw new ArgumentNullException("remainingArguments");
      }

      _flagParameters = new HashSet<object>(flagParameters);
      _valueParameters = valueParameters;
      _remainingArguments = remainingArguments;
    }

    #endregion
  }
}

