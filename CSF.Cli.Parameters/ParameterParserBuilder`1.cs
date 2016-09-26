//
// ParameterParserBuilder1.cs
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
using System.Linq.Expressions;
using CSF.Reflection;

namespace CSF.Cli.Parameters
{
  /// <summary>
  /// Builder type creates an instance of the generic <see cref="T:IParameterParser{T}"/> and registers mappings
  /// between object properties and parameters.
  /// </summary>
  public class ParameterParserBuilder<TParsed> where TParsed : class,new()
  {
    #region fields

    private Dictionary<PropertyInfo,ParameterMapping> _mappings;
    private PropertyInfo _remainingArguments;
    private bool _built;

    #endregion

    #region methods

    /// <summary>
    /// Adds a 'flag' type parameter.  This is a parameter which is either present (<c>true</c>) or not present
    /// (<c>false</c>).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Flag-type parameters are represented by boolean properties on the parsed object.  If the flag is present then
    /// the property is set to <c>true</c>.  If the flag is not present then the property value is left unchanged.
    /// </para>
    /// <para>
    /// At least one of <paramref name="shortName"/> or <paramref name="longName"/> must contain a non-null value.
    /// Short names may only be a single character in length.  Long names may be multiple characters in length, and may
    /// contain any alphanumeric characters, as well as the dash and underscore symbols.
    /// </para>
    /// </remarks>
    /// <returns>The current builder instance.</returns>
    /// <param name="propertyExpression">A property expression, indicating a property of a type with which this parameter is associated.</param>
    /// <param name="shortName">An optional short (single character) name for the parameter.</param>
    /// <param name="longName">An optional long name (one or more characters) name for the parameter.</param>
    public ParameterParserBuilder<TParsed> AddFlag(Expression<Func<TParsed,bool>> propertyExpression,
                                                   string shortName = null,
                                                   string longName = null)
    {
      string[]
        shortNames = (shortName != null)? new [] { shortName } : null,
        longNames = (longName != null)? new [] { longName } : null;

      return this.AddFlag(propertyExpression, shortNames: shortNames, longNames: longNames);
    }

    /// <summary>
    /// Adds a 'flag' type parameter.  This is a parameter which is either present (<c>true</c>) or not present
    /// (<c>false</c>).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Flag-type parameters are represented by boolean properties on the parsed object.  If the flag is present then
    /// the property is set to <c>true</c>.  If the flag is not present then the property value is left unchanged.
    /// </para>
    /// <para>
    /// At least one of <paramref name="shortNames"/> or <paramref name="longNames"/> must contain at least one non-null
    /// value. Short names may only be a single character in length.  Long names may be multiple characters in length,
    /// and may contain any alphanumeric characters, as well as the dash and underscore symbols.
    /// </para>
    /// </remarks>
    /// <returns>The current builder instance.</returns>
    /// <param name="propertyExpression">A property expression, indicating a property of a type with which this parameter is associated.</param>
    /// <param name="shortNames">An optional collection of short (single character) names for the parameter.</param>
    /// <param name="longNames">An optional collection of long (one or more characters) names for the parameter.</param>
    public ParameterParserBuilder<TParsed> AddFlag(Expression<Func<TParsed,bool>> propertyExpression,
                                                   IEnumerable<string> shortNames = null,
                                                   IEnumerable<string> longNames = null)
    {
      if(propertyExpression == null)
      {
        throw new ArgumentNullException("propertyExpression");
      }

      this.CheckNotBuilt();

      var property = Reflect.Property(propertyExpression);
      if(property.ReflectedType != typeof(TParsed))
      {
        string message = String.Format("Property expression must indicate a first-class property of type `{0}'.",
                                       typeof(TParsed).FullName);
        throw new ArgumentException(message, "propertyExpression");
      }

      _mappings.Add(property, new ParameterMapping(property,
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
    /// Value-type parameters are represented by string properties on the parsed object.  If the parameter is present
    /// with a value then the property is set to that value.  If no value is present then the behaviour depends upon
    /// whether the parameter is marked <paramref name="optional"/> or not.  If the parameter is optional and no value
    /// is present (the parameter is presented like a flag), then the property is set to <c>String.Empty</c>.  If the
    /// parameter is not optional and no value is present then the property is left unchanged.  In any case; if the
    /// parameter is not present at all then the property is lefy unchanged.  For best effect here, ensure that object
    /// properties which represent value parameters are initialised in their construction to <c>null</c>.
    /// </para>
    /// <para>
    /// At least one of <paramref name="shortName"/> or <paramref name="longName"/> must contain a non-null value.
    /// Short names may only be a single character in length.  Long names may be multiple characters in length, and may
    /// contain any alphanumeric characters, as well as the dash and underscore symbols.
    /// </para>
    /// </remarks>
    /// <param name="propertyExpression">A property expression, indicating a property of a type with which this parameter is associated.</param>
    /// <param name="shortName">An optional short (single character) name for the parameter.</param>
    /// <param name="longName">An optional long name (one or more characters) name for the parameter.</param>
    /// <param name="optional">A value which indicates whether the value is optional or not (default <c>true</c>).</param>
    public ParameterParserBuilder<TParsed> AddValue(Expression<Func<TParsed,string>> propertyExpression,
                                                    string shortName = null,
                                                    string longName = null,
                                                    bool optional = true)
    {
      string[]
        shortNames = (shortName != null)? new [] { shortName } : null,
        longNames = (longName != null)? new [] { longName } : null;

      return this.AddValue(propertyExpression, shortNames: shortNames, longNames: longNames, optional: optional);
    }

    /// <summary>
    /// Adds a 'value' type parameter.  This is a parameter which if present, has a string value stored within.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Value-type parameters are represented by string properties on the parsed object.  If the parameter is present
    /// with a value then the property is set to that value.  If no value is present then the behaviour depends upon
    /// whether the parameter is marked <paramref name="optional"/> or not.  If the parameter is optional and no value
    /// is present (the parameter is presented like a flag), then the property is set to <c>String.Empty</c>.  If the
    /// parameter is not optional and no value is present then the property is left unchanged.  In any case; if the
    /// parameter is not present at all then the property is lefy unchanged.  For best effect here, ensure that object
    /// properties which represent value parameters are initialised in their construction to <c>null</c>.
    /// </para>
    /// <para>
    /// At least one of <paramref name="shortNames"/> or <paramref name="longNames"/> must contain at least one non-null
    /// value. Short names may only be a single character in length.  Long names may be multiple characters in length,
    /// and may contain any alphanumeric characters, as well as the dash and underscore symbols.
    /// </para>
    /// </remarks>
    /// <returns>The current builder instance.</returns>
    /// <param name="propertyExpression">A property expression, indicating a property of a type with which this parameter is associated.</param>
    /// <param name="shortNames">An optional collection of short (single character) names for the parameter.</param>
    /// <param name="longNames">An optional collection of long (one or more characters) names for the parameter.</param>
    /// <param name="optional">A value which indicates whether the value is optional or not (default <c>true</c>).</param>
    public ParameterParserBuilder<TParsed> AddValue(Expression<Func<TParsed,string>> propertyExpression,
                                                    IEnumerable<string> shortNames = null,
                                                    IEnumerable<string> longNames = null,
                                                    bool optional = true)
    {
      if(propertyExpression == null)
      {
        throw new ArgumentNullException("propertyExpression");
      }

      this.CheckNotBuilt();

      var property = Reflect.Property(propertyExpression);
      if(property.ReflectedType != typeof(TParsed))
      {
        string message = String.Format("Property expression must indicate a first-class property of type `{0}'.",
                                       typeof(TParsed).FullName);
        throw new ArgumentException(message, "propertyExpression");
      }

      var behaviour = optional? ParameterBehaviour.ValueOptional : ParameterBehaviour.ValueRequired;

      _mappings.Add(property, new ParameterMapping(property,
                                                   behaviour,
                                                   shortNames: shortNames,
                                                   longNames: longNames));

      return this;
    }

    /// <summary>
    /// Indicates a property in which to store any remaining non-parameter arguments received on the commandline.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Not all arguments passed on the commandline are parameters, some may be arguments with meaning based upon their
    /// position.  This method indicates a property of the parsed object which will be populated with those remaining
    /// arguments.  This property must be an <c>System.Collections.Generic.IList&lt;System.String&gt;</c>.
    /// </para>
    /// </remarks>
    /// <returns>The current builder instance.</returns>
    /// <param name="propertyExpression">A property expression, indicating a property of a type with which this parameter is associated.</param>
    public ParameterParserBuilder<TParsed> RemainingArguments(Expression<Func<TParsed,IList<string>>> propertyExpression)
    {
      if(propertyExpression == null)
      {
        throw new ArgumentNullException("propertyExpression");
      }

      this.CheckNotBuilt();

      var property = Reflect.Property(propertyExpression);
      if(property.ReflectedType != typeof(TParsed))
      {
        string message = String.Format("Property expression must indicate a first-class property of type `{0}'.",
                                       typeof(TParsed).FullName);
        throw new ArgumentException(message, "propertyExpression");
      }

      _remainingArguments = property;

      return this;
    }

    /// <summary>
    /// Builds a parameter parser from the current instance.
    /// </summary>
    public IParameterParser<TParsed> Build()
    {
      this.CheckNotBuilt();

      _built = true;

      return new PosixParameterParser<TParsed>(_mappings, _remainingArguments);
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
    /// Initializes a new instance of the <see cref="T:CSF.Cli.ParameterParserBuilder{TParsed}"/> class.
    /// </summary>
    public ParameterParserBuilder()
    {
      _mappings = new Dictionary<PropertyInfo, ParameterMapping>();
    }

    #endregion
  }
}

