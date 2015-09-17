//
// ITabularDataParser.cs
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
using System.IO;

namespace CSF.IO
{
  /// <summary>
  /// Interface for a type that is capable of parsing tabular data structures to/from string-based datastructures.
  /// </summary>
  public interface ITabularDataParser
  {
    #region properties
    
    /// <summary>
    /// Gets or sets information about the format of data that this parser reads and writes.
    /// </summary>
    /// <value>
    /// The format.
    /// </value>
    TabularDataFormat Format { get; }
    
    /// <summary>
    /// Gets or sets the size (in bytes) of the buffer used for read operations.
    /// </summary>
    /// <value>
    /// The size of the read buffer.
    /// </value>
    int ReadBufferSize { get; set; }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Reads the specified string-based data and returns a collection representing the tabular data.
    /// </summary>
    /// <param name='stringData'>
    /// Tabular data formatted as a string.
    /// </param>
    TabularData Read(string stringData);
    
    /// <summary>
    /// Reads the specified string-based data and returns a collection representing the tabular data.
    /// </summary>
    /// <param name='stringDataReader'>
    /// A <see cref="TextReader"/> that reads tabular data, formatted as a string.
    /// </param>
    TabularData Read(TextReader stringDataReader);
    
    /// <summary>
    /// Writes the specified data to a string-based format.
    /// </summary>
    /// <param name='data'>
    /// The data to write.
    /// </param>
    string Write(IList<IList<string>> data);
    
    /// <summary>
    /// Writes the specified data to a string-based format.
    /// </summary>
    /// <param name='data'>
    /// The data to write.
    /// </param>
    /// <param name='options'>
    /// Additional write options to use when writing.
    /// </param>
    string Write(IList<IList<string>> data, TabularDataWriteOptions options);
    
    /// <summary>
    /// Write the specified data to a given <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='data'>
    /// A <see cref="TextWriter"/> to write the data to.
    /// </param>
    /// <param name='stringDataWriter'>
    /// String data writer.
    /// </param>
    void Write(IList<IList<string>> data, TextWriter stringDataWriter);
    
    /// <summary>
    /// Write the specified data to a given <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='data'>
    /// The data to write.
    /// </param>
    /// <param name='stringDataWriter'>
    /// A <see cref="TextWriter"/> to write the data to.
    /// </param>
    /// <param name='options'>
    /// Additional write options to use when writing.
    /// </param>
    void Write(IList<IList<string>> data, TextWriter stringDataWriter, TabularDataWriteOptions options);
    
    /// <summary>
    /// Writes the specified data to a string-based format.
    /// </summary>
    /// <param name='data'>
    /// The data to write.
    /// </param>
    string Write(string[,] data);
    
    /// <summary>
    /// Writes the specified data to a string-based format.
    /// </summary>
    /// <param name='data'>
    /// The data to write.
    /// </param>
    /// <param name='options'>
    /// Additional write options to use when writing.
    /// </param>
    string Write(string[,] data, TabularDataWriteOptions options);
    
    /// <summary>
    /// Write the specified data to a given <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='data'>
    /// A <see cref="TextWriter"/> to write the data to.
    /// </param>
    /// <param name='stringDataWriter'>
    /// String data writer.
    /// </param>
    void Write(string[,] data, TextWriter stringDataWriter);
    
    /// <summary>
    /// Write the specified data to a given <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='data'>
    /// The data to write.
    /// </param>
    /// <param name='stringDataWriter'>
    /// A <see cref="TextWriter"/> to write the data to.
    /// </param>
    /// <param name='options'>
    /// Additional write options to use when writing.
    /// </param>
    void Write(string[,] data, TextWriter stringDataWriter, TabularDataWriteOptions options);
    
    #endregion
  }
}

