//
// IParameterParser.cs
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
  /// Interface for a commandline parameter parser.
  /// </summary>
  public interface IParameterParser
  {
    #region properties

    /// <summary>
    /// Gets or sets a collection of the parameters registered with this instance.
    /// </summary>
    /// <value>
    /// The registered parameters.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    IList<IParameter> RegisteredParameters { get; set; }

    #endregion

    #region adding definitions
    
    /// <summary>
    /// Adds/registers a single parameter with the parser.  The parameter implicitly has the
    /// <see cref="ParameterBehaviour.Switch"/> behaviour.
    /// </summary>
    /// <returns>
    /// The current <see cref="IParameterParser"/> instance, permitting method-chaining.
    /// </returns>
    /// <param name='shortName'>
    /// The short name of the parameter (typically one character long).
    /// </param>
    /// <param name='longName'>
    /// The long name of the parameter (multiple characters, with no spaces)
    /// </param>
    /// <param name='identifier'>
    /// A unique identifier for the parameter.
    /// </param>
    IParameterParser AddParameter(string shortName,
                                  string longName,
                                  object identifier);
    
    /// <summary>
    /// Adds/registers a single parameter with the parser.  The parameter implicitly has the
    /// <see cref="ParameterBehaviour.Switch"/> behaviour.
    /// </summary>
    /// <returns>
    /// The current <see cref="IParameterParser"/> instance, permitting method-chaining.
    /// </returns>
    /// <param name='shortNames'>
    /// A collection of the short names for the parameter (typically each is one character long).
    /// </param>
    /// <param name='longNames'>
    /// A collection of the long names for the parameter (each may be multiple characters, with no spaces)
    /// </param>
    /// <param name='identifier'>
    /// A unique identifier for the parameter.
    /// </param>
    IParameterParser AddParameter(IList<string> shortNames,
                                  IList<string> longNames,
                                  object identifier);
    
    /// <summary>
    /// Adds/registers a single parameter with the parser.
    /// </summary>
    /// <returns>
    /// The current <see cref="IParameterParser"/> instance, permitting method-chaining.
    /// </returns>
    /// <param name='shortName'>
    /// The short name of the parameter (typically one character long).
    /// </param>
    /// <param name='longName'>
    /// The long name of the parameter (multiple characters, with no spaces)
    /// </param>
    /// <param name='behaviour'>
    /// The behaviour of the parameter.
    /// </param>
    /// <param name='identifier'>
    /// A unique identifier for the parameter.
    /// </param>
    /// <typeparam name='TParameterValue'>
    /// The expected return value of the parameter.
    /// </typeparam>
    IParameterParser AddParameter<TParameterValue>(string shortName,
                                                   string longName,
                                                   ParameterBehaviour behaviour,
                                                   object identifier);
    
    /// <summary>
    /// Adds/registers a single parameter with the parser.
    /// </summary>
    /// <returns>
    /// The current <see cref="IParameterParser"/> instance, permitting method-chaining.
    /// </returns>
    /// <param name='shortNames'>
    /// A collection of the short names for the parameter (typically each is one character long).
    /// </param>
    /// <param name='longNames'>
    /// A collection of the long names for the parameter (each may be multiple characters, with no spaces)
    /// </param>
    /// <param name='behaviour'>
    /// The behaviour of the parameter.
    /// </param>
    /// <param name='identifier'>
    /// A unique identifier for the parameter.
    /// </param>
    /// <typeparam name='TParameterValue'>
    /// The expected return value of the parameter.
    /// </typeparam>
    IParameterParser AddParameter<TParameterValue>(IList<string> shortNames,
                                                   IList<string> longNames,
                                                   ParameterBehaviour behaviour,
                                                   object identifier);
    
    /// <summary>
    /// Registers multiple parameters with the parser, using the properties and fields from the type
    /// <typeparamref name="TParameterType"/> to detect the properties of each parameter.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method inspects the members of the <typeparamref name="TParameterType"/> for those decorated with 
    /// <see cref="ParameterAttribute"/> in order to discover parameter specifications.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The current <see cref="IParameterParser"/> instance, permitting method-chaining.
    /// </returns>
    /// <typeparam name='TParameterType'>
    /// A type that implements <see cref="IParsedParameters"/> from which to read the parameter specifications.
    /// </typeparam>
    IParameterParser AddParameters<TParameterType>() where TParameterType : IParsedParameters;
    
    #endregion
    
    #region parsing
    
    /// <summary>
    /// Parse the specified command line arguments, returning an object that contains the results of the parsing.
    /// </summary>
    /// <param name='commandLineArguments'>
    /// The input command line arguments from which to parse parameters.
    /// </param>
    IParsedParameters Parse(IList<string> commandLineArguments);
    
    /// <summary>
    /// Parse the specified command line arguments, returning an object that contains the results of the parsing.
    /// </summary>
    /// <param name='commandLineArguments'>
    /// The input command line arguments from which to parse parameters.
    /// </param>
    /// <typeparam name='TOutput'>
    /// A type that implements <see cref="IParsedParameters"/>, providing a strongly-typed container for the parsing
    /// results.
    /// </typeparam>
    TOutput Parse<TOutput>(IList<string> commandLineArguments) where TOutput : IParsedParameters;
    
    #endregion
  }
}

