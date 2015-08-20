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

namespace CSF.Cli
{
  /// <summary>
  /// Implementation of an <see cref="IParameterParser"/> that deals with POSIX-style parameters.
  /// </summary>
  public class PosixParameterParser : IParameterParser
  {
    #region constants
    
    /// <summary>
    /// The parameters that make up the method signature for the constructor that is required of any type that
    /// implements <see cref="IParsedParameters"/>.
    /// </summary>
    private static readonly Type[]
      IParsedParametersCtorSignature = new Type[] { typeof(IDictionary<object, IParameter>), typeof(IList<string>) };
    
    private const string
      LONG_PARAMETER_PATTERN            = @"^--([A-Za-z0-9_-]{2,})$",
      SHORT_PARAMETER_PATTERN           = @"^-([A-Za-z0-9])$";
    
    private static readonly Regex
      LongParameter                     = new Regex(LONG_PARAMETER_PATTERN, RegexOptions.Compiled),
      ShortParameter                    = new Regex(SHORT_PARAMETER_PATTERN, RegexOptions.Compiled);
    
    #endregion
    
    #region fields
    
    private IList<IParameter> _registeredParameters;
    
    #endregion
    
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
    public IList<IParameter> RegisteredParameters
    {
      get {
        return _registeredParameters;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _registeredParameters = value;
      }
    }
    
    #endregion
    
    #region registering parameters
    
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
    public IParameterParser AddParameter(string shortName,
                                         string longName,
                                         object identifier)
    {
      return this.AddParameter<bool>(shortName, longName, ParameterBehaviour.Switch, identifier);
    }

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
    public IParameterParser AddParameter(IList<string> shortNames,
                                         IList<string> longNames,
                                         object identifier)
    {
      return this.AddParameter<bool>(shortNames, longNames, ParameterBehaviour.Switch, identifier);
    }

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
    public IParameterParser AddParameter<TParameterValue>(string shortName,
                                                          string longName,
                                                          ParameterBehaviour behaviour,
                                                          object identifier)
    {
      IList<string> shortNames, longNames;
      
      shortNames = (shortName != null)? new string[] { shortName } : new string[0];
      longNames = (longName != null)? new string[] { longName } : new string[0];
      
      return this.AddParameter<TParameterValue>(shortNames, longNames, behaviour, identifier);
    }

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
    public IParameterParser AddParameter<TParameterValue>(IList<string> shortNames,
                                                          IList<string> longNames,
                                                          ParameterBehaviour behaviour,
                                                          object identifier)
    {
      this.RegisteredParameters.Add(new Parameter<TParameterValue>() {
        ShortNames = shortNames,
        LongNames = longNames,
        Behaviour = behaviour,
        Identifier = identifier
      });
      return this;
    }

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
    public IParameterParser AddParameters<TParameterType>() where TParameterType : IParsedParameters
    {
      Type parameterType = typeof(TParameterType);
      ConstructorInfo constructor = parameterType.GetConstructor(IParsedParametersCtorSignature);
      
      if(constructor == null)
      {
        string message = String.Format("The parameter result/output type `{0}' does not contain a constructor with" +
                                       "the required signature: (IDictionary<object, IParameter>, IList<string>)",
                                       parameterType.FullName);
        throw new InvalidOperationException(message);
      }
      
      foreach(PropertyInfo property in parameterType.GetProperties())
      {
        object[] allAttributes = property.GetCustomAttributes(typeof(ParameterAttribute), true);
        if(allAttributes.Length == 1)
        {
          this.AddParameter(property, (ParameterAttribute) allAttributes[0]);
        }
      }
      
      return this;
    }
    
