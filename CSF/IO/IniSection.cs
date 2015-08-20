//
// IniSection.cs
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
using System.Text.RegularExpressions;
using System.IO;

namespace CSF.IO
{
  /// <summary>
  /// <para>Represents a section of configuration settings within an <see cref="IniDocument"/>.</para>
  /// </summary>
  public class IniSection : Dictionary<string, string>, IIniSection
  {
    #region constants
    
    private const string VALID_KEY = @"^\S(.*\S)?$";
    private static readonly Regex ValidKey = new Regex(VALID_KEY, RegexOptions.Compiled);
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Gets and sets settings by their key.</para>
    /// </summary>
    /// <param name="key">
    /// A <see cref="System.String"/>
    /// </param>
    public new string this[string key]
    {
      get {
        ValidateKey(key);
        return base[key];
      }
      set {
        ValidateKey(key);
        base[key] = value;
      }
    }
    
    #endregion
    
    #region public methods
    
    /// <summary>
    /// <para>Adds a new setting to this instance.</para>
    /// </summary>
    /// <param name="key">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="value">
    /// A <see cref="System.String"/>
    /// </param>
    public new void Add (string key, string value)
    {
      ValidateKey(key);
      base.Add(key, value);
    }
    
    /// <summary>
    /// <para>Removes a key from the current instance.</para>
    /// </summary>
    /// <param name="key">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public new bool Remove (string key)
    {
      ValidateKey(key);
      return base.Remove(key);
    }
    
    /// <summary>
    /// Writes this instance to the given <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='writer'>
    /// A <see cref="TextWriter"/> to write the output to.
    /// </param>
    /// <param name='name'>
    /// The name of this section.
    /// </param>
    public void WriteTo(TextWriter writer, string name)
    {
      bool writeNewLine = false;
      
      if(writer == null)
      {
        throw new ArgumentNullException ("writer");
      }
      
      if(!String.IsNullOrEmpty(name))
      {
        writer.Write("[{0}]", name);
        writeNewLine = true;
      }
      
      foreach(string key in this.Keys)
      {
        writer.Write("{2}{0} = {1}", key, this[key], writeNewLine? "\n" : String.Empty);
        writeNewLine = true;
      }
    }
    
    /// <summary>
    /// Writes this instance to the given <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='writer'>
    /// A <see cref="TextWriter"/> to write the output to.
    /// </param>
    protected void WriteTo(TextWriter writer)
    {
      this.WriteTo(writer, null);
    }
    
    #endregion
    
    #region private methods
    
    /// <summary>
    /// Validates a string key.
    /// </summary>
    /// <param name='key'>
    /// The key to validate
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown if <paramref name="key"/> is null.
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown if <paramref name="key"/> is not a valid key within an INI document.
    /// </exception>
    private void ValidateKey(string key)
    {
      if(key == null)
      {
        throw new ArgumentNullException("key");
      }
      
      if(!ValidKey.Match(key).Success)
      {
        throw new ArgumentException(String.Format("Invalid INI key: '{0}'", key));
      }
    }
    
    #endregion
  
    #region constructor
    
    /// <summary>
    /// <para>Initialises the current instance.</para>
    /// </summary>
    public IniSection() : base() {}
  
    #endregion
  }
}
