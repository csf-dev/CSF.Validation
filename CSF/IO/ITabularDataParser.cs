//  
//  ITabularDataParser.cs
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

