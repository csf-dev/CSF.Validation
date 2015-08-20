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

namespace CSF.Cli
{
  /// <summary>
  /// Represents the results of a <see cref="IParameterParser"/>'s parsing activity.  Contains the parameters that were
  /// parsed and their values.
  /// </summary>
  public class ParsedParameters : IParsedParameters
  {
    #region fields
    
    private IDictionary<object, IParameter> _parameters;
    private IList<string> _remainingArguments;
    
    #endregion
    
    #region properties
    
    
    /// <summary>
    /// Gets a collection of the remaining arguments; these are command-line arguments that have not been treated as
    /// parameters.
    /// </summary>
    /// <value>
    /// The remaining arguments.
    /// </value>
    public IEnumerable<string> RemainingArguments
    {
      get {
        string[] output = new string[_remainingArguments.Count];
        _remainingArguments.CopyTo(output, 0);
        return output;
      }
    }
    
    /// <summary>
    /// Gets a collection of the <see cref="IParameter"/>s that have been successfully parsed.
    /// </summary>
    /// <value>
    /// The parameters.
    /// </value>
    public IEnumerable<IParameter> Parameters
    {
      get {
        return this.BaseParameters.Values;
      }
    }
    
    /// <summary>
    /// Gets or sets the parameters that were successfully parsed.
    /// </summary>
    /// <value>
    /// The base parameters.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    protected IDictionary<object, IParameter> BaseParameters
    {
      get {
        return _parameters;
      }
      private set {
        if (value == null) {
          throw new ArgumentNullException ("value");
        }
        
        _parameters = value;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Gets a value that indicates whether the parameter (identified using the specified <paramref name="identifier"/>)
    /// is contained within this instance.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    public bool Contains(object identifier)
    {
      return this.BaseParameters.ContainsKey(identifier);
    }
    
    /// <summary>
    /// Gets a parameter using the specified identifier.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    public IParameter Get(object identifier)
    {
      return this.BaseParameters[identifier];
    }
    
    /// <summary>
    /// Gets a strongly-typed parameter using the specified identifier.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    /// <typeparam name='TParameterValue'>
    /// The value-type for the parameter.
    /// </typeparam>
    public IParameter<TParameterValue> Get<TParameterValue>(object identifier)
    {
      return this.Get(identifier) as IParameter<TParameterValue>;
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Cli.ParsedParameters"/> class.
    /// </summary>
    /// <param name='parameters'>
    /// The parameters that were successfuly parsed in the parsing operation.
    /// </param>
    /// <param name='remainingArguments'>
    /// The remaining arguments.
    /// </param>
    public ParsedParameters(IDictionary<object, IParameter> parameters, IList<string> remainingArguments)
    {
      this.BaseParameters = parameters;
      _remainingArguments = remainingArguments;
    }
    
    #endregion
  }
}

