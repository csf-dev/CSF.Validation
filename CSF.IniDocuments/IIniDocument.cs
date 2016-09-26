//
// IIniDocument.cs
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
using System.IO;
using System.Collections.Generic;

namespace CSF.IniDocuments
{
  /// <summary>
  /// Interface for a document that implements the INI file specification.
  /// </summary>
  public interface IIniDocument : IIniSection
  {
    #region properties
    
    /// <summary>
    /// Gets or sets the sections that this document contains.
    /// </summary>
    /// <value>
    /// A collection of the sections.
    /// </value>
    IDictionary<string, IIniSection> Sections { get; set; }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Write this instance (as an INI-formatted file) to the specified path.
    /// </summary>
    /// <param name='filePath'>
    /// The path to save the written file.
    /// </param>
    void Write(string filePath);
    
    /// <summary>
    /// Write this instance to a string (as INI-formatted data).
    /// </summary>
    string Write();
    
    /// <summary>
    /// Write this instance (as INI-formatted data) to the specified <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='writer'>
    /// A <see cref="TextWriter"/>.
    /// </param>
    void Write(TextWriter writer);
    
    #endregion
  }
}

