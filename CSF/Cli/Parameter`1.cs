//  
//  ParameterT.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2012 Craig Fowler
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
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

