//
// ParameterMapping.cs
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
using System.Linq;

namespace CSF.Cli
{
  /// <summary>
  /// Represents the mapping of a single parameter to be used in an <see cref="T:IParameterParser{T}"/>
  /// </summary>
  public class ParameterMapping
  {
    #region fields

    private object _identifier;
    private ISet<string> _shortNames, _longNames;
    private ParameterBehaviour _behaviour;

    #endregion

    #region properties

    /// <summary>
    /// Gets an object by which the parameter is identified.
    /// </summary>
    /// <value>The identifier.</value>
    public object Identifier
    {
      get {
        return _identifier;
      }
    }

    /// <summary>
    /// Gets a collection of the short parameter names.
    /// </summary>
    /// <value>The short names.</value>
    public IEnumerable<string> ShortNames
    {
      get {
        return _shortNames;
      }
    }

    /// <summary>
    /// Gets a collection of the long parameter names.
    /// </summary>
    /// <value>The long names.</value>
    public IEnumerable<string> LongNames
    {
      get {
        return _longNames;
      }
    }

    /// <summary>
    /// Gets the parameter behaviour.
    /// </summary>
    /// <value>The behaviour.</value>
    public ParameterBehaviour Behaviour
    {
      get {
        return _behaviour;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Cli.ParameterMapping"/> class.
    /// </summary>
    /// <param name="identifier">An object by which the parameter is identified.</param>
    /// <param name="behaviour">The parameter behaviour.</param>
    /// <param name="shortNames">The paramater short names.</param>
    /// <param name="longNames">The parameter long names.</param>
    public ParameterMapping(object identifier,
                            ParameterBehaviour behaviour,
                            IEnumerable<string> shortNames = null,
                            IEnumerable<string> longNames = null)
    {
      if(identifier == null)
      {
        throw new ArgumentNullException("identifier");
      }
      if(!behaviour.IsDefinedValue())
      {
        throw new ArgumentException("Behaviour must be a defined enumeration constant.", "behaviour");
      }

      _identifier = identifier;
      _behaviour = behaviour;
      _shortNames = new HashSet<string>(shortNames?? new string[0]);
      _longNames = new HashSet<string>(longNames?? new string[0]);
    }

    #endregion
  }
}

