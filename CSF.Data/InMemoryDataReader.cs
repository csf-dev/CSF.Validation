//
// InMemoryDataReader.cs
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
using System.Linq;
using System.Data;
using System.Text;

namespace CSF.Data
{
  /// <summary>
  /// An implementation of <see cref="IDataReader"/> that makes use of a number of in-memory string arrays to provide
  /// the backing store.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This type is best-used for injecting dummy data during testing.
  /// </para>
  /// </remarks>
  public class InMemoryDataReader : IDataReader
  {
    #region constants

    /// <summary>
    /// A prefix that is used to name column headings when no headings have been supplied.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The zero-based column index is appended to this prefix in order to create the column heading name.
    /// </para>
    /// </remarks>
    public static readonly string HeaderlessColumnNamePrefix = "Column";

    #endregion

    #region fields

    private string[][,] _dataSources;
    private bool _isDisposed, _isClosed, _firstRowsAreHeaders;
    private int _currentDatatableIndex, _currentRowIndex;

    #endregion

    #region getting data from the reader

    /// <summary>
    /// Gets the data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the data could not be converted to the desired format.
    /// </exception>
    public virtual bool GetBoolean (int i)
    {
      return Boolean.Parse(this.GetString(i));
    }

    /// <summary>
    /// Gets the first byte of data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the data could not be converted to the desired format.
    /// </exception>
    public virtual byte GetByte (int i)
    {
      byte[] buffer = new byte[1];
      this.GetBytes(i, 0, buffer, 0, 1);

      return buffer[0];
    }