    /// <summary>
    /// Adds a single parameter to the current instance.
    /// </summary>
    /// <param name='property'>
    /// The property for which to add the parameter.
    /// </param>
    /// <param name='attribute'>
    /// The <see cref="ParameterAttribute"/> found on that property.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    private void AddParameter(PropertyInfo property, ParameterAttribute attribute)
    {
      if(property == null)
      {
        throw new ArgumentNullException ("property");
      }
      if(attribute == null)
      {
        throw new ArgumentNullException ("attribute");
      }
          
      Type parameterGenericType = typeof(Parameter<>).MakeGenericType(new Type[] { property.PropertyType });
      IParameter parameter = (IParameter) parameterGenericType.GetConstructor(Type.EmptyTypes).Invoke(null);
      
      parameter.Behaviour = attribute.Behaviour;
      parameter.Identifier = property;
      
      if(attribute.ShortName != null)
      {
        parameter.ShortNames.Add(attribute.ShortName);
      }
      if(attribute.LongName != null)
      {
        parameter.LongNames.Add(attribute.LongName);
      }
      
      foreach(ParameterNameAttribute name in property.GetCustomAttributes(typeof(ParameterNameAttribute), true))
      {
        var paramList = name.IsLongName? parameter.LongNames : parameter.ShortNames;
        paramList.Add(name.Name);
      }
      
      this.RegisteredParameters.Add(parameter);
    }
    
    #endregion
    
    #region parsing
  
    /// <summary>
    /// Parses the specified command line arguments and returns a default object instance that implements
    /// <see cref="IParsedParameters"/>.
    /// </summary>
    /// <param name='commandLineArguments'>
    /// The raw command line arguments to parse.
    /// </param>
    public IParsedParameters Parse(IList<string> commandLineArguments)
    {
      return this.Parse<ParsedParameters>(commandLineArguments);
    }
  
    /// <summary>
    /// Parses the specified command line arguments and returns an object instance that implements
    /// <see cref="IParsedParameters"/>, based upon the generic type parameter passed to this method.
    /// </summary>
    /// <param name='commandLineArguments'>
    /// The raw command line arguments to parse.
    /// </param>
    /// <typeparam name='TOutput'>
    /// The output (parsed parameters container) type to return.
    /// </typeparam>
    public TOutput Parse<TOutput>(IList<string> commandLineArguments) where TOutput : IParsedParameters
    {
      IDictionary<string, IParameter> parametersByShortName, parametersByLongName;
      IDictionary<object, IParameter> parametersFound = new Dictionary<object, IParameter>();
      IList<string> remainingArguments = new List<string>();
      IParameter currentParameter = null;
      
      if(commandLineArguments == null)
      {
        throw new ArgumentNullException ("commandLineArguments");
      }
      
      this.NormalisesParameterAliases(this.RegisteredParameters, out parametersByLongName, out parametersByShortName);
      
      foreach(string argument in commandLineArguments)
      {
        IParameter parameter;
        
        // If there is a current parameter expecting a value then we can just store this argument as that value.
        if(currentParameter != null && currentParameter.Behaviour == ParameterBehaviour.ValueRequired)
        {
          currentParameter.SetValue(argument);
          currentParameter = null;
          continue;
        }
        
        parameter = this.GetParameter(argument, parametersByShortName, parametersByLongName);
        
        if(parameter == null && currentParameter != null)
        {
          // The current parameter is expecting a value and the current argument doesn't match a parameter definition
          currentParameter.SetValue(argument);
          currentParameter = null;
        }
        else if(parameter == null)
        {
          // The current argument doesn't match a parameter definition but we were not expecting a value
          remainingArguments.Add(argument);
        }
        else
        {
          // The current argument is a parameter
          parametersFound.Add(parameter.Identifier, parameter);
          currentParameter = (parameter.Behaviour == ParameterBehaviour.ValueOptional
                              || parameter.Behaviour == ParameterBehaviour.ValueRequired)? parameter : null;
        }
      }
      
      if(currentParameter != null && currentParameter.Behaviour == ParameterBehaviour.ValueRequired)
      {
        throw new InvalidOperationException("A value-mandatory parameter is missing a value.");
      }
      
      return CreateParameterContainer<TOutput>(parametersFound, remainingArguments);
    }
    
