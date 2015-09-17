//
// PosixParameterParser1.cs
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
using System.Linq;

namespace CSF.Cli
{
  /// <summary>
  /// Generic implementation of <see cref="PosixParameterParser"/>, which parses parameters into object instances.
  /// </summary>
  public class PosixParameterParser<TParsed> : PosixParameterParser, IParameterParser<TParsed>
    where TParsed : class,new()
  {
    #region fields

    private Dictionary<PropertyInfo,ParameterMapping> _mappings;
    private PropertyInfo _remainingArguments;

    #endregion

    #region methods

    /// <summary>
    /// Gets a read-only collection of the parameters registered for the current instance.
    /// </summary>
    /// <returns>A read-only collection of the registered parameters.</returns>
    public override ParameterMapping[] GetRegisteredParameters()
    {
      return _mappings.Values.ToArray();
    }

    /// <summary>
    /// Parses the given command line arguments into an instance of <typeparamref name="TParsed" />.
    /// </summary>
    /// <param name="commandlineArguments">The command line arguments.</param>
    public new TParsed Parse(IList<string> commandlineArguments)
    {
      var parsedParams = base.Parse(commandlineArguments);
      var allMappings = GetRegisteredParameters();
      TParsed output = new TParsed();

      foreach(var mapping in allMappings)
      {
        object identifier = mapping.Identifier;

        if(parsedParams.HasParameter(identifier))
        {
          PropertyInfo property = (PropertyInfo) identifier;

          if(mapping.Behaviour == ParameterBehaviour.Switch)
          {
            property.SetValue(output, true);
          }
          else
          {
            property.SetValue(output, parsedParams.GetParameterValue(identifier));
          }
        }
      }

      if(_remainingArguments != null)
      {
        _remainingArguments.SetValue(output, (IList<string>) parsedParams.GetRemainingArguments());
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Cli.PosixParameterParser{TParsed}"/> class.
    /// </summary>
    /// <param name="mappings">The parameter mappings.</param>
    /// <param name="remainingArguments">An optional <c>System.Reflection.PropertyInfo</c> indicating where to store the remaining arguments.</param>
    public PosixParameterParser(Dictionary<PropertyInfo,ParameterMapping> mappings,
                                PropertyInfo remainingArguments) : base()
    {
      if(mappings == null)
      {
        throw new ArgumentNullException("mappings");
      }

      _mappings = mappings;
      _remainingArguments = remainingArguments;
    }

    #endregion
  }
}

