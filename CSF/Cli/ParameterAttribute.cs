//
// ParameterAttribute.cs
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
  /// Attribute used to decorate members of a type that implements <see cref="IParsedParameters"/>, providing
  /// information about the behaviour of the parameter.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public class ParameterAttribute : Attribute
  {
    #region fields
    
    private ParameterBehaviour _behaviour;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets or sets a short name associated with the parameter.
    /// </summary>
    /// <value>
    /// The short name.
    /// </value>
    public string ShortName
    {
      get;
      set;
    }
    
    /// <summary>
    /// Gets or sets a long name associated with the parameter.
    /// </summary>
    /// <value>
    /// The long name.
    /// </value>
    public string LongName
    {
      get;
      set;
    }
    
    /// <summary>
    /// Gets or sets the behaviour of the parameter.
    /// </summary>
    /// <value>
    /// The behaviour.
    /// </value>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public ParameterBehaviour Behaviour
    {
      get {
        return _behaviour;
      }
      set {
        if(!Enum.IsDefined(typeof(ParameterBehaviour), value))
        {
          throw new ArgumentException("Unrecognised parameter behaviour.");
        }
        
        _behaviour = value;
      }
    }
    
    #endregion
    
    #region constructor
  
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Cli.ParameterAttribute"/> class.
    /// </summary>
    /// <param name='behaviour'>
    /// The parameter behaviour.
    /// </param>
    public ParameterAttribute(ParameterBehaviour behaviour)
    {
      this.Behaviour = behaviour;
      this.ShortName = null;
      this.LongName = null;
    }
    
    #endregion
  }
}

