//
//  TabularArrayData.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2015 Craig Fowler
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
using System.Linq;

namespace CSF.IO
{
  /// <summary>
  /// Immutable type representing a <see cref="TabularData"/> built from a two-dimensional array.
  /// </summary>
  public class TabularArrayData : TabularData
  {
    #region fields

    private string[,] _data;

    #endregion

    #region properties

    /// <summary>
    /// Gets a collection of <c>System.String</c> representing a given row of data.
    /// </summary>
    /// <param name="row">Row.</param>
    public override IList<string> this [int row]
    {
      get {
        return Enumerable
          .Range(_data.GetLowerBound(1), _data.GetLength(1))
          .Select(col => _data[row,col])
          .ToList();
      }
    }

    #endregion

    #region implemented abstract members of TabularData

    /// <summary>
    /// Gets the count of rows in the data.
    /// </summary>
    /// <returns>The row count.</returns>
    public override int GetRowCount()
    {
      return _data.GetLength(0);
    }

    /// <summary>
    /// Gets the count of columns in the data.
    /// </summary>
    /// <returns>The column count.</returns>
    public override int GetColumnCount()
    {
      return _data.GetLength(1);
    }

    /// <summary>
    /// Gets the data item at the given row and column.
    /// </summary>
    /// <returns>The data.</returns>
    /// <param name="row">Row.</param>
    /// <param name="column">Column.</param>
    public override string GetData(int row, int column)
    {
      return _data[row,column];
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.IO.TabularArrayData"/> class.
    /// </summary>
    /// <param name="data">Data.</param>
    public TabularArrayData(string[,] data)
    {
      if(data == null)
      {
        throw new ArgumentNullException("data");
      }

      _data = data;
    }

    #endregion
  }
}