    /// <summary>
    /// Normaliseses the parameter aliases into two dictionaries for quick searching.
    /// </summary>
    /// <param name='parameters'>
    /// The registered parameters.
    /// </param>
    /// <param name='longAliases'>
    /// A dictionary of the registered parameters, indexed by their long names.
    /// </param>
    /// <param name='shortAliases'>
    /// A dictionary of the registered parameters, indexed by their short names.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='InvalidOperationException'>
    /// Is thrown when an operation cannot be performed.
    /// </exception>
    private void NormalisesParameterAliases(IEnumerable<IParameter> parameters,
                                            out IDictionary<string, IParameter> longAliases,
                                            out IDictionary<string, IParameter> shortAliases)
    {
      longAliases = new Dictionary<string, IParameter>();
      shortAliases = new Dictionary<string, IParameter>();
      
      if(parameters == null)
      {
        throw new ArgumentNullException("parameters");
      }
      
      foreach(IParameter param in parameters)
      {
        foreach(string name in param.LongNames)
        {
          if(!longAliases.ContainsKey(name))
          {
            longAliases.Add(name, param);
          }
          else
          {
            string message = String.Format("Duplicate parameter long name: '{0}'.  Multiple registered parameters " +
                                           "with the same names are not permitted.",
                                           name);
            throw new InvalidOperationException(message);
          }
        }
        
        foreach(string name in param.ShortNames)
        {
          if(!shortAliases.ContainsKey(name))
          {
            shortAliases.Add(name, param);
          }
          else
          {
            string message = String.Format("Duplicate parameter short name: '{0}'.  Multiple registered parameters " +
                                           "with the same names are not permitted.",
                                           name);
            throw new InvalidOperationException(message);
          }
        }
      }
    }
    
    /// <summary>
    /// Gets the parameter that matches the <paramref name="argument"/> if one is found.
    /// </summary>
    /// <returns>
    /// The parameter, or a null reference if no matching parameter was found.
    /// </returns>
    /// <param name='argument'>
    /// The argument to match.
    /// </param>
    /// <param name='parametersByShortName'>
    /// Parameters by short name.
    /// </param>
    /// <param name='parametersByLongName'>
    /// Parameters by long name.
    /// </param>
    private IParameter GetParameter(string argument,
                                      IDictionary<string, IParameter> parametersByShortName,
                                      IDictionary<string, IParameter> parametersByLongName)
    {
      IParameter output = null;
      Match
        longParameter = LongParameter.Match(argument),
        shortParameter = ShortParameter.Match(argument);
      
      if(longParameter.Success && parametersByLongName.ContainsKey(longParameter.Groups[1].Value))
      {
        output = parametersByLongName[longParameter.Groups[1].Value];
      }
      else if(shortParameter.Success && parametersByShortName.ContainsKey(shortParameter.Groups[1].Value))
      {
        output = parametersByShortName[shortParameter.Groups[1].Value];
      }
      
      return output;
    }
    
    /// <summary>
    /// Creates an instance of a parameter container type.
    /// </summary>
    /// <returns>
    /// The parameter container.
    /// </returns>
    /// <param name='parameters'>
    /// The parameters found by the parsing process.
    /// </param>
    /// <param name='remainingArguments'>
    /// The remaining (non-parameter) command line arguments.
    /// </param>
    /// <typeparam name='TOutput'>
    /// The output (parsed parameters container) type to return.
    /// </typeparam>
    private TOutput CreateParameterContainer<TOutput>(IDictionary<object, IParameter> parameters,
                                                      IList<string> remainingArguments) where TOutput : IParsedParameters
    {
      ConstructorInfo constructor = typeof(TOutput).GetConstructor(IParsedParametersCtorSignature);
      
      if(constructor == null)
      {
        throw new InvalidOperationException(String.Format("`{0}' does not contain a constructor of the required " +
                                                          "signature: (IDictionary<object, IParameter>, IList<string>)",
                                                          typeof(TOutput).FullName));
      }
      
      return (TOutput) constructor.Invoke(new object[] { parameters, remainingArguments });
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Cli.PosixParameterParser"/> class.
    /// </summary>
    public PosixParameterParser ()
    {
      this.RegisteredParameters = new List<IParameter>();
    }
    
    #endregion
  }
}

