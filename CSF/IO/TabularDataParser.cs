//  
//  TabularDataParser.cs
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

/*
 * This type was originally inspired by the following work by Andreas Knab:
 * * http://knab.ws/blog/index.php?/archives/3-CSV-file-parser-and-writer-in-C-Part-1.html
 * * http://knab.ws/blog/index.php?url=archives/10-CSV-file-parser-and-writer-in-C-Part-2.html
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSF.IO
{
  /// <summary>
  /// Represents a concrete <see cref="ITabularDataParser"/> that is able to read from &amp; write to tabular string data
  /// structures.
  /// </summary>
  public class TabularDataParser : ITabularDataParser
  {
    #region constants
    
    /// <summary>
    /// Constant value indicates the 'end of file' character.
    /// </summary>
    public static readonly char EndOfFile = '\0';
    
    /// <summary>
    /// Constant value contains the default/hardcoded length of the read buffer.
    /// </summary>
    public static readonly int DefaultBufferSize = 4096;
    
    #endregion
    
    #region fields
    
    private TabularDataFormat _format;
    private int _readBufferSize;
    
    #endregion
    
    #region properties
  
    /// <summary>
    /// Gets or sets information about the format of data that this parser reads and writes.
    /// </summary>
    /// <value>
    /// The format.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public TabularDataFormat Format
    {
      get {
        return _format;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _format = value;
      }
    }
    
    /// <summary>
    /// Gets or sets the size (in bytes) of the buffer used for read operations.
    /// </summary>
    /// <value>
    /// The size of the read buffer.
    /// </value>
    public virtual int ReadBufferSize
    {
      get {
        return _readBufferSize;
      }
      set {
        if(value < 1)
        {
          throw new ArgumentOutOfRangeException("value", "Read buffer cannot be less than one byte");
        }
        
        _readBufferSize = value;
      }
    }
    
    #endregion
    
    #region ITabularDataParser implementation
    
    /// <summary>
    /// Reads the specified string-based data and returns a collection representing the tabular data.
    /// </summary>
    /// <param name='stringData'>
    /// Tabular data formatted as a string.
    /// </param>
    public virtual TabularData Read(string stringData)
    {
      TabularData output;
      
      using(TextReader reader = new StringReader(stringData))
      {
        output = this.Read(reader);
      }
      
      return output;
    }
    
    /// <summary>
    /// Reads the specified string-based data and returns a collection representing the tabular data.
    /// </summary>
    /// <param name='stringDataReader'>
    /// A <see cref="TextReader"/> that reads tabular data, formatted as a string.
    /// </param>
    public virtual TabularData Read(TextReader stringDataReader)
    {
      TabularDataStream readHelper = this.GetDataStream(stringDataReader);
      int currentRow = 1, columnCount = 0;
      TabularDataBuilder output = null;
      
      try
      {
        foreach(var row in readHelper)
        {
          if(output == null)
          {
            columnCount = row.Count;
            output = new TabularDataBuilder(columnCount);
          }

          if(row.Count != columnCount
             && row.Count == 1
             && this.Format.TolerateEmptyRows)
          {
            continue;
          }
          else if(row.Count != columnCount)
          {
            string message = String.Format("Invalid tabular data; column count does not match first column at row {0}.",
                                           currentRow);
            throw new TabularDataReadException(message);
          }

          output.AddRow(row);
          currentRow++;
        }
      }
      catch(TabularDataReadException)
      {
        throw;
      }
      catch(Exception ex)
      {
        string message = String.Format("Invalid tabular data; an error was encountered whilst parsing row {0}.",
                                       currentRow);
        throw new TabularDataReadException(message, ex);
      }

      return output.Build();
    }
    
    /// <summary>
    /// Writes the specified data to a string-based format.
    /// </summary>
    /// <param name='data'>
    /// The data to write.
    /// </param>
    /// <param name='options'>
    /// The options to use when writing the data.
    /// </param>
    public virtual string Write(IList<IList<string>> data, TabularDataWriteOptions options)
    {
      StringBuilder output = new StringBuilder();
      
      using(TextWriter writer = new StringWriter(output))
      {
        this.Write(data, writer, options);
      }
      
      return output.ToString();
    }
    
    /// <summary>
    /// Writes the specified data to a string-based format.
    /// </summary>
    /// <param name='data'>
    /// The tabular data structure.
    /// </param>
    public virtual string Write(IList<IList<string>> data)
    {
      return this.Write(data, this.Format.DefaultWriteOptions);
    }
    
    /// <summary>
    /// Write the specified data to a given <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='data'>
    /// The tabular data structure.
    /// </param>
    /// <param name='stringDataWriter'>
    /// A <see cref="TextWriter"/> to write the output to.
    /// </param>
    /// <param name='options'>
    /// The options to use when writing the data.
    /// </param>
    public virtual void Write(IList<IList<string>> data, TextWriter stringDataWriter, TabularDataWriteOptions options)
    {
      var tabularData = new TabularListData(data);
      this.Write(tabularData, stringDataWriter, options);
    }
    
    /// <summary>
    /// Write the specified data to a given <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='data'>
    /// The tabular data structure.
    /// </param>
    /// <param name='stringDataWriter'>
    /// A <see cref="TextWriter"/> to write the output to.
    /// </param>
    public virtual void Write(IList<IList<string>> data, TextWriter stringDataWriter)
    {
      this.Write(data, stringDataWriter, this.Format.DefaultWriteOptions);
    }
    
    /// <summary>
    /// Writes the specified data to a string-based format.
    /// </summary>
    /// <param name='data'>
    /// The data to write.
    /// </param>
    public virtual string Write(string[,] data)
    {
      return this.Write(data, this.Format.DefaultWriteOptions);
    }
    
    /// <summary>
    /// Writes the specified data to a string-based format.
    /// </summary>
    /// <param name='data'>
    /// The data to write.
    /// </param>
    /// <param name='options'>
    /// Additional write options to use when writing.
    /// </param>
    public virtual string Write(string[,] data, TabularDataWriteOptions options)
    {
      StringBuilder output = new StringBuilder();
      
      using(TextWriter writer = new StringWriter(output))
      {
        this.Write(data, writer, options);
      }
      
      return output.ToString();
    }
    
    /// <summary>
    /// Write the specified data to a given <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='data'>
    /// A <see cref="TextWriter"/> to write the data to.
    /// </param>
    /// <param name='stringDataWriter'>
    /// String data writer.
    /// </param>
    public virtual void Write(string[,] data, TextWriter stringDataWriter)
    {
      this.Write(data, stringDataWriter, this.Format.DefaultWriteOptions);
    }
    
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
    public virtual void Write(string[,] data, TextWriter stringDataWriter, TabularDataWriteOptions options)
    {
      var tabularData = new TabularArrayData(data);
      this.Write(tabularData, stringDataWriter, options);
    }

    /// <summary>
    /// Write the specified data to a given <see cref="TextWriter"/>.
    /// </summary>
    /// <param name='data'>
    /// The tabular data structure.
    /// </param>
    /// <param name='stringDataWriter'>
    /// A <see cref="TextWriter"/> to write the output to.
    /// </param>
    /// <param name='options'>
    /// The options to use when writing the data.
    /// </param>
    public virtual void Write(TabularData data, TextWriter stringDataWriter, TabularDataWriteOptions options)
    {
      if(data == null)
      {
        throw new ArgumentNullException ("data");
      }

      int rowCount = data.GetRowCount();
      for(int rowNumber = 0; rowNumber < rowCount; rowNumber++)
      {
        IList<string> row = data[rowNumber];
        this.Write(row, stringDataWriter, options);

        if(rowNumber < rowCount - 1)
        {
          stringDataWriter.Write(this.Format.RowDelimiter);
        }
      }
    }

    #endregion

    #region methods
    
    /// <summary>
    /// Gets an instance of a helper type that assists in the parsing of data from a <paramref name="reader"/>.
    /// </summary>
    /// <returns>
    /// The data stream helper type.
    /// </returns>
    /// <param name='reader'>
    /// A <see cref="TextReader"/>.
    /// </param>
    protected virtual TabularDataStream GetDataStream(TextReader reader)
    {
      return new TabularDataStream(this, reader);
    }
    
    /// <summary>
    /// Writes a single row of data to the <paramref name="writer"/>.
    /// </summary>
    /// <param name='row'>
    /// The row to write
    /// </param>
    /// <param name='writer'>
    /// The <see cref="TextWriter"/> to write to
    /// </param>
    /// <param name='options'>
    /// A set of <see cref="TabularDataWriteOptions"/> providing additional options for the writer.
    /// </param>
    protected virtual void Write(IList<string> row, TextWriter writer, TabularDataWriteOptions options)
    {
      if(row == null)
      {
        throw new ArgumentNullException ("row");
      }
      else if(writer == null)
      {
        throw new ArgumentNullException ("writer");
      }
      
      for(int columnPosition = 0; columnPosition < row.Count; columnPosition++)
      {
        string valueToWrite = row[columnPosition]?? String.Empty;
        bool writeColumnDelimiter = (columnPosition < row.Count - 1);
        
        if(this.Format.QuoteWhenWriting(valueToWrite, options))
        {
          writer.Write(String.Format("{0}{1}{0}{2}",
                                     this.Format.QuotationCharacter,
                                     valueToWrite.Replace(this.Format.QuotationCharacter.ToString(),
                                                          String.Concat(this.Format.QuotationEscapeCharacter,
                                                                        this.Format.QuotationCharacter)),
                                     writeColumnDelimiter? this.Format.ColumnDelimiter.ToString() : String.Empty));
        }
        else
        {
          writer.Write(String.Format("{0}{1}",
                                     valueToWrite,
                                     writeColumnDelimiter? this.Format.ColumnDelimiter.ToString() : String.Empty));
        }
      }
    }

    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.IO.TabularDataParser"/> class.
    /// </summary>
    /// <param name='format'>
    /// The format information for the type of file that the current instance will read.
    /// </param>
    public TabularDataParser(TabularDataFormat format)
    {
      this.Format = format;
      this.ReadBufferSize = DefaultBufferSize;
    }
    
    #endregion
  }
}

