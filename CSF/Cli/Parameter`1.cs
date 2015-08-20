//
// Parameter`1.cs
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
  /// A strongly-typed implementation of an <see cref="IParameter"/>.
  /// </summary>
  public class Parameter<TParameterValue> : IParameter<TParameterValue>, IParameter
  {
    #region fields
    
    private IList<string> _shortNames, _longNames;
    private TParameterValue _value;
    private ParameterBehaviour _behaviour;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets a unique identifier by which this parameter may be identified.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public object Identifier
    {
      get;
      set;
    }
    
    /// <summary>
    /// Gets a collection of 'short' names (typically only 1 character long each) for this parameter.
    /// </summary>
    /// <value>
    /// The short names.
    /// </value>
    public IList<string> ShortNames
    {
      get {
        return _shortNames;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException ("_value");
        }
        
        _shortNames = value;
      }
    }
    
    /// <summary>
    /// Gets a collection of 'long' names (may be over 1 character each, but with no spaces) for this parameter.
    /// </summary>
    /// <value>
    /// The long names.
    /// </value>
    public IList<string> LongNames
    {
      get {
        return _longNames;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException ("_value");
        }
        
        _longNames = value;
      }
    }
    
    /// <summary>
    /// Gets the behaviour of this parameter.
    /// </summary>
    /// <value>
    /// The behaviour.
    /// </value>
    public ParameterBehaviour Behaviour
    {
      get {
        return _behaviour;
      }
      set {
        if(!Enum.IsDefined(typeof(ParameterBehaviour), value))
        {
          throw new ArgumentException("Unrecognised behaviour.");
        }
        
        _behaviour = value;
      }
    }
    
    #endregion

    #region methods
    
    /// <summary>
    /// Reads the value associated with this parameter.
    /// </summary>
    /// <returns>
    /// The value.
    /// </returns>
    public TParameterValue GetValue()
    {
      if(_value == null)
      {
        throw new InvalidOperationException("The current instance does not contain a stored value.");
      }
      
      return _value;
    }
    
    /// <summary>
    /// Sets the value stored in this instance.
    /// </summary>
    /// <param name='value'>
    /// The value to store.
    /// </param>
    public void SetValue(TParameterValue value)
    {
      _value = value;
    }
    
    /// <summary>
    /// Checks that the current instance is a valid parameter.
    /// </summary>
    /// <exception cref='InvalidOperationException'>
    /// Is thrown if the current instance is invalid.
    /// </exception>
    public void CheckValidity()
    {
      if(this.ShortNames.Count == 0 && this.LongNames.Count == 0)
      {
        throw new InvalidOperationException("Invalid parameter - contains no command line names.");
      }
    }
    
    #endregion
    
    #region explicit IParameter implementation
    
    object IParameter.GetValue()
    {
      return this.GetValue();
    }
    
    void IParameter.SetValue(string value)
    {
      this.SetValue((TParameterValue) Convert.ChangeType(value, typeof(TParameterValue)));
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public Parameter()
    {
      this.ShortNames = new List<string>();
      this.LongNames = new List<string>();
      this.Behaviour = ParameterBehaviour.ValueOptional;
    }
    
    #endregion
  }
}

