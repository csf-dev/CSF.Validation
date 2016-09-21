//
// PosixParameterParser.cs
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
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;

namespace CSF.Cli.Parameters
{
  /// <summary>
  /// Implementation of an <see cref="IParameterParser"/> that deals with POSIX-style parameters.
  /// </summary>
  public class PosixParameterParser : IParameterParser
  {
    #region constants

    private const string
      LONG_PARAMETER_PATTERN            = @"^--([A-Za-z0-9_-]{2,})$",
      SHORT_PARAMETER_PATTERN           = @"^-([A-Za-z0-9])$";
    
    private static readonly Regex
      LongParameter                     = new Regex(LONG_PARAMETER_PATTERN, RegexOptions.Compiled),
      ShortParameter                    = new Regex(SHORT_PARAMETER_PATTERN, RegexOptions.Compiled);
    
    #endregion
    
    #region fields
    
    private IDictionary<object,ParameterMapping> _mappings;

    #endregion

    #region methods

    /// <summary>
    /// Gets a read-only collection of the parameters registered for the current instance.
    /// </summary>
    /// <returns>A read-only collection of the registered parameters.</returns>
    public virtual ParameterMapping[] GetRegisteredParameters()
    {
      return _mappings.Values.ToArray();
    }

    /// <summary>
    /// Parses the given command line arguments into a <see cref="ParsedParameters"/> instance.
    /// </summary>
    /// <param name="commandlineArguments">The command line arguments.</param>
    public ParsedParameters Parse(IList<string> commandlineArguments)
    {
      if(commandlineArguments == null)
      {
        throw new ArgumentNullException("commandlineArguments");
      }

      IList<string> remainingArguments = new List<string>();
      ICollection<object> flagParameters = new List<object>();
      IDictionary<object,string> valueParameters = new Dictionary<object,string>();

      IDictionary<string,ParameterMapping> shortNames, longNames;
      var allParameters = this.GetRegisteredParameters();
      this.NormaliseNames(allParameters, out shortNames, out longNames);

      ParameterMapping currentValueParameter = null;

      foreach(string arg in commandlineArguments)
      {
        // If there is a current parameter expecting a value then we can just store this argument as that value.
        if(currentValueParameter != null
           && currentValueParameter.Behaviour == ParameterBehaviour.ValueRequired)
        {
          valueParameters.Add(currentValueParameter.Identifier, arg);
          currentValueParameter = null;
          continue;
        }

        ParameterMapping mapping = this.GetMapping(arg, shortNames, longNames);
        if(mapping == null
           && currentValueParameter != null)
        {
          valueParameters.Add(currentValueParameter.Identifier, arg);
          currentValueParameter = null;
          continue;
        }
        else if(mapping == null)
        {
          remainingArguments.Add(arg);
          continue;
        }

        if(currentValueParameter != null
           && currentValueParameter.Behaviour == ParameterBehaviour.ValueOptional)
        {
          valueParameters.Add(currentValueParameter.Identifier, String.Empty);
          currentValueParameter = null;
        }

        if(mapping.Behaviour == ParameterBehaviour.Switch)
        {
          flagParameters.Add(mapping.Identifier);
        }
        else
        {
          currentValueParameter = mapping;
        }
      }

      if(currentValueParameter != null
           && currentValueParameter.Behaviour == ParameterBehaviour.ValueOptional)
      {
        valueParameters.Add(currentValueParameter.Identifier, String.Empty);
        currentValueParameter = null;
      }

      return new ParsedParameters(flagParameters, valueParameters, remainingArguments);
    }

    /// <summary>
    /// Normalises the parameter names and returns two <c>IDictionary&lt;string,ParameterMapping&gt;</c> instances,
    /// indicating all of the parameter mappings by their long names and short names.
    /// </summary>
    /// <param name="registeredParameters">The registered parameters.</param>
    /// <param name="shortNames">The parameters by their short names.</param>
    /// <param name="longNames">The parameters by their long names.</param>
    protected void NormaliseNames(IEnumerable<ParameterMapping> registeredParameters,
                                  out IDictionary<string,ParameterMapping> shortNames,
                                  out IDictionary<string,ParameterMapping> longNames)
    {
      if(registeredParameters == null)
      {
        throw new ArgumentNullException("registeredParameters");
      }

      shortNames = (from param in registeredParameters
                    from name in param.ShortNames
                    select new { Parameter = param, Name = name })
        .ToDictionary(k => k.Name, v => v.Parameter);

      longNames = (from param in registeredParameters
                   from name in param.LongNames
                   select new { Parameter = param, Name = name })
        .ToDictionary(k => k.Name, v => v.Parameter);
    }

    /// <summary>
    /// Gets the appropriate parameter mapping.
    /// </summary>
    /// <returns>The mapping, or a <c>null</c> reference if no mapping matches the argument.</returns>
    /// <param name="argument">The command line argument.</param>
    /// <param name="shortNames">The mappings, indexed by their short names.</param>
    /// <param name="longNames">The mappings, indexed by their long names.</param>
    protected ParameterMapping GetMapping(string argument,
                                          IDictionary<string,ParameterMapping> shortNames,
                                          IDictionary<string,ParameterMapping> longNames)
    {
      ParameterMapping output = null;
      Match
        longParameter = LongParameter.Match(argument),
        shortParameter = ShortParameter.Match(argument);

      if(longParameter.Success && longNames.ContainsKey(longParameter.Groups[1].Value))
      {
        output = longNames[longParameter.Groups[1].Value];
      }
      else if(shortParameter.Success && shortNames.ContainsKey(shortParameter.Groups[1].Value))
      {
        output = shortNames[shortParameter.Groups[1].Value];
      }

      return output;
    }

    #endregion
    
    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Cli.PosixParameterParser"/> class.
    /// </summary>
    protected PosixParameterParser()
    {
      _mappings = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Cli.PosixParameterParser"/> class.
    /// </summary>
    /// <param name="mappings">The parameter mappings.</param>
    public PosixParameterParser(IDictionary<object,ParameterMapping> mappings)
    {
      if(mappings == null)
      {
        throw new ArgumentNullException("mappings");
      }

      _mappings = mappings;
    }
    
    #endregion
  }
}

