//  
//  TabularDataList.cs
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

namespace CSF.IO
{
  /// <summary>
  /// A specialised collection type for handling <see cref="System.String"/> data that is organised in a 2-dimensional
  /// tabular structure.  This type enforces constraints on the rows of data, ensuring that each has the correct number
  /// of columns.
  /// </summary>
  public class TabularDataList : List<IList<string>>, IList<IList<string>>
  {
    #region fields
    
    private int _columnCount;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// Gets or sets the desired count of columns.
    /// </summary>
    /// <value>
    /// The column count.
    /// </value>
    public virtual int ColumnCount
    {
      get {
        return _columnCount;
      }
      private set {
        if(value < 0)
        {
          throw new ArgumentOutOfRangeException("value", "Column count may not be less than zero.");
        }
        
        _columnCount = value;
      }
    }
    
    #endregion
    
    #region overrides to List<IList<string>>
    
    /// <summary>
    /// Adds a new row to this list.
    /// </summary>
    /// <param name='item'>
    /// The row to add
    /// </param>
    public new void Add(IList<string> item)
    {
      if(item == null)
      {
        throw new ArgumentNullException("item");
      }
      else if(item.Count != this.ColumnCount)
      {
        throw new ArgumentException("Row to add has an incorrect count of columns.");
      }
      
      base.Add(item);
    }
    
    /// <summary>
    /// Inserts a row at the specified index.
    /// </summary>
    /// <param name='index'>
    /// The index at which to add the row.
    /// </param>
    /// <param name='item'>
    /// The row to insert.
    /// </param>
    public new void Insert(int index, IList<string> item)
    {
      if(item == null)
      {
        throw new ArgumentNullException ("item");
      }
      else if(item.Count != this.ColumnCount)
      {
        throw new ArgumentException("Row to insert has an incorrect count of columns.");
      }
      
      base.Insert(index, item);
    }
    
    /// <summary>
    /// Gets or sets the row at the specified index.
    /// </summary>
    /// <param name='index'>
    /// Index.
    /// </param>
    public new IList<string> this[int index]
    {
      get {
        return base[index];
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException ("item");
        }
        else if(value.Count != this.ColumnCount)
        {
          throw new ArgumentException("Row has an incorrect count of columns.");
        }
        
        base[index] = value;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Creates a new row with <see cref="ColumnCount"/> columns.
    /// </summary>
    /// <returns>
    /// The row.
    /// </returns>
    public IList<string> CreateRow()
    {
      return new string[this.ColumnCount];
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.IO.TabularDataList"/> class.
    /// </summary>
    /// <param name='columnCount'>
    /// The desired column count.
    /// </param>
    public TabularDataList(int columnCount)
    {
      this.ColumnCount = columnCount;
    }
    
    #endregion

    #region static methods

    /// <summary>
    /// Creates a new <see cref="TabularDataList"/> instance from a given two-dimensional string array.
    /// </summary>
    /// <returns>
    /// The created tabular data list.
    /// </returns>
    /// <param name='twoDimensionalArray'>
    /// A two-dimensional string array.
    /// </param>
    public static TabularDataList CreateFrom(string[,] twoDimensionalArray)
    {
      if(twoDimensionalArray == null)
      {
        throw new ArgumentNullException("twoDimensionalArray");
      }

      TabularDataList output = new TabularDataList(twoDimensionalArray.GetLength(1));

      for(int row = 0; row < twoDimensionalArray.GetLength(0); row++)
      {
        var newRow = output.CreateRow();
        for(int col = 0; col < newRow.Count; col++)
        {
          newRow[col] = twoDimensionalArray[row,col];
        }
        output.Add(newRow);
      }

      return output;
    }

    #endregion
  }
}

