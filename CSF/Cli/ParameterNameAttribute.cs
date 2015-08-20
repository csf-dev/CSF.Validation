//
// ParameterNameAttribute.cs
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

namespace CSF.Cli
{
  /// <summary>
  /// Attribute that complements a <see cref="ParameterAttribute"/>, providing an alternate short or long name for the
  /// parameter.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
  public class ParameterNameAttribute : Attribute
  {
    #region fields
    
    private string _name;
    
    #endregion
    
    #region properties
  
    /// <summary>
    /// Gets or sets a value indicating whether this instance is a long name.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is a long name; otherwise, <c>false</c>.
    /// </value>
    public bool IsLongName
    {
      get;
      set;
    }
  
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public string Name
    {
      get {
        return _name;
      }
      set {
        if (value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _name = value;
      }
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Cli.ParameterNameAttribute"/> class.
    /// </summary>
    /// <param name='name'>
    /// Name.
    /// </param>
    public ParameterNameAttribute(string name)
    {
      this.Name = name;
      this.IsLongName = false;
    }
    
    #endregion
  }
}

