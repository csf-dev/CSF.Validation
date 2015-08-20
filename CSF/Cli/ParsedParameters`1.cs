//
// ParsedParameters`1.cs
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
using System.Linq.Expressions;
using System.Collections.Generic;
using CSF.Reflection;

namespace CSF.Cli
{
  /// <summary>
  /// Generic implementation of <see cref="ParsedParameters"/> that is designed to be subclassed.  It adds some
  /// protected helper methods that derived classes will find useful for handling parameters.
  /// </summary>
  public abstract class ParsedParameters<TParameterContainer> : ParsedParameters, IParsedParameters
  {
    #region methods
    
    /// <summary>
    /// Gets a value that indicates whether the parameter (identified using the specified <paramref name="identifier"/>)
    /// is contained within this instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a convenience method intended for use by subclassing types that are created in conjunction with
    /// <see cref="ParameterAttribute"/>.  They provide a simple way of getting parameter values based on the member
    /// that the attribute was defined upon.
    /// </para>
    /// </remarks>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    protected bool Contains(Expression<Func<TParameterContainer, object>> identifier)
    {
      return this.Contains(Reflect.Member<TParameterContainer>(identifier));
    }
    
    /// <summary>
    /// Gets a parameter using the specified identifier.
    /// </summary>
    /// <param name='identifier'>
    /// The identifier for this parameter.
    /// </param>
    protected IParameter Get(Expression<Func<TParameterContainer, object>> identifier)
    {
      return this.Get(Reflect.Member<TParameterContainer>(identifier));
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
    protected IParameter<TParameterValue> Get<TParameterValue>(Expression<Func<TParameterContainer, object>> identifier)
    {
      return this.Get<TParameterValue>(Reflect.Member<TParameterContainer>(identifier));
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
    public ParsedParameters(IDictionary<object, IParameter> parameters,
                            IList<string> remainingArguments) : base(parameters, remainingArguments) {}
    
    #endregion
  }
}