    /// <summary>
    /// Gets the data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <param name='fieldOffset'>
    /// The offset within the data from which to begin getting bytes.
    /// </param>
    /// <param name='buffer'>
    /// The buffer to which the output should be copied.
    /// </param>
    /// <param name='bufferoffset'>
    /// The offset within the buffer from which to begin copying bytes.
    /// </param>
    /// <param name='length'>
    /// The maximum number of bytes to copy to the buffer.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the data could not be converted to the desired format.
    /// </exception>
    public virtual long GetBytes (int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
    {
      var stringVal = this.GetString(i);
      return Encoding.UTF8.GetBytes(stringVal, (int) fieldOffset, length, buffer, bufferoffset);
    }

    /// <summary>
    /// Gets the first character of data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the data could not be converted to the desired format.
    /// </exception>
    public virtual char GetChar (int i)
    {
      char[] buffer = new char[1];
      this.GetChars(i, 0, buffer, 0, 1);

      return buffer[0];
    }

    /// <summary>
    /// Gets the data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <param name='fieldOffset'>
    /// The offset within the data from which to begin getting characters.
    /// </param>
    /// <param name='buffer'>
    /// The buffer to which the output should be copied.
    /// </param>
    /// <param name='bufferoffset'>
    /// The offset within the buffer from which to begin copying characters.
    /// </param>
    /// <param name='length'>
    /// The maximum number of characters to copy to the buffer.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the data could not be converted to the desired format.
    /// </exception>
    public virtual long GetChars (int i, long fieldOffset, char[] buffer, int bufferoffset, int length)
    {
      var stringVal = this.GetString(i);
      stringVal.CopyTo((int) fieldOffset, buffer, bufferoffset, length);

      return stringVal.Length;
    }

    /// <summary>
    /// Gets the data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the data could not be converted to the desired format.
    /// </exception>
    public virtual DateTime GetDateTime (int i)
    {
      return DateTime.Parse(this.GetString(i));
    }

    /// <summary>
    /// Gets the data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the data could not be converted to the desired format.
    /// </exception>
    public virtual decimal GetDecimal (int i)
    {
      return Decimal.Parse(this.GetString(i));
    }

    /// <summary>
    /// Gets the data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the data could not be converted to the desired format.
    /// </exception>
    public virtual double GetDouble (int i)
    {
      return Double.Parse(this.GetString(i));
    }

    /// <summary>
    /// Gets the data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the data could not be converted to the desired format.
    /// </exception>
    public virtual float GetFloat (int i)
    {
      return Single.Parse(this.GetString(i));
    }

    /// <summary>
    /// Gets the data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the data could not be converted to the desired format.
    /// </exception>
    public virtual Guid GetGuid (int i)
    {
      return new Guid(this.GetString(i));
    }

    /// <summary>
    /// Gets the data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the data could not be converted to the desired format.
    /// </exception>
    public virtual short GetInt16 (int i)
    {
      return Int16.Parse(this.GetString(i));
    }

    /// <summary>
    /// Gets the data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the data could not be converted to the desired format.
    /// </exception>
    public virtual int GetInt32 (int i)
    {
      return Int32.Parse(this.GetString(i));
    }

    /// <summary>
    /// Gets the data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the data could not be converted to the desired format.
    /// </exception>
    public virtual long GetInt64 (int i)
    {
      return Int64.Parse(this.GetString(i));
    }

    /// <summary>
    /// Gets the data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    public virtual string GetString (int i)
    {
      string output = this.GetDataAtColumn(i);

      if(output == null)
      {
        throw new InvalidOperationException("The data at column " + i + " is null. Use IsDBNull(i) to detect this.");
      }

      return output;
    }

    /// <summary>
    /// Gets the data from the current row, at the requested column index.
    /// </summary>
    /// <returns>
    /// The data from column <paramref name="i"/>, typed in the requested format.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index of the desired column.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// If the current instance is closed (see <see cref="IsClosed"/>), or the current data-row is invalid (for example,
    /// if <see cref="Read"/> has been used to move past the last row of data) or the data in requested column is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If <paramref name="i"/> is less than zero or greater than the maximum column index.
    /// </exception>
    public virtual object GetValue (int i)
    {
      return this.GetString(i);
    }

    /// <summary>
    /// Gets the data from all columns in the current row.
    /// </summary>
    /// <returns>
    /// A value that indicates the number of non-null data values present in the <paramref name="values"/> buffer.
    /// </returns>
    /// <param name='values'>
    /// A buffer of objects that receives the data.
    /// </param>
    public virtual int GetValues (object[] values)
    {
      if(values == null)
      {
        throw new ArgumentNullException("values");
      }

      int output = 0;

      for(int i = 0; i < values.Length && this.IsColumnIndexValid(i); i++)
      {
        values[i] = this.GetDataAtColumn(i);
        if(values[i] != null)
        {
          output++;
        }
      }

      return output;
    }

    /// <summary>
    /// Determines whether the data at the specified column index is null or not.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the data is null; <c>false</c> otherwise.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index to inspect.
    /// </param>
    public virtual bool IsDBNull (int i)
    {
      return (this.GetDataAtColumn(i) == null);
    }

    /// <summary>
    /// Gets the data stored in the current row, within the column of the given name.
    /// </summary>
    /// <param name='name'>
    /// The column name from which to get the data.
    /// </param>
    public virtual object this[string name] {
      get {
        int columnIndex = this.GetOrdinal(name);
        return this.GetValue(columnIndex);
      }
    }

    /// <summary>
    /// Gets the data stored in the current row, within the column identified by the given column index.
    /// </summary>
    /// <param name='i'>
    /// The zero-based column index from which to get the data.
    /// </param>
    public virtual object this[int i] {
      get {
        return this.GetValue(i);
      }
    }

    #endregion

    #region cleanup

    /// <summary>
    /// Gets a value indicating whether this instance is closed.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is closed; otherwise, <c>false</c>.
    /// </value>
    public virtual bool IsClosed {
      get {
        return _isClosed;
      }
    }

    /// <summary>
    /// Releases all resource used by the <see cref="CSF.Data.InMemoryDataReader"/> object.
    /// </summary>
    /// <remarks>
    /// Call <see cref="Dispose"/> when you are finished using the <see cref="CSF.Data.InMemoryDataReader"/>. The
    /// <see cref="Dispose"/> method leaves the <see cref="CSF.Data.InMemoryDataReader"/> in an unusable state. After
    /// calling <see cref="Dispose"/>, you must release all references to the <see cref="CSF.Data.InMemoryDataReader"/>
    /// so the garbage collector can reclaim the memory that the <see cref="CSF.Data.InMemoryDataReader"/> was occupying.
    /// </remarks>
    public virtual void Dispose ()
    {
      this.Close();

      if(_isDisposed)
      {
        throw new InvalidOperationException("This instance has already been disposed.");
      }

      _isDisposed = true;
    }

    /// <summary>
    /// Closes the current instance.
    /// </summary>
    public virtual void Close ()
    {
      _isClosed = true;
    }

    #endregion

    #region unsupported functionality

    /// <summary>
    /// This method is unsupported at this time.
    /// </summary>
    public virtual DataTable GetSchemaTable ()
    {
      throw new NotImplementedException("This method is presently unsupported in an InMemoryDataReader.");
    }

    /// <summary>
    /// This method is unsupported at this time.
    /// </summary>
    public virtual IDataReader GetData (int i)
    {
      throw new NotImplementedException("This method is presently unsupported in an InMemoryDataReader.");
    }

    #endregion

    #region informational and misc

    /// <summary>
    /// Gets a descriptive name for the underlying data-type of the data at the specified column.
    /// </summary>
    /// <returns>
    /// A human-readable data-type name.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index to inspect.
    /// </param>
    public virtual string GetDataTypeName(int i)
    {
      return this.GetFieldType(i).Name;
    }

    /// <summary>
    /// Gets a <c>System.Type</c> for the underlying data-type of the data at the specified column.
    /// </summary>
    /// <returns>
    /// The data-type.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index to inspect.
    /// </param>
    public virtual Type GetFieldType (int i)
    {
      if(!this.IsColumnIndexValid(i))
      {
        throw new ArgumentOutOfRangeException("i", "Invalid column index");
      }

      return typeof(string);
    }

    /// <summary>
    /// Gets the column name/header for the given column index.
    /// </summary>
    /// <returns>
    /// The column name.
    /// </returns>
    /// <param name='i'>
    /// The zero-based column index to inspect.
    /// </param>
    public virtual string GetName (int i)
    {
      if(!this.IsColumnIndexValid(i))
      {
        throw new ArgumentOutOfRangeException("i", "Invalid column index");
      }

      return _firstRowsAreHeaders? this.GetDataTable()[0,i] : String.Format("{0}{1}",
                                                                            HeaderlessColumnNamePrefix,
                                                                            i);
    }

    /// <summary>
    /// Gets the count of the columns in the current data-table, or zero if there is no current table.
    /// </summary>
    public virtual int FieldCount {
      get {
        int output;

        if(this.IsClosed
           || _currentDatatableIndex >= _dataSources.GetLength(0))
        {
          output = 0;
        }
        else
        {
          output = this.GetDataTable().GetLength(1);
        }

        return output;
      }
    }

    /// <summary>
    /// Gets the depth, this value will always return zero in this implementation.
    /// </summary>
    public virtual int Depth {
      get {
        return 0;
      }
    }

    /// <summary>
    /// Gets the records affected, this value will always return either zero or minus-one in this implementation.
    /// </summary>
    public virtual int RecordsAffected {
      get {
        return this.IsClosed? 0 : -1;
      }
    }

    /// <summary>
    /// Advances the current data reader instance to the next 'table' of results.
    /// </summary>
    /// <remarks>
    /// This method would be used when traversing multiple sets of results, such as when a batch of queries executes
    /// sequentially, returning many resultsets.
    /// </remarks>
    /// <returns>
    /// <c>true</c> if there was a result to move to; <c>false</c> otherwise.
    /// </returns>
    public virtual bool NextResult ()
    {
      if(this.IsClosed)
      {
        throw new InvalidOperationException("Cannot advance to next result: the current reader instance is closed.");
      }

      _currentRowIndex = -1;
      return ++_currentDatatableIndex < _dataSources.GetLength(0);
    }

    /// <summary>
    /// Advances the current data reader instance to the next result row.
    /// </summary>
    /// <returns>
    /// <c>true</c> if there was a row to move to; <c>false</c> otherwise.
    /// </returns>
    public virtual bool Read ()
    {
      if(this.IsClosed)
      {
        throw new InvalidOperationException("Cannot advance to next row: the current reader instance is closed.");
      }

      bool output;

      if(_currentDatatableIndex >= _dataSources.GetLength(0))
      {
        output = false;
      }
      else if(_currentRowIndex == -1)
      {
        _currentRowIndex = _firstRowsAreHeaders? 1 : 0;
        output = _currentRowIndex < this.GetDataTable().GetLength(0);
      }
      else
      {
        output = ++_currentRowIndex < this.GetDataTable().GetLength(0);
      }

      return output;
    }

    /// <summary>
    /// Gets the zero-based column index (ordinal) for a given column name/header.
    /// </summary>
    /// <returns>
    /// The column index corresponding to the column name.
    /// </returns>
    /// <param name='name'>
    /// The column name for which to get an index.
    /// </param>
    public virtual int GetOrdinal(string name)
    {
      if(name == null)
      {
        throw new ArgumentNullException("name");
      }
      else if(this.IsClosed)
      {
        throw new InvalidOperationException("Cannot get ordinal: the current reader instance is closed.");
      }

      this.CheckDataTableIsValid();

      int output = -1;

      if(_firstRowsAreHeaders)
      {
        var dataTable = this.GetDataTable();

        for(int i = 0; i < dataTable.GetLength(1); i++)
        {
          if(dataTable[0,i].Equals(name))
          {
            output = i;
            break;
          }
        }

        if(output == -1)
        {
          for(int i = 0; i < dataTable.GetLength(1); i++)
          {
            if(dataTable[0,i].ToLowerInvariant().Equals(name.ToLowerInvariant()))
            {
              output = i;
              break;
            }
          }
        }
      }
      else
      {
        if(name.ToLowerInvariant().StartsWith(HeaderlessColumnNamePrefix.ToLowerInvariant()))
        {
          int outputAttempt;
          if(Int32.TryParse(name.Substring(HeaderlessColumnNamePrefix.Length), out outputAttempt)
             && this.IsColumnIndexValid(outputAttempt))
          {
            output = outputAttempt;
          }
        }
      }

      if(output == -1)
      {
        throw new IndexOutOfRangeException("The specified name could not be found amongst the column headings.");
      }

      return output;
    }

    #endregion

    #region private methods

    private string GetDataAtColumn(int columnIndex)
    {
      return this.GetDataAtColumn(columnIndex, _currentRowIndex);
    }

    private string GetDataAtColumn(int columnIndex, int rowIndex)
    {
      if(this.IsClosed)
      {
        throw new InvalidOperationException("Cannot get column data: the current reader instance is closed.");
      }

      this.CheckRowIsValid(rowIndex);

      if(!IsColumnIndexValid(columnIndex))
      {
        throw new IndexOutOfRangeException("Invalid column index.  Index must be greater than or equal to zero " +
                                           "and less than 'FieldCount'.");
      }

      return this.GetDataTable()[rowIndex, columnIndex];
    }

    private void CheckDataTableIsValid()
    {
      this.CheckDataTableIsValid(_currentDatatableIndex);
    }

    private void CheckDataTableIsValid(int tableIndex)
    {
      if(tableIndex >= _dataSources.GetLength(0))
      {
        throw new InvalidOperationException("The current operation is invalid as there is no current data.  Have you " +
                                            "used NextResult() to move past the last dataset?");
      }
    }

    private void CheckRowIsValid(int rowIndex)
    {
      this.CheckDataTableIsValid();

      if(rowIndex < 0)
      {
        throw new InvalidOperationException("The current operation is invalid as there is no current row.  Have you " +
                                            "forgotten to use Read() at least once?");
      }
      else if(rowIndex >= this.GetDataTable().GetLength(0))
      {
        throw new InvalidOperationException("The current operation is invalid as there is no current row.  Have you " +
                                            "used Read() to move past the last row?");
      }
    }

    private bool IsColumnIndexValid(int index)
    {
      this.CheckDataTableIsValid();

      return (index >= 0 && index < this.GetDataTable().GetLength(1));
    }

    private string[,] GetDataTable()
    {
      return this.GetDataTable(_currentDatatableIndex);
    }

    private string[,] GetDataTable(int tableIndex)
    {
      this.CheckDataTableIsValid(tableIndex);

      return _dataSources[tableIndex];
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Data.InMemoryDataReader"/> class.
    /// </summary>
    /// <param name='firstRowsAsHeaders'>
    /// If <c>true</c> then the first row of every data source provided is treated as containing header information.
    /// </param>
    /// <param name='dataSources'>
    /// A collection of the data-sources to use for this reader instance.
    /// </param>
    public InMemoryDataReader(bool firstRowsAsHeaders, params string[][,] dataSources)
    {
      if(dataSources == null)
      {
        throw new ArgumentNullException("dataSources");
      }
      else if(dataSources.GetLength(0) == 0)
      {
        throw new ArgumentException("At least one data-source must be provided", "dataSources");
      }
      else if(firstRowsAsHeaders && dataSources.Any(x => x.GetLength(0) == 0))
      {
        throw new ArgumentException("When the first row of data-sources is to be used as headers, every data source " +
                                    "must have at least one row.",
                                    "dataSources");
      }

      _dataSources = dataSources;
      _isDisposed = false;
      _isClosed = false;
      _currentDatatableIndex = 0;
      _firstRowsAreHeaders = firstRowsAsHeaders;
      _currentRowIndex = -1;
    }

    #endregion
  }
}

