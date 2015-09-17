//
// ParameterParserBuilder.cs
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
  /// Builder type creates an instance of the non-generic <see cref="IParameterParser"/> and registers parameters which
  /// are to be parsed.
  /// </summary>
  public class ParameterParserBuilder
  {
    #region fields

    private Dictionary<object,ParameterMapping> _mappings;
    private bool _built;

    #endregion

    #region methods

    /// <summary>
    /// Adds a 'flag' type parameter.  This is a parameter which is either present (<c>true</c>) or not present
    /// (<c>false</c>).
    /// </summary>
    /// <remarks>
    /// <para>
    /// At least one of <paramref name="shortName"/> or <paramref name="longName"/> must contain a non-null value.
    /// Short names may only be a single character in length.  Long names may be multiple characters in length, and may
    /// contain any alphanumeric characters, as well as the dash and underscore symbols.
    /// </para>
    /// </remarks>
    /// <returns>The current builder instance.</returns>
    /// <param name="identifier">An identifier for the parameter added.</param>
    /// <param name="shortName">An optional short (single character) name for the parameter.</param>
    /// <param name="longName">An optional long name (one or more characters) name for the parameter.</param>
    public ParameterParserBuilder AddFlag(object identifier,
                                          string shortName = null,
                                          string longName = null)
    {
      string[]
        shortNames = (shortName != null)? new [] { shortName } : null,
        longNames = (longName != null)? new [] { longName } : null;

      return this.AddFlag(identifier, shortNames: shortNames, longNames: longNames);
    }

    /// <summary>
    /// Adds a 'flag' type parameter.  This is a parameter which is either present (<c>true</c>) or not present
    /// (<c>false</c>).
    /// </summary>
    /// <remarks>
    /// <para>
    /// At least one of <paramref name="shortNames"/> or <paramref name="longNames"/> must contain at least one non-null
    /// value. Short names may only be a single character in length.  Long names may be multiple characters in length,
    /// and may contain any alphanumeric characters, as well as the dash and underscore symbols.
    /// </para>
    /// </remarks>
    /// <returns>The current builder instance.</returns>
    /// <param name="identifier">An identifier for the parameter added.</param>
    /// <param name="shortNames">An optional collection of short (single character) names for the parameter.</param>
    /// <param name="longNames">An optional collection of long (one or more characters) names for the parameter.</param>
    public ParameterParserBuilder AddFlag(object identifier,
                                          IEnumerable<string> shortNames = null,
                                          IEnumerable<string> longNames = null)
    {
      if(identifier == null)
      {
        throw new ArgumentNullException("identifier");
      }

      this.CheckNotBuilt();

      _mappings.Add(identifier, new ParameterMapping(identifier,
                                                     ParameterBehaviour.Switch,
                                                     shortNames: shortNames,
                                                     longNames: longNames));

      return this;
    }

    /// <summary>
    /// Adds a 'value' type parameter.  This is a parameter which if present, has a string value stored within.
    /// </summary>
    /// <returns>The current builder instance.</returns>
    /// <remarks>
    /// <para>
    /// At least one of <paramref name="shortName"/> or <paramref name="longName"/> must contain a non-null value.
    /// Short names may only be a single character in length.  Long names may be multiple characters in length, and may
    /// contain any alphanumeric characters, as well as the dash and underscore symbols.
    /// </para>
    /// </remarks>
    /// <param name="identifier">An identifier for the parameter added.</param>
    /// <param name="shortName">An optional short (single character) name for the parameter.</param>
    /// <param name="longName">An optional long name (one or more characters) name for the parameter.</param>
    /// <param name="optional">A value which indicates whether the value is optional or not (default <c>true</c>).</param>
    public ParameterParserBuilder AddValue(object identifier,
                                           string shortName = null,
                                           string longName = null,
                                           bool optional = true)
    {
      string[]
        shortNames = (shortName != null)? new [] { shortName } : null,
        longNames = (longName != null)? new [] { longName } : null;

      return this.AddValue(identifier, shortNames: shortNames, longNames: longNames, optional: optional);
    }

    /// <summary>
    /// Adds a 'value' type parameter.  This is a parameter which if present, has a string value stored within.
    /// </summary>
    /// <remarks>
    /// <para>
    /// At least one of <paramref name="shortNames"/> or <paramref name="longNames"/> must contain at least one non-null
    /// value. Short names may only be a single character in length.  Long names may be multiple characters in length,
    /// and may contain any alphanumeric characters, as well as the dash and underscore symbols.
    /// </para>
    /// </remarks>
    /// <returns>The current builder instance.</returns>
    /// <param name="identifier">An identifier for the parameter added.</param>
    /// <param name="shortNames">An optional collection of short (single character) names for the parameter.</param>
    /// <param name="longNames">An optional collection of long (one or more characters) names for the parameter.</param>
    /// <param name="optional">A value which indicates whether the value is optional or not (default <c>true</c>).</param>
    public ParameterParserBuilder AddValue(object identifier,
                                           IEnumerable<string> shortNames = null,
                                           IEnumerable<string> longNames = null,
                                           bool optional = true)
    {
      if(identifier == null)
      {
        throw new ArgumentNullException("identifier");
      }

      this.CheckNotBuilt();

      var behaviour = optional? ParameterBehaviour.ValueOptional : ParameterBehaviour.ValueRequired;

      _mappings.Add(identifier, new ParameterMapping(identifier,
                                                     behaviour,
                                                     shortNames: shortNames,
                                                     longNames: longNames));

      return this;
    }

    /// <summary>
    /// Builds a parameter parser from the current instance.
    /// </summary>
    public IParameterParser Build()
    {
      this.CheckNotBuilt();

      _built = true;

      return new PosixParameterParser(_mappings);
    }

    /// <summary>
    /// Checks that the current instance has not already been used to build a parameter parser.
    /// </summary>
    private void CheckNotBuilt()
    {
      if(_built)
      {
        throw new InvalidOperationException("A parameter parser builder may be used only once.  Construct a new " +
                                            "builder if you wish to build another parser.");
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Cli.ParameterParserBuilder"/> class.
    /// </summary>
    public ParameterParserBuilder()
    {
      _mappings = new Dictionary<object, ParameterMapping>();
    }

    #endregion
  }
}

